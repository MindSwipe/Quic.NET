using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuickNet.Utilities;

namespace QuicNet.Tests.Unit
{
    [TestClass]
    public class VariableIntegerTests
    {
        [TestMethod]
        public void Zero()
        {
            var integer = new VariableInteger(0);
            byte[] bin = integer;
            ulong num = integer;

            Assert.IsNotNull(bin);
            Assert.AreEqual(bin.Length, 1);
            Assert.AreEqual(bin[0], (byte) 0);
            Assert.AreEqual(num, (ulong) 0);
        }

        [TestMethod]
        public void One()
        {
            var integer = new VariableInteger(1);
            byte[] bin = integer;
            ulong num = integer;

            Assert.IsNotNull(bin);
            Assert.AreEqual(bin.Length, 1);
            Assert.AreEqual(bin[0], (byte) 1);
            Assert.AreEqual(num, (ulong) 1);
        }

        [TestMethod]
        public void Test63()
        {
            var integer = new VariableInteger(63);
            byte[] bin = integer;
            ulong num = integer;

            Assert.IsNotNull(bin);
            Assert.AreEqual(bin.Length, 1);
            Assert.AreEqual(bin[0], (byte) 63);
            Assert.AreEqual(num, (ulong) 63);
        }

        [TestMethod]
        public void Test64()
        {
            var integer = new VariableInteger(64);
            byte[] bin = integer;
            ulong num = integer;

            Assert.IsNotNull(bin);
            Assert.AreEqual(bin.Length, 2);
            Assert.AreEqual(bin[0], (byte) 64);
            Assert.AreEqual(bin[1], (byte) 64);
            Assert.AreEqual(num, (ulong) 64);
        }

        [TestMethod]
        public void Test256()
        {
            var integer = new VariableInteger(256);
            byte[] bin = integer;
            ulong num = integer;

            Assert.IsNotNull(bin);
            Assert.AreEqual(bin.Length, 2);
            Assert.AreEqual(bin[0], (byte) 65);
            Assert.AreEqual(bin[1], (byte) 0);
            Assert.AreEqual(num, (ulong) 256);
        }

        [TestMethod]
        public void TestVariableIntegerMaxValue()
        {
            var integer = new VariableInteger(VariableInteger.MaxValue);
            byte[] bin = integer;
            ulong num = integer;

            Assert.IsNotNull(bin);
            Assert.AreEqual(bin.Length, 8);
            Assert.AreEqual(num, VariableInteger.MaxValue);
        }

        [TestMethod]
        public void TestUInt64MaxValue()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new VariableInteger(ulong.MaxValue));
        }
    }
}