﻿namespace QuicNet.Infrastructure.Packets
{
    public class Unpacker
    {
        public Packet Unpack(byte[] data)
        {
            Packet result = null;

            var type = GetPacketType(data);
            switch(type)
            {
                case QuicPacketType.Initial: result = new InitialPacket(); break;
                case QuicPacketType.ShortHeader: result = new ShortHeaderPacket(); break;
            }

            if (result == null)
                return null;

            result.Decode(data);

            return result;
        }

        public QuicPacketType GetPacketType(byte[] data)
        {
            if (data == null || data.Length <= 0)
                return QuicPacketType.Broken;

            var type = data[0];

            if ((type & 0xC0) == 0xC0)
                return QuicPacketType.Initial;
            if ((type & 0x40) == 0x40)
                return QuicPacketType.ShortHeader;
            if ((type & 0x80) == 0x80)
                return QuicPacketType.VersionNegotiation;
            if ((type & 0xE0) == 0xE0)
                return QuicPacketType.LongHeader;
            
            return QuicPacketType.Broken;            
        }
    }
}
