using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuickNet.Utilities;

namespace QuicNet.Tests.Unit
{
    [TestClass]
    public class StreamIdTests
    {
        [TestMethod]
        public void ClientBidirectional()
        {
            var id = new StreamId(123, StreamType.ClientBidirectional);
            byte[] data = id;

            Assert.IsNotNull(data);
            Assert.AreEqual(data.Length, 8);
            Assert.AreEqual(data[6], 1);
            Assert.AreEqual(data[7], 236);
        }

        [TestMethod]
        public void ClientUnidirectional()
        {
            var id = new StreamId(123, StreamType.ClientUnidirectional);
            byte[] data = id;

            Assert.IsNotNull(data);
            Assert.AreEqual(data.Length, 8);
            Assert.AreEqual(data[6], 1);
            Assert.AreEqual(data[7], 238);
        }

        [TestMethod]
        public void ServerBidirectional()
        {
            var id = new StreamId(123, StreamType.ServerBidirectional);
            byte[] data = id;

            Assert.IsNotNull(data);
            Assert.AreEqual(data.Length, 8);
            Assert.AreEqual(data[6], 1);
            Assert.AreEqual(data[7], 237);
        }

        [TestMethod]
        public void ServerUnidirectional()
        {
            var id = new StreamId(123, StreamType.ServerUnidirectional);
            byte[] data = id;

            Assert.IsNotNull(data);
            Assert.AreEqual(data.Length, 8);
            Assert.AreEqual(data[6], 1);
            Assert.AreEqual(data[7], 239);
        }
    }
}