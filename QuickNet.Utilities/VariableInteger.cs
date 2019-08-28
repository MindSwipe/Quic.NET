using System;

namespace QuickNet.Utilities
{
    public class VariableInteger
    {
        public const ulong MaxValue = 4611686018427387903;

        public VariableInteger(ulong integer)
        {
            Value = integer;
        }

        public ulong Value { get; }

        public static implicit operator byte[](VariableInteger integer)
        {
            return Encode(integer.Value);
        }

        public static implicit operator VariableInteger(byte[] bytes)
        {
            return new VariableInteger(Decode(bytes));
        }

        public static implicit operator VariableInteger(ulong integer)
        {
            return new VariableInteger(integer);
        }

        public static implicit operator ulong(VariableInteger integer)
        {
            return integer.Value;
        }

        public static int Size(byte firstByte)
        {
            return (int) Math.Pow(2, firstByte >> 6);
        }

        public static byte[] Encode(ulong integer)
        {
            int requiredBytes;
            if (integer <= byte.MaxValue >> 2) /* 63 */
                requiredBytes = 1;
            else if (integer <= ushort.MaxValue >> 2) /* 16383 */
                requiredBytes = 2;
            else if (integer <= uint.MaxValue >> 2) /* 1073741823 */
                requiredBytes = 4;
            else if (integer <= ulong.MaxValue >> 2) /* 4611686018427387903 */
                requiredBytes = 8;
            else
                throw new ArgumentOutOfRangeException(nameof(integer),
                    "Value is larger than VariableInteger.MaxValue.");

            var offset = 8 - requiredBytes;

            var uInt64Bytes = ByteUtilities.GetBytes(integer);
            var first = uInt64Bytes[offset];
            first = (byte) (first | ((requiredBytes / 2) << 6));
            uInt64Bytes[offset] = first;

            var result = new byte[requiredBytes];
            Buffer.BlockCopy(uInt64Bytes, offset, result, 0, requiredBytes);

            return result;
        }

        public static ulong Decode(byte[] bytes)
        {
            var i = 8 - bytes.Length;
            var buffer = new byte[8];

            Buffer.BlockCopy(bytes, 0, buffer, i, bytes.Length);
            buffer[i] = (byte) (buffer[i] & (255 >> 2));

            return ByteUtilities.ToUInt64(buffer);
        }
    }
}