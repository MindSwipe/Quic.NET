using QuickNet.Utilities;
using System.Collections.Generic;

namespace QuicNet.Infrastructure.Packets
{
    public class ShortHeaderPacket : Packet
    {
        public override byte Type => 0x43; // 0100 0011;

        public byte DestinationConnectionId { get; set; }

        public uint PacketNumber { get; set; }
        
        public override void Decode(byte[] packet)
        {
            var array = new ByteArray(packet);
            var type = array.ReadByte();
            DestinationConnectionId = array.ReadByte();
            PacketNumber = array.ReadUInt32();

            DecodeFrames(array);
        }

        public override byte[] Encode()
        {
            var frames = EncodeFrames();

            var result = new List<byte>();
            result.Add(Type);
            result.Add(DestinationConnectionId);
            result.AddRange(ByteUtilities.GetBytes(PacketNumber));
            result.AddRange(frames);

            return result.ToArray();
        }
    }
}
