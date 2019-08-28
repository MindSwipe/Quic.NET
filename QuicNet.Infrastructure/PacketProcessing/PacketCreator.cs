using QuicNet.Infrastructure.Frames;
using QuicNet.Infrastructure.Packets;

namespace QuicNet.Infrastructure.PacketProcessing
{
    public class PacketCreator
    {
        private readonly NumberSpace _ns;

        private readonly uint _peerConnectionId;

        public PacketCreator(uint peerConnectionId)
        {
            _ns = new NumberSpace();
            _peerConnectionId = peerConnectionId;
        }

        public ShortHeaderPacket CreateConnectionClosePacket(ErrorCode code, string reason)
        {
            var packet = new ShortHeaderPacket
            {
                PacketNumber = _ns.Get(), DestinationConnectionId = (byte) _peerConnectionId
            };

            packet.AttachFrame(new ConnectionCloseFrame(code, reason));
            return packet;
        }

        public ShortHeaderPacket CreateDataPacket(ulong streamId, byte[] data)
        {
            var packet = new ShortHeaderPacket
            {
                PacketNumber = _ns.Get(), DestinationConnectionId = (byte) _peerConnectionId
            };

            packet.AttachFrame(new StreamFrame(streamId, data, 0, true));
            return packet;
        }

        public InitialPacket CreateServerBusyPacket()
        {
            return new InitialPacket();
        }

        public ShortHeaderPacket CreateShortHeaderPacket()
        {
            return new ShortHeaderPacket();
        }
    }
}
