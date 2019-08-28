namespace QuicNet.Infrastructure
{
    public enum PacketType : ushort
    {
        Initial = 0x0,
        ZeroRttProtected = 0x1,
        Handshake = 0x2,
        RetryPacket = 0x3
    }
}
