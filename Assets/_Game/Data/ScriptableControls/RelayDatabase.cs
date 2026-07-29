using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Relay configuration database for the game.
    /// Contains all relay parameters.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Relay Database")]
    public sealed class RelayDatabase : ScriptableObject
    {
        [Header("Relay")]
        public string relayServer = "https://relay.unity.com";
        public int relayPort = 443;
        public float connectionTimeout = 10f;

        [Header("Region")]
        public string preferredRegion = "us-east";
        public string[] availableRegions = { "us-east", "us-west", "eu-west", "asia-east" };

        [Header("Performance")]
        public int maxConnections = 4;
        public float heartbeatInterval = 5f;
        public float disconnectTimeout = 30f;
    }
}
