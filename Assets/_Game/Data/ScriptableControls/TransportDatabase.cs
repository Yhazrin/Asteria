using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Transport configuration database for the game.
    /// Contains all transport parameters.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Transport Database")]
    public sealed class TransportDatabase : ScriptableObject
    {
        [Header("Transport")]
        public string transportType = "unity-transport";
        public int maxPayloadSize = 1024;
        public int maxConnections = 4;

        [Header("Channels")]
        public int reliableChannel = 0;
        public int unreliableChannel = 1;
        public int stateChannel = 2;

        [Header("Timing")]
        public int sendRate = 20;
        public int tickRate = 20;
        public float disconnectTimeout = 30f;
    }
}
