using System;
using System.Text;
using QuicNet;

namespace QuickNet.Tests.ConsoleServer
{
    internal class Program
    {
        private static void Main()
        {
            var listener = new QuicListener(11000);
            listener.Start();

            while (true)
            {
                // Blocks while waiting for a connection
                var client = listener.AcceptQuicClient();

                // Assign an action when a data is received from that client.
                client.OnDataReceived += c =>
                {
                    var data = c.Data;

                    Console.WriteLine("Data received: " + Encoding.Unicode.GetString(data));

                    c.Send(Encoding.UTF8.GetBytes("Echo!"));
                };
            }

            // ReSharper disable once FunctionNeverReturns
            // We need this so the server actually responds
        }
    }
}