using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Network configuration database for the game.
    /// Contains all network parameters.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Network Database")]
    public sealed class NetworkDatabase : ScriptableObject
    {
        [Header("Connection")]
        public string serverAddress = "localhost";
        public int serverPort = 7777;
        public int maxPlayers = 4;
        public float connectionTimeout = 10f;

        [Header("Sync")]
        public int tickRate = 20;
        public float interpolationDelay = 0.1f;
        public float positionThreshold = 0.01f;
        public float rotationThreshold = 1f;

        [Header("Reconnection")]
        public float reconnectTimeout = 30f;
        public int maxReconnectAttempts = 3;
        public float reconnectInterval = 5f;

        [Header("Snapshot")]
        public float snapshotInterval = 5f;
        public int maxSnapshots = 10;
    }
}
