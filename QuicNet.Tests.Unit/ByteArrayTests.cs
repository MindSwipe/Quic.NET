using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuickNet.Utilities;

namespace QuicNet.Tests.Unit
{
    [TestClass]
    public class ByteArrayTests
    {
        [TestMethod]
        public void SingleByte()
        {
            byte[] data = {1, 1, 2, 3, 5, 8, 13, 21, 34};
            var array = new ByteArray(data);

            var peek = array.PeekByte();
            var result = array.ReadByte();

            Assert.AreEqual(peek, (byte) 1);
            Assert.AreEqual(result, (byte) 1);
        }

        [TestMethod]
        public void SingleConsecutiveBytes()
        {
            byte[] data = {1, 1, 2, 3, 5, 8, 13, 21, 34};
            var array = new ByteArray(data);

            var r1 = array.ReadByte();
            var r2 = array.ReadByte();
            var r3 = array.ReadByte();

            Assert.AreEqual(r1, (byte) 1);
            Assert.AreEqual(r2, (byte) 1);
            Assert.AreEqual(r3, (byte) 2);
        }

        [TestMethod]
        public void MultipleBytes()
        {
            byte[] data = {1, 1, 2, 3, 5, 8, 13, 21, 34};
            var array = new ByteArray(data);

            var result = array.ReadBytes(6);

            Assert.AreEqual(result.Length, 6);
            CollectionAssert.AreEquivalent(result, new byte[] {1, 1, 2, 3, 5, 8});
        }

        [TestMethod]
        public void ReadShort()
        {
            byte[] data = {1, 1, 2, 3, 5, 8, 13, 21, 34};
            var array = new ByteArray(data);

            var result = array.ReadUInt16();
            Assert.AreEqual(result, (ushort) 257);
        }

        [TestMethod]
        public void ReadInteger()
        {
            byte[] data = {1, 1, 2, 3, 5, 8, 13, 21, 34};
            var array = new ByteArray(data);

            var result = array.ReadUInt32();
            Assert.AreEqual(result, (uint) 16843267);
        }

        [TestMethod]
        public void ReadTooMany()
        {
            byte[] data = {1, 1, 2, 3, 5, 8, 13, 21, 34};
            var array = new ByteArray(data);

            Assert.ThrowsException<ArgumentException>(() => { array.ReadBytes(10); });
        }
    }
}