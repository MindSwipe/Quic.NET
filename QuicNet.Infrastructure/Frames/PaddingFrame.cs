using QuickNet.Utilities;
using System.Collections.Generic;

namespace QuicNet.Infrastructure.Frames
{
    public class PaddingFrame : Frame
    {
        public override byte Type => 0x00;

        public override void Decode(ByteArray array)
        {
            array.ReadByte();
        }

        public override byte[] Encode()
        {
            var data = new List<byte> {Type};
            return data.ToArray();
        }
    }
}
