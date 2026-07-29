using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Technical parameters database.
    /// Contains all technical values for the game.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Technical Database")]
    public sealed class TechnicalDatabase : ScriptableObject
    {
        [Header("Performance")]
        public int targetFps = 60;
        public float frameBudgetMs = 16.67f;
        public int maxDrawCalls = 500;
        public int maxVertices = 500000;

        [Header("Memory")]
        public int maxTextureSize = 2048;
        public int maxMeshVertices = 65000;
        public float gcThreshold = 0.1f;

        [Header("Physics")]
        public int physicsTickRate = 50;
        public int collisionIterations = 6;
        public float solverTolerance = 0.001f;

        [Header("Networking")]
        public int networkTickRate = 20;
        public int maxPacketSize = 1024;
        public float interpolationDelay = 0.1f;

        [Header("Audio")]
        public int audioSampleRate = 44100;
        public int audioBufferSize = 1024;
        public int maxAudioSources = 32;

        [Header("Rendering")]
        public int shadowResolution = 1024;
        public float shadowDistance = 200f;
        public int msaaLevel = 4;
    }
}
