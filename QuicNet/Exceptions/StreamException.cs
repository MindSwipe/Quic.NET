using System;

namespace QuicNet.Exceptions
{
    public class StreamException : Exception
    {
        public StreamException(string message) : base(message)
        {
        }
    }
}