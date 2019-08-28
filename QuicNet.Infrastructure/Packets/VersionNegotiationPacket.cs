using System;

namespace QuicNet.Infrastructure.Packets
{
    public class VersionNegotiationPacket : Packet
    {
        public override byte Type => throw new NotImplementedException();

        public override void Decode(byte[] packet)
        {
            throw new NotImplementedException();
        }

        public override byte[] Encode()
        {
            throw new NotImplementedException();
        }
    }
}
