using QuickNet.Utilities;
using System.Collections.Generic;

namespace QuicNet.Infrastructure.Frames
{
    public class MaxStreamsFrame : Frame
    {
        private readonly StreamType _appliesTo;

        public override byte Type => 0x12;

        public VariableInteger MaximumStreams { get; set; }

        public MaxStreamsFrame()
        {

        }

        public MaxStreamsFrame(ulong maximumStreamId, StreamType appliesTo)
        {
            _appliesTo = appliesTo;
            MaximumStreams = new VariableInteger(maximumStreamId);
        }

        public override void Decode(ByteArray array)
        {
            var type = array.ReadByte();
            MaximumStreams = array.ReadVariableInteger();
        }

        public override byte[] Encode()
        {
            var result = new List<byte> {Type};
            byte[] streamId = MaximumStreams;
            result.AddRange(streamId);

            return result.ToArray();
        }
    }
}
