using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Diagnostics configuration database for the game.
    /// Contains all diagnostics parameters.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Diagnostics Config Database")]
    public sealed class DiagnosticsConfigDatabase : ScriptableObject
    {
        [Header("Diagnostics")]
        public bool enableDiagnostics = true;
        public float diagnosticsInterval = 60f;

        [Header("Profiling")]
        public bool enableProfiling = false;
        public bool profileCPU = true;
        public bool profileGPU = true;
        public bool profileMemory = true;

        [Header("Debug")]
        public bool enableDebugOverlay = false;
        public bool showFPS = false;
        public bool showMemory = false;
        public bool showNetwork = false;
    }
}
