using System.Net;
using System.Net.Sockets;
using QuicNet.Connections;
using QuicNet.Exceptions;
using QuicNet.Infrastructure.Frames;
using QuicNet.Infrastructure.PacketProcessing;
using QuicNet.Infrastructure.Packets;
using QuicNet.InternalInfrastructure;

namespace QuicNet
{
    /// <summary>
    ///     Quic Client. Used for sending and receiving data from a Quic Server.
    /// </summary>
    public class QuicClient
    {
        private readonly UdpClient _client;

        private readonly InitialPacketCreator _packetCreator;

        private QuicConnection _connection;
        private IPEndPoint _peerIp;

        private PacketWireTransfer _pwt;

        public QuicClient()
        {
            _client = new UdpClient();
            _packetCreator = new InitialPacketCreator();
        }

        /// <summary>
        ///     Connect to a remote server.
        /// </summary>
        /// <param name="ip">Ip Address</param>
        /// <param name="port">Port</param>
        /// <returns></returns>
        public QuicConnection Connect(string ip, int port)
        {
            // Establish socket connection
            _peerIp = new IPEndPoint(IPAddress.Parse(ip), port);

            // Initialize packet reader
            _pwt = new PacketWireTransfer(_client, _peerIp);

            // Start initial protocol process
            var connectionPacket = _packetCreator.CreateInitialPacket(0, 0);

            // Send the initial packet
            _pwt.SendPacket(connectionPacket);

            // Await response for sucessfull connection creation by the server
            var packet = (InitialPacket) _pwt.ReadPacket();

            HandleInitialFrames(packet);
            EstablishConnection(packet.SourceConnectionId, packet.SourceConnectionId);

            return _connection;
        }

        /// <summary>
        ///     Handles initial packet's frames. (In most cases protocol frames)
        /// </summary>
        /// <param name="packet"></param>
        private void HandleInitialFrames(Packet packet)
        {
            var frames = packet.GetFrames();
            for (var i = frames.Count - 1; i > 0; i--)
            {
                var frame = frames[i];
                if (frame is ConnectionCloseFrame ccf)
                    throw new QuicConnectivityException(ccf.ReasonPhrase);

                // Break out if the first Padding Frame has been reached
                if (frame is PaddingFrame)
                    break;
            }
        }

        /// <summary>
        ///     Create a new connection
        /// </summary>
        /// <param name="connectionId"></param>
        /// <param name="peerConnectionId"></param>
        private void EstablishConnection(uint connectionId, uint peerConnectionId)
        {
            var connection = new ConnectionData(_pwt, connectionId, peerConnectionId);
            _connection = new QuicConnection(connection);
        }
    }
}