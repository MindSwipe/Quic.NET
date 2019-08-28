namespace QuickNet.Utilities
{
    public enum StreamType
    {
        ClientBidirectional = 0x0,
        ServerBidirectional = 0x1,
        ClientUnidirectional = 0x2,
        ServerUnidirectional = 0x3
    }

    public class StreamId
    {
        public StreamId(ulong id, StreamType type)
        {
            Id = id;
            Type = type;
            IntegerValue = (id << 2) | (ulong) type;
        }

        public ulong Id { get; }
        public ulong IntegerValue { get; }
        public StreamType Type { get; }

        public static implicit operator byte[](StreamId id)
        {
            return Encode(id.Id, id.Type);
        }

        public static implicit operator StreamId(byte[] data)
        {
            return Decode(data);
        }

        public static implicit operator ulong(StreamId streamId)
        {
            return streamId.Id;
        }

        public static byte[] Encode(ulong id, StreamType type)
        {
            var identifier = (id << 2) | (ulong) type;
            return ByteUtilities.GetBytes(identifier);
        }

        public static StreamId Decode(byte[] data)
        {
            var id = ByteUtilities.ToUInt64(data);
            var identifier = id >> 2;
            var type = 0x03 & id;
            var streamType = (StreamType) type;

            return new StreamId(identifier, streamType);
        }
    }
}