using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Debug configuration database.
    /// Contains all debug settings for development.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Debug Database")]
    public sealed class DebugDatabase : ScriptableObject
    {
        [Header("Visual Debug")]
        public bool showFPS = false;
        public bool showColliders = false;
        public bool showNavMesh = false;
        public bool showLOD = false;
        public bool showChunkBounds = false;

        [Header("Gameplay Debug")]
        public bool godMode = false;
        public bool infiniteEnergy = false;
        public bool skipTutorials = false;
        public bool fastTravel = false;
        public bool spawnAllItems = false;

        [Header("System Debug")]
        public bool verboseLogging = false;
        public bool profilePerformance = false;
        public bool dumpSaveData = false;
        public bool testMigration = false;

        [Header("Network Debug")]
        public bool simulateLatency = false;
        public float simulatedLatency = 100f;
        public bool simulatePacketLoss = false;
        public float packetLossRate = 0.05f;
    }
}
