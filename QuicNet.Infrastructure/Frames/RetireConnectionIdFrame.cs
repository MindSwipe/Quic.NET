using QuickNet.Utilities;
using System;

namespace QuicNet.Infrastructure.Frames
{
    public class RetireConnectionIdFrame : Frame
    {
        public override byte Type => 0x19;

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
