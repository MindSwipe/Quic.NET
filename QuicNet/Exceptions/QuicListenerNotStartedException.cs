using System;

namespace QuicNet.Exceptions
{
    public class QuicListenerNotStartedException : Exception
    {
        public QuicListenerNotStartedException(string message) : base(message)
        {
        }
    }
}