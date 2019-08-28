using QuicNet.Streams;

namespace QuicNet.Context
{
    /// <summary>
    ///     Wrapper to represent the stream.
    /// </summary>
    public class QuicStreamContext
    {
        /// <summary>
        ///     Data received
        /// </summary>
        public byte[] Data { get; private set; }

        /// <summary>
        ///     Unique stream identifier
        /// </summary>
        public ulong StreamId { get; }

        /// <summary>
        ///     Send data to the client.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool Send(byte[] data)
        {
            if (Stream.CanSendData() == false)
                return false;

            // Ignore empty packets
            if (data == null || data.Length <= 0)
                return true;

            return false;
        }

        public void Close()
        {
            // TODO: Close out the stream by sending appropriate packets to the peer
        }


        #region Internal

        internal QuicStream Stream { get; set; }

        /// <summary>
        ///     Internal constructor to prevent creating the context outside the scope of Quic.
        /// </summary>
        /// <param name="stream">The stream to set</param>
        internal QuicStreamContext(QuicStream stream)
        {
            Stream = stream;
            StreamId = stream.StreamId;
        }

        internal void SetData(byte[] data)
        {
            Data = data;
        }

        #endregion
    }
}