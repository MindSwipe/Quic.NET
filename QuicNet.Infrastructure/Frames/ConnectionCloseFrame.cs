using QuickNet.Utilities;
using System.Collections.Generic;

namespace QuicNet.Infrastructure.Frames
{
    public class ConnectionCloseFrame : Frame
    {
        public override byte Type => 0x1c;
        public ushort ErrorCode { get; set; }
        public VariableInteger ReasonPhraseLength { get; set; }
        public string ReasonPhrase { get; set; }

        public ConnectionCloseFrame()
        {
            ErrorCode = 0;
            ReasonPhraseLength = new VariableInteger(0);
        }

        public ConnectionCloseFrame(ErrorCode error, string reason)
        {
            ReasonPhraseLength = new VariableInteger(0);

            ErrorCode = (ushort)error;
            ReasonPhrase = reason;
        }

        public override void Decode(ByteArray array)
        {
            array.ReadByte();

            ErrorCode = array.ReadUInt16();
            ReasonPhraseLength = array.ReadVariableInteger();

            var rp = array.ReadBytes((int)ReasonPhraseLength.Value);
            ReasonPhrase = ByteUtilities.GetString(rp);
        }

        public override byte[] Encode()
        {
            var result = new List<byte>();
            result.Add(Type);

            var errorCode = ByteUtilities.GetBytes(ErrorCode);
            result.AddRange(errorCode);

            if (string.IsNullOrWhiteSpace(ReasonPhrase) == false)
            {
                var reasonPhrase = ByteUtilities.GetBytes(ReasonPhrase);
                byte[] rpl = new VariableInteger((ulong)ReasonPhrase.Length);
                result.AddRange(rpl);
                result.AddRange(reasonPhrase);
            }

            return result.ToArray();
        }
    }
}
