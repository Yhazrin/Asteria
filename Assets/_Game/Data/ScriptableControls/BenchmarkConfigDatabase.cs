using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Benchmark configuration database for the game.
    /// Contains all benchmark parameters.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Benchmark Config Database")]
    public sealed class BenchmarkConfigDatabase : ScriptableObject
    {
        [Header("Benchmark")]
        public bool enableBenchmark = false;
        public float benchmarkDuration = 60f;
        public int benchmarkIterations = 3;

        [Header("Scenes")]
        public string[] benchmarkScenes = { "SphereMoveDemo", "HomePlanet" };

        [Header("Metrics")]
        public bool measureFPS = true;
        public bool measureLoadTime = true;
        public bool measureMemory = true;
        public bool measureDrawCalls = true;
    }
}
