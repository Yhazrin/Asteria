using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Performance monitoring database for the game.
    /// Contains all performance metrics and thresholds.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Performance Database")]
    public sealed class PerformanceDatabase : ScriptableObject
    {
        [Header("Frame Rate")]
        public float targetFps = 60f;
        public float minAcceptableFps = 30f;
        public float fpsCheckInterval = 1f;

        [Header("Memory")]
        public long maxMemoryMB = 2048;
        public float memoryCheckInterval = 5f;

        [Header("Draw Calls")]
        public int maxDrawCalls = 500;
        public float drawCallCheckInterval = 2f;

        [Header("LOD")]
        public float lodBias = 1f;
        public int maxLODLevels = 4;
        public float lodTransitionSpeed = 2f;

        [Header("Physics")]
        public int physicsTickRate = 50;
        public int maxPhysicsIterations = 6;
    }
}
