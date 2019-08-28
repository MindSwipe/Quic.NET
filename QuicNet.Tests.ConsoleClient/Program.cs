using System;
using System.Text;
using QuickNet.Utilities;

namespace QuicNet.Tests.ConsoleClient
{
    internal class Program
    {
        private static void Main()
        {
            Console.WriteLine("Starting client.");
            var client = new QuicClient();
            Console.WriteLine("Connecting to server.");
            var connection = client.Connect("127.0.0.1", 11000); // Connect to peer (Server)
            Console.WriteLine("Connected");

            var stream = connection.CreateStream(StreamType.ClientBidirectional); // Create a data stream
            Console.WriteLine("Create stream with id: " + stream.StreamId.IntegerValue);

            Console.WriteLine("Send 'Hello From Client!'");
            stream.Send(Encoding.Unicode.GetBytes("Hello from Client!")); // Send Data
            Console.WriteLine("Waiting for message from the server");

            try
            {
                var data = stream.Receive(); // Receive from server
                Console.WriteLine("Received: " + Encoding.UTF8.GetString(data));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.ReadLine();
        }
    }
}