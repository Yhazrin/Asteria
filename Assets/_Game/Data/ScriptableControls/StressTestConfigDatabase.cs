using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Stress test configuration database for the game.
    /// Contains all stress test parameters.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Stress Test Config Database")]
    public sealed class StressTestConfigDatabase : ScriptableObject
    {
        [Header("Stress Test")]
        public bool enableStressTest = false;
        public float stressTestDuration = 300f;

        [Header("Load")]
        public int maxEntities = 100;
        public int maxParticles = 1000;
        public int maxAudioSources = 32;

        [Header("Network")]
        public int maxConnections = 4;
        public float messageRate = 100f;
        public bool simulateLatency = false;
        public float simulatedLatency = 100f;
    }
}
