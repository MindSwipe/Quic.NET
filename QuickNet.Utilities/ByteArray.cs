using System;

namespace QuickNet.Utilities
{
    public class ByteArray
    {
        private readonly byte[] _array;
        private readonly int _length;
        private int _offset;

        public ByteArray(byte[] array)
        {
            _array = array;

            _length = array.Length;
            _offset = 0;
        }

        public byte ReadByte()
        {
            var result = _array[_offset++];
            return result;
        }

        public byte PeekByte()
        {
            var result = _array[_offset];
            return result;
        }

        public byte[] ReadBytes(int count)
        {
            var bytes = new byte[count];
            Buffer.BlockCopy(_array, _offset, bytes, 0, count);
            _offset += count;

            return bytes;
        }

        public ushort ReadUInt16()
        {
            var bytes = ReadBytes(2);
            return ByteUtilities.ToUInt16(bytes);
        }

        public uint ReadUInt32()
        {
            return ByteUtilities.ToUInt32(ReadBytes(4));
        }

        public VariableInteger ReadVariableInteger()
        {
            // Set Token Length and Token
            var initial = PeekByte();
            var size = VariableInteger.Size(initial);

            var bytes = new byte[size];
            Buffer.BlockCopy(_array, _offset, bytes, 0, size);
            _offset += size;

            return bytes;
        }

        public StreamId ReadStreamId()
        {
            return ReadBytes(8);
        }

        public bool HasData()
        {
            return _offset < _length;
        }
    }
}