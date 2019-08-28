using QuickNet.Utilities;
using System.Collections.Generic;

namespace QuicNet.Infrastructure.Frames
{
    public class MaxDataFrame : Frame
    {
        public override byte Type => 0x10;
        public VariableInteger MaximumData { get; set; }

        public override void Decode(ByteArray array)
        {
            array.ReadByte();
            MaximumData = array.ReadVariableInteger();
        }

        public override byte[] Encode()
        {
            var result = new List<byte>();
            byte[] maxData = MaximumData;

            result.Add(Type);
            result.AddRange(maxData);

            return result.ToArray();
        }
    }
}
