using System;

namespace QuicNet.Exceptions
{
    public class QuicConnectivityException : Exception
    {
        public QuicConnectivityException(string message) : base(message)
        {
        }
    }
}