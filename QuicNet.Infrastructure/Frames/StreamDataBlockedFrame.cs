using QuickNet.Utilities;
using System.Collections.Generic;

namespace QuicNet.Infrastructure.Frames
{
    public class StreamDataBlockedFrame : Frame
    {
        public override byte Type => 0x15;

        public VariableInteger StreamId { get; set; }

        public VariableInteger StreamDataLimit { get; set; }

        public StreamDataBlockedFrame()
        {

        }

        public StreamDataBlockedFrame(ulong streamId, ulong streamDataLimit)
        {
            StreamId = streamId;
            StreamDataLimit = streamDataLimit;
        }

        public override void Decode(ByteArray array)
        {
            array.ReadByte();
            StreamId = array.ReadVariableInteger();
            StreamDataLimit = array.ReadVariableInteger();
        }

        public override byte[] Encode()
        {
            var result = new List<byte> {Type};

            byte[] streamId = StreamId;
            byte[] streamDataLimit = StreamDataLimit;

            result.AddRange(streamId);
            result.AddRange(streamDataLimit);

            return result.ToArray();
        }
    }
}
