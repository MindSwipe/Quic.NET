using QuickNet.Utilities;
using System;

namespace QuicNet.Infrastructure.Frames
{
    public class NewConnectionIdFrame : Frame
    {
        public override byte Type => 0x18;

        public override void Decode(ByteArray array)
        {
            throw new NotImplementedException();
        }

        public override byte[] Encode()
        {
            throw new NotImplementedException();
        }
    }
}
