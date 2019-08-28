using QuickNet.Utilities;
using System.Collections.Generic;

namespace QuicNet.Infrastructure.Frames
{
    public class ResetStreamFrame : Frame
    {
        public override byte Type => 0x04;
        public VariableInteger StreamId { get; set; }
        public ushort ApplicationErrorCode { get; set; }
        public VariableInteger FinalOffset { get; set; }

        public override void Decode(ByteArray array)
        {
            array.ReadByte();
            StreamId = array.ReadVariableInteger();
            ApplicationErrorCode = array.ReadUInt16();
            FinalOffset = array.ReadVariableInteger();
        }

        public override byte[] Encode()
        {
            var result = new List<byte> {Type};

            byte[] streamId = StreamId;
            result.AddRange(streamId);

            result.AddRange(ByteUtilities.GetBytes(ApplicationErrorCode));

            byte[] offset = FinalOffset;
            result.AddRange(offset);

            return result.ToArray();
        }
    }
}
