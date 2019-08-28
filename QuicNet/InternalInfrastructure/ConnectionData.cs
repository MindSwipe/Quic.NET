namespace QuicNet.InternalInfrastructure
{
    internal class ConnectionData
    {
        public ConnectionData(PacketWireTransfer pwt, uint connectionId, uint peerConnnectionId)
        {
            Pwt = pwt;
            ConnectionId = connectionId;
            PeerConnectionId = peerConnnectionId;
        }

        public PacketWireTransfer Pwt { get; set; }
        public uint ConnectionId { get; set; }
        public uint PeerConnectionId { get; set; }
    }
}