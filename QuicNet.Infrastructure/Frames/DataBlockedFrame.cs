using QuickNet.Utilities;
using System.Collections.Generic;

namespace QuicNet.Infrastructure.Frames
{
    public class DataBlockedFrame : Frame
    {
        public override byte Type => 0x14;

        public VariableInteger DataLimit { get; set; }

        public override void Decode(ByteArray array)
        {
            array.ReadByte();
            DataLimit = array.ReadVariableInteger();
        }

        public override byte[] Encode()
        {
            var result = new List<byte> {Type};
            byte[] dataLimit = DataLimit;

            result.AddRange(dataLimit);

            return result.ToArray();
        }
    }
}
