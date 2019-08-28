namespace QuicNet.Streams
{
    public enum StreamState
    {
        Recv,
        Recvd,
        SizeKnown,
        DataRecvd,
        DataRead,
        ResetRecvd
    }
}