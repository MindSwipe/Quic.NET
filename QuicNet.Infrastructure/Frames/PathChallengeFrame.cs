using QuickNet.Utilities;
using System;

namespace QuicNet.Infrastructure.Frames
{
    public class PathChallengeFrame : Frame
    {
        public override byte Type => 0x1a;

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
