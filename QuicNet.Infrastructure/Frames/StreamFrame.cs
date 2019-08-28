using QuickNet.Utilities;
using System.Collections.Generic;

namespace QuicNet.Infrastructure.Frames
{
    public class StreamFrame : Frame
    {
        private byte _actualType = 0x08;

        public override byte Type => 0x08;

        public VariableInteger StreamId { get; set; }

        public VariableInteger Offset { get; set; }

        public VariableInteger Length { get; set; }

        public byte[] StreamData { get; set; }

        public StreamId ConvertedStreamId { get; set; }

        public bool EndOfStream { get; set; }

        public StreamFrame()
        {

        }

        public StreamFrame(ulong streamId, byte[] data, ulong offset, bool eos)
        {
            StreamId = streamId;
            StreamData = data;
            Length = (ulong)data.Length;
            Offset = offset;
            EndOfStream = eos;
        }

        public override void Decode(ByteArray array)
        {
            var type = array.ReadByte();

            var offBit = (byte)(type & 0x04);
            var lenBit = (byte)(type & 0x02);
            var finBit = (byte)(type & 0x01);

            StreamId = array.ReadVariableInteger();
            if (offBit > 0)
                Offset = array.ReadVariableInteger();
            if (lenBit > 0)
                Length = array.ReadVariableInteger();
            if (finBit > 0)
                EndOfStream = true;
            
            StreamData = array.ReadBytes((int)Length.Value);
            ConvertedStreamId = QuickNet.Utilities.StreamId.Decode(ByteUtilities.GetBytes(StreamId.Value));
        }

        public override byte[] Encode()
        {
            if (Offset != null && Offset.Value > 0)
                _actualType = (byte)(_actualType | 0x04);
            if (Length != null && Length.Value > 0)
                _actualType = (byte)(_actualType | 0x02);
            if (EndOfStream)
                _actualType = (byte)(_actualType | 0x01);

            var offBit = (byte)(_actualType & 0x04);
            var lenBit = (byte)(_actualType & 0x02);

            var result = new List<byte> {_actualType};
            byte[] streamId = StreamId;
            result.AddRange(streamId);

            if (offBit > 0)
            {
                byte[] offset = Offset;
                result.AddRange(offset);
            }

            if (lenBit > 0)
            {
                byte[] length = Length;
                result.AddRange(length);
            }

            result.AddRange(StreamData);

            return result.ToArray();
        }
    }
}
