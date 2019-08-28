namespace QuicNet.Infrastructure
{
    public enum QuicPacketType
    {
        Initial,
        LongHeader,
        ShortHeader,
        VersionNegotiation,
        Broken
    }
}
