using QuickNet.Utilities;
using System;

namespace QuicNet.Infrastructure.Frames
{
    public class StreamsBlockedFrame : Frame
    {
        public override byte Type => 0x16;

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
