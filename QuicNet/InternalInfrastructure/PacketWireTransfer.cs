using System.Net;
using System.Net.Sockets;
using QuicNet.Exceptions;
using QuicNet.Infrastructure.Packets;

namespace QuicNet.InternalInfrastructure
{
    internal class PacketWireTransfer
    {
        private readonly UdpClient _client;

        private readonly Unpacker _unpacker;
        private IPEndPoint _peerEndpoint;

        public PacketWireTransfer(UdpClient client, IPEndPoint peerEndpoint)
        {
            _client = client;
            _peerEndpoint = peerEndpoint;
            _unpacker = new Unpacker();
        }

        public Packet ReadPacket()
        {
            // Await response for sucessfull connection creation by the server
            var peerData = _client.Receive(ref _peerEndpoint);
            if (peerData == null)
                throw new QuicConnectivityException("Server did not respond properly.");

            return _unpacker.Unpack(peerData);
        }

        public bool SendPacket(Packet packet)
        {
            var data = packet.Encode();
            var sent = _client.Send(data, data.Length, _peerEndpoint);

            return sent > 0;
        }

        public IPEndPoint LastTransferEndpoint()
        {
            return _peerEndpoint;
        }
    }
}