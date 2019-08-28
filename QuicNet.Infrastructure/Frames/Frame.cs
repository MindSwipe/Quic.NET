﻿using QuickNet.Utilities;

namespace QuicNet.Infrastructure.Frames
{
    /// <summary>
    /// Data encapsulation unit for a Packet.
    /// </summary>
    public abstract class Frame
    {
        public abstract byte Type { get; }
        public abstract byte[] Encode();
        public abstract void Decode(ByteArray array);
    }
}
