using QuickNet.Utilities;
using System.Collections.Generic;

namespace QuicNet.Infrastructure.Packets
{
    public class InitialPacket : Packet
    {
        public override byte Type => 0x7F | 1 << 7; // 1111 1111
        
        public byte DcilScil { get; set; }
        public byte DestinationConnectionId { get; set; }
        public byte SourceConnectionId { get; set; }
        public VariableInteger TokenLength { get; set; }
        public byte[] Token { get; set; }
        public VariableInteger Length { get; set; }
        public uint PacketNumber { get; set; }

        public InitialPacket()
        {
            DcilScil = 0;
        }

        public override void Decode(byte[] packet)
        {
            var array = new ByteArray(packet);
            array.ReadByte();
            Version = array.ReadUInt32();
            DcilScil = array.ReadByte();

            if ((DcilScil & 0xF0) != 0)
                DestinationConnectionId = array.ReadByte();
            if ((DcilScil & 0x0F) != 0)
                SourceConnectionId = array.ReadByte();

            TokenLength = array.ReadVariableInteger();
            if (TokenLength != 0)
                Token = array.ReadBytes((int)TokenLength.Value);

            Length = array.ReadVariableInteger();
            PacketNumber = array.ReadUInt32();

            Length -= 4;

            DecodeFrames(array);
        }

        public override byte[] Encode()
        {
            var frames = EncodeFrames();

            var result = new List<byte>();
            result.Add(Type);
            result.AddRange(ByteUtilities.GetBytes(Version));
            
            if (DestinationConnectionId > 0)
                DcilScil = (byte)(DcilScil | 0x50);
            if (SourceConnectionId > 0)
                DcilScil = (byte)(DcilScil | 0x05);

            result.Add(DcilScil);

            if (DestinationConnectionId > 0)
                result.Add(DestinationConnectionId);
            if (SourceConnectionId > 0)
                result.Add(SourceConnectionId);

            byte[] tokenLength = new VariableInteger(0);
            byte[] length = new VariableInteger(4 + (ulong)frames.Length);

            result.AddRange(tokenLength);
            result.AddRange(length);
            result.AddRange(ByteUtilities.GetBytes(PacketNumber));
            result.AddRange(frames);

            return result.ToArray();
        }
    }
}
