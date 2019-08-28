using System;
using System.Text;

namespace QuickNet.Utilities
{
    public static class ByteUtilities
    {
        public static byte[] GetBytes(ulong integer)
        {
            var result = BitConverter.GetBytes(integer);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(result);

            return result;
        }

        public static byte[] GetBytes(uint integer)
        {
            var result = BitConverter.GetBytes(integer);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(result);

            return result;
        }

        public static byte[] GetBytes(ushort integer)
        {
            var result = BitConverter.GetBytes(integer);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(result);

            return result;
        }

        public static byte[] GetBytes(string str)
        {
            return Encoding.Unicode.GetBytes(str);
        }

        public static ulong ToUInt64(byte[] data)
        {
            if (BitConverter.IsLittleEndian)
                Array.Reverse(data);

            return BitConverter.ToUInt64(data, 0);
        }

        public static uint ToUInt32(byte[] data)
        {
            if (BitConverter.IsLittleEndian)
                Array.Reverse(data);

            return BitConverter.ToUInt32(data, 0);
        }

        public static ushort ToUInt16(byte[] data)
        {
            if (BitConverter.IsLittleEndian)
                Array.Reverse(data);

            return BitConverter.ToUInt16(data, 0);
        }

        public static string GetString(byte[] str)
        {
            return Encoding.UTF8.GetString(str);
        }
    }
}