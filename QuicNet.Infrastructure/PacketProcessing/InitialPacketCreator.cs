using QuicNet.Infrastructure.Frames;
using QuicNet.Infrastructure.Packets;
using QuicNet.Infrastructure.Settings;

namespace QuicNet.Infrastructure.PacketProcessing
{
    public class InitialPacketCreator
    {
        public InitialPacket CreateInitialPacket(byte sourceConnectionId, byte destinationConnectionId)
        {
            var packet = new InitialPacket
            {
                PacketNumber = 0,
                SourceConnectionId = sourceConnectionId,
                DestinationConnectionId = destinationConnectionId,
                Version = QuicVersion.CurrentVersion
            };

            var length = packet.Encode().Length;
            var padding = QuicSettings.PMTU - length;

            for (var i = 0; i < padding; i++)
                packet.AttachFrame(new PaddingFrame());

            return packet;
        }

        public VersionNegotiationPacket CreateVersionNegotiationPacket()
        {
            return new VersionNegotiationPacket();
        }
    }
}
