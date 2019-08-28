﻿using System;
using System.Collections.Generic;
using QuickNet.Utilities;
using QuicNet.Exceptions;
using QuicNet.Infrastructure;
using QuicNet.Infrastructure.Frames;
using QuicNet.Infrastructure.PacketProcessing;
using QuicNet.Infrastructure.Packets;
using QuicNet.Infrastructure.Settings;
using QuicNet.InternalInfrastructure;
using QuicNet.Streams;

namespace QuicNet.Connections
{
    public class QuicConnection
    {
        private readonly PacketWireTransfer _pwt;
        private readonly Dictionary<ulong, QuicStream> _streams;
        private ulong _currentTransferRate;
        private string _lastError;
        private ConnectionState _state;

        public uint ConnectionId { get; }

        public uint PeerConnectionId { get; }

        public PacketCreator PacketCreator { get; }

        public ulong MaxData { get; private set; }

        public ulong MaxStreams { get; private set; }

        public event Action<QuicStream> OnDataReceived;

        /// <summary>
        ///     Creates a new stream for sending/receiving data.
        /// </summary>
        /// <param name="type">Type of the stream (Uni-Bidirectional)</param>
        /// <returns>A new stream instance or Null if the connection is terminated.</returns>
        public QuicStream CreateStream(StreamType type)
        {
            if (_state != ConnectionState.Open)
                return null;

            var stream = new QuicStream(this, new StreamId(0, type));
            _streams.Add(0, stream);

            return stream;
        }

        public void ProcessFrames(List<Frame> frames)
        {
            foreach (var frame in frames)
            {
                if (frame.Type == 0x01)
                    OnRstStreamFrame(frame);
                if (frame.Type == 0x04)
                    OnRstStreamFrame(frame);
                if (frame.Type >= 0x08 && frame.Type <= 0x0f)
                    OnStreamFrame(frame);
                if (frame.Type == 0x10)
                    OnMaxDataFrame(frame);
                if (frame.Type == 0x11)
                    OnMaxStreamDataFrame(frame);
                if (frame.Type >= 0x12 && frame.Type <= 0x13)
                    OnMaxStreamFrame(frame);
                if (frame.Type == 0x14)
                    OnDataBlockedFrame(frame);
                if (frame.Type == 0x1c)
                    OnConnectionCloseFrame(frame);
            }
        }

        public void IncrementRate(int length)
        {
            _currentTransferRate += (uint) length;
        }

        public bool MaximumReached()
        {
            if (_currentTransferRate >= MaxData)
                return true;

            return false;
        }

        private void OnConnectionCloseFrame(Frame frame)
        {
            var ccf = (ConnectionCloseFrame) frame;
            _state = ConnectionState.Draining;
            _lastError = ccf.ReasonPhrase;
        }

        private void OnRstStreamFrame(Frame frame)
        {
            var rsf = (ResetStreamFrame) frame;
            if (_streams.ContainsKey(rsf.StreamId))
            {
                // Find and reset the stream
                var stream = _streams[rsf.StreamId];
                stream.ResetStream(rsf);

                // Remove the stream from the connection
                _streams.Remove(rsf.StreamId);
            }
        }

        private void OnStreamFrame(Frame frame)
        {
            var sf = (StreamFrame) frame;
            if (_streams.ContainsKey(sf.ConvertedStreamId.Id) == false)
            {
                var stream = new QuicStream(this, sf.ConvertedStreamId);
                stream.ProcessData(sf);

                if ((ulong) _streams.Count < MaxStreams)
                    _streams.Add(sf.StreamId.Value, stream);
                else
                    SendMaximumStreamReachedError();
            }
            else
            {
                var stream = _streams[sf.StreamId];
                stream.ProcessData(sf);
            }
        }

        private void OnMaxDataFrame(Frame frame)
        {
            var sf = (MaxDataFrame) frame;
            if (sf.MaximumData.Value > MaxData)
                MaxData = sf.MaximumData.Value;
        }

        private void OnMaxStreamDataFrame(Frame frame)
        {
            var msdf = (MaxStreamDataFrame) frame;
            if (_streams.ContainsKey(msdf.StreamId))
            {
                // Find and set the new maximum stream data on the stream
                var stream = _streams[msdf.ConvertedStreamId.Id];
                stream.SetMaximumStreamData(msdf.MaximumStreamData.Value);
            }
        }

        private void OnMaxStreamFrame(Frame frame)
        {
            var msf = (MaxStreamsFrame) frame;
            if (msf.MaximumStreams > MaxStreams)
                MaxStreams = msf.MaximumStreams.Value;
        }

        // ReSharper disable once UnusedParameter.Local
        // See comment below
        private void OnDataBlockedFrame(Frame frame)
        {
            // TODO: Tuning of data transfer.

            // Since no stream id is present on this frame, we should be
            // stopping the connection.
            TerminateConnection();
        }

        #region Internal

        internal QuicConnection(ConnectionData connection)
        {
            _currentTransferRate = 0;
            _state = ConnectionState.Open;
            _lastError = string.Empty;
            _streams = new Dictionary<ulong, QuicStream>();
            _pwt = connection.Pwt;

            ConnectionId = connection.ConnectionId;
            PeerConnectionId = connection.PeerConnectionId;
            // Also creates a new number space
            PacketCreator = new PacketCreator(PeerConnectionId);
            MaxData = QuicSettings.MaxData;
            MaxStreams = QuicSettings.MaximumStreamId;
        }

        /// <summary>
        ///     Client only!
        /// </summary>
        /// <returns></returns>
        internal void ReceivePacket()
        {
            var packet = _pwt.ReadPacket();

            if (packet is ShortHeaderPacket)
            {
                var shp = (ShortHeaderPacket) packet;
                ProcessFrames(shp.GetFrames());
            }

            // If the connection has been closed
            if (_state == ConnectionState.Draining)
            {
                if (string.IsNullOrWhiteSpace(_lastError))
                    _lastError = "Protocol error";

                TerminateConnection();

                throw new QuicConnectivityException(_lastError);
            }
        }

        internal bool SendData(Packet packet)
        {
            return _pwt.SendPacket(packet);
        }

        internal void DataReceived(QuicStream context)
        {
            OnDataReceived?.Invoke(context);
        }

        internal void TerminateConnection()
        {
            _state = ConnectionState.Draining;
            _streams.Clear();

            ConnectionPool.RemoveConnection(ConnectionId);
        }

        internal void SendMaximumStreamReachedError()
        {
            var packet = PacketCreator.CreateConnectionClosePacket(ErrorCode.STREAM_LIMIT_ERROR,
                "Maximum number of streams reached.");
            Send(packet);
        }

        /// <summary>
        ///     Used to send protocol packets to the peer.
        /// </summary>
        /// <param name="packet"></param>
        /// <returns></returns>
        internal bool Send(Packet packet)
        {
            // Encode the packet
            var data = packet.Encode();

            // Increment the connection transfer rate
            IncrementRate(data.Length);

            // If the maximum transfer rate is reached, send FLOW_CONTROL_ERROR
            if (MaximumReached())
            {
                packet = PacketCreator.CreateConnectionClosePacket(ErrorCode.FLOW_CONTROL_ERROR,
                    "Maximum data transfer reached.");

                TerminateConnection();
            }

            // Ignore empty packets
            if (data.Length <= 0)
                return true;

            var result = _pwt.SendPacket(packet);

            return result;
        }

        #endregion
    }
}