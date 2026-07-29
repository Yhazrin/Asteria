using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Network configuration file database for the game.
    /// Contains all network parameters.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Network Config File Database")]
    public sealed class NetworkConfigFileDatabase : ScriptableObject
    {
        [Header("Connection")]
        public int maxPlayers = 4;
        public float connectionTimeout = 10f;
        public float reconnectTimeout = 30f;

        [Header("Sync")]
        public int tickRate = 20;
        public float interpolationDelay = 0.1f;
        public float positionThreshold = 0.01f;

        [Header("Snapshot")]
        public float snapshotInterval = 5f;
        public int maxSnapshots = 10;
    }
}
