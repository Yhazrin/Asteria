using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Build configuration database for different platforms.
    /// Contains platform-specific settings and optimizations.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Build Config Database")]
    public sealed class BuildConfigDatabase : ScriptableObject
    {
        [Header("Platform")]
        public string platformName = "StandaloneWindows64";
        public string targetArchitecture = "x86_64";

        [Header("Rendering")]
        public bool useURP = true;
        public int msaaLevel = 4;
        public bool enableHDR = true;
        public bool enablePostProcessing = true;

        [Header("Performance")]
        public int targetFrameRate = 60;
        public bool enableVSync = true;
        public int qualityLevel = 2;
        public bool enableLOD = true;
        public float lodBias = 1f;

        [Header("Audio")]
        public int audioSampleRate = 44100;
        public int audioBufferSize = 1024;
        public bool enableSpatialAudio = true;

        [Header("Networking")]
        public bool enableMultiplayer = true;
        public int maxPlayers = 4;
        public int networkTickRate = 20;

        [Header("Debug")]
        public bool enableDevelopmentBuild = false;
        public bool enableScriptDebugging = false;
        public bool enableDeepProfiling = false;
    }
}
