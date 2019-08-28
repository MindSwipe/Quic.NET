using System;

namespace QuicNet.Infrastructure.Exceptions
{
    public class ProtocolException : Exception
    {
        public ProtocolException(string message) : base(message)
        {
        }
    }
}
