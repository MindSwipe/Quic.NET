using QuickNet.Utilities;
using QuicNet.Infrastructure.Exceptions;
using QuicNet.Infrastructure.Frames;
using QuicNet.Infrastructure.Settings;
using System.Collections.Generic;

namespace QuicNet.Infrastructure.Packets
{
    /// <summary>
    /// Base data transfer unit of QUIC Transport.
    /// </summary>
    public abstract class Packet
    {
        protected List<Frame> Frames = new List<Frame>();
        public abstract byte Type { get; }

        public uint Version { get; set; }

        public abstract byte[] Encode();

        public abstract void Decode(byte[] packet);

        public virtual void AttachFrame(Frame frame)
        {
            Frames.Add(frame);
        }

        public virtual List<Frame> GetFrames()
        {
            return Frames;
        }

        public virtual void DecodeFrames(ByteArray array)
        {
            var factory = new FrameParser(array);
            Frame result;
            var frames = 0;
            while (array.HasData() && frames <= QuicSettings.PMTU)
            {
                result = factory.GetFrame();
                if (result != null)
                    Frames.Add(result);

                frames++;

                // TODO: Possibily handle broken frames.
            }

            if (array.HasData())
                throw new ProtocolException("Unexpected number of frames or possibly corrupted frame was sent.");
        }

        public virtual byte[] EncodeFrames()
        {
            var result = new List<byte>();
            foreach (var frame in Frames)
            {
                result.AddRange(frame.Encode());
            }

            return result.ToArray();
        }
    }
}
