<p align="center">
    <img src="https://i.imgur.com/r3nH7de.png"></img>
</p>
<p align="center">
    0x5C2AAD80
</p>
<p align="center">
    <a href="https://travis-ci.org/Vect0rZ/Quic.NET">
        <img src="https://travis-ci.org/Vect0rZ/Quic.NET.svg?branch=master" alt="Build Status">
    </a>
    <a href="https://semver.org/">
        <img src="https://img.shields.io/badge/semver-2.0.0-blue.svg">
    </a>
    <img src="https://img.shields.io/badge/version-0.2.0 alpha-green.svg">
</p>
<h1 align="center"> QuicNet </h1>

# Preamble
This is a fork, not the original. The purpose of this fork was to port the Libraries from .NET Framework 4.5.2 (and multiple other versions) to .NET Standard 2.0 to allow interoperability with .NET Framework and .NET Core. The Unit Test project and the Console test projects are still .NET Framework, although they are .NET Framework 4.7.2 now. I also updated the .csproj files from their old VS 2015 format to the new VS 2017 and upwards format. I may or may not continue developing this, I just got it into a state where I can work with it and can start experimenting to see if it fits my needs or not.

# Table of contents
   - [What is QuicNet](#what-is-quicnet)
   - [Get started](#get-started)
      * [Preview](#preview)
      * [Server](#server)
      * [Client](#client)
   - [What is QUIC](#what-is-quic)
      * [Connections](#connections)
      * [Streams](#streams)
      * [Packet](#packet)
      * [Frame](#frame)
   - [Contributing](#contributing)
   - [More](#more)

# What is QuicNet?

QuicNet is a .NET implementation of the QUIC protocol mentioned below.
The implementation stays in line with the 17th version of the [quic-transport](https://datatracker.ietf.org/doc/draft-ietf-quic-transport/?include_text=1) draft,
and does NOT YET offer implementation of the following related drafts:

* [quic-tls](https://datatracker.ietf.org/doc/draft-ietf-quic-tls/?include_text=1)
* [quic-recovery](https://datatracker.ietf.org/doc/draft-ietf-quic-recovery/?include_text=1)

# Get started
Minimal working examples

## Preview

![Alt](https://media.giphy.com/media/9PgwY6Wy8HtjtxoMAt/giphy.gif)

## Server
```csharp
using System;
using QuicNet;
using QuicNet.Context;

namespace QuickNet.Tests.ConsoleServer
{
    class Program
    {
        static void Main(string[] args)
        {
            QuicListener listener = new QuicListener(11000);
            listener.Start();
            while (true)
            {
                // Blocks while waiting for a connection
                QuicConnection client = listener.AcceptQuicClient();

                // Assign an action when a data is received from that client.
                client.OnDataReceived += (c) => {
                    byte[] data = c.Data;
                    Console.WriteLine("Data received: " + Encoding.UTF8.GetString(data));
                    // Echo back data to the client
                    c.Send(Encoding.UTF8.GetBytes("Echo!"));
                };
            }
        }
    }
}
```

## Client
```csharp
using QuickNet;

namespace QuicNet.Tests.ConsoleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            QuicClient client = new QuicClient();
            QuicConnection connection = client.Connect("127.0.0.1", 11000);   // Connect to peer (Server)
            
            // Create a Bidirectional data stream
            QuicStream stream = connection.CreateStream(QuickNet.Utilities.StreamType.ClientBidirectional);
            
            // Send Data
            stream.Send(Encoding.UTF8.GetBytes("Hello from Client!"));        
            
            // Receive from server (Blocks)
            byte[] data = stream.Receive();                                   
            Console.ReadKey();
        }
    }
}

```

# What is QUIC?

QUIC is an experimental transport layer protocol designed by Google, aiming to speed up the data transfer of connection-oriented web applications.
This application-level protocol aims to switch from TCP to UDP by using several techniques to resemble the TCP transfer while reducing the connection handshakes,
as well as to provide sensible multiplexing techniques in a way that different data entities can be interleaved during transfer.

## Connections
Connections are the first tier logical channels representing a communication between two endpoints. When a connection is established, a ConnectionId is negotiated between the two endpoints. The ConnectionId is used for identifying connection even if changes occur on the lower protocol layers, such as a Phone changing Wi-Fi or switching from Wi-Fi to Mobile data. This mechanism prevents restarting the negotiation flow and resending data.

## Streams
Streams are second tier logical channels representing streams of data. A single connection can have a negotiated number of streams (8 maximum for example) which serve as multiplexing entities. Every stream has it's own, generated StreamId, used for identifiying the different data objects being transferred. Streams are closed when all of the data is read, or the negotiated maximum data transfer is reached.

## Packet
Packets are the data transfer units. The packet header contains information about the connection that this packet is being sent to, and cryptographic information. After stipping off the additional transfer information, what is left are the Frames of data (A packet can have multiple frames).

## Frame
Frames are the smallest unit that contain either data that needs to be trasferred to the Endpoint or protocol packets necessary for actions such as handshake negotiation, error handling and other.

# Contributing

Following the Fork and Pull GitHub workflow:

  1. **Fork** the repo on GitHub;
  2. **Clone** the project locally;
  3. **Commit** changes;
  4. **Push** your work back up to your fork;
  5. Submit a **Pull request** so that the changes go through a review.

For more info, read the [CONTRIBUTING](https://github.com/Vect0rZ/Quic.NET/blob/master/CONTRIBUTING.md)

# More

The quic-transport draft can be found, as previously mentioned at [quic-transport](https://datatracker.ietf.org/doc/draft-ietf-quic-transport/?include_text=1).

To test QUIC and find additional information, you can visit [Playing with QUIC](https://www.chromium.org/quic/playing-with-quic).

The official C++ source code can be found at [proto-quic](https://github.com/google/proto-quic).
