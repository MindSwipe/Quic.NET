using QuickNet.Utilities;
using System.Collections.Generic;

namespace QuicNet.Infrastructure.Frames
{
    public class MaxStreamDataFrame : Frame
    {
        public override byte Type => 0x11;
        public VariableInteger StreamId { get; set; }
        public VariableInteger MaximumStreamData { get; set; }

        public StreamId ConvertedStreamId { get; set; }

        public override void Decode(ByteArray array)
        {
            array.ReadByte();
            StreamId = array.ReadVariableInteger();
            MaximumStreamData = array.ReadVariableInteger();
        }

        public override byte[] Encode()
        {
            var result = new List<byte> {Type};

            byte[] streamId = StreamId;
            result.AddRange(streamId);

            byte[] maximumStreamData = MaximumStreamData;
            result.AddRange(maximumStreamData);

            return result.ToArray();
        }
    }
}
