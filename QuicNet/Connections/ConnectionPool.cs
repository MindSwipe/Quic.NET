using System.Collections.Generic;
using QuicNet.Infrastructure;
using QuicNet.Infrastructure.Settings;
using QuicNet.InternalInfrastructure;

namespace QuicNet.Connections
{
    /// <summary>
    ///     Since UDP is a stateless protocol, the ConnectionPool is used as a Conenction Manager to
    ///     route packets to the right "Connection".
    /// </summary>
    internal static class ConnectionPool
    {
        /// <summary>
        ///     Starting point for connection identifiers.
        ///     ConnectionId's are incremented sequentially by 1.
        /// </summary>
        private static readonly NumberSpace Ns = new NumberSpace(QuicSettings.MaximumConnectionIds);

        private static readonly Dictionary<uint, QuicConnection> Pool = new Dictionary<uint, QuicConnection>();

        /// <summary>
        ///     Adds a connection to the connection pool.
        ///     For now assume that the client connection id is valid, and just send it back.
        ///     Later this should change in a way that the server validates, and regenerates a connection Id.
        /// </summary>
        /// <param name="connection">ConnectionData object to read values from</param>
        /// <param name="availableConnectionId">The ConnectionId available</param>
        /// <returns></returns>
        public static bool AddConnection(ConnectionData connection, out uint availableConnectionId)
        {
            availableConnectionId = 0;

            if (Pool.ContainsKey(connection.ConnectionId))
                return false;
            if (Pool.Count > QuicSettings.MaximumConnectionIds)
                return false;

            availableConnectionId = Ns.Get();
            connection.PeerConnectionId = connection.ConnectionId;
            Pool.Add(availableConnectionId, new QuicConnection(connection));

            return true;
        }

        public static void RemoveConnection(uint id)
        {
            if (Pool.ContainsKey(id))
                Pool.Remove(id);
        }

        public static QuicConnection Find(uint id)
        {
            if (Pool.ContainsKey(id) == false)
                return null;

            return Pool[id];
        }
    }
}