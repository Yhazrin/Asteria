using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Telemetry configuration database for the game.
    /// Contains all telemetry parameters.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Telemetry Config Database")]
    public sealed class TelemetryConfigDatabase : ScriptableObject
    {
        [Header("Telemetry")]
        public bool enableTelemetry = false;
        public string telemetryEndpoint = "";
        public float sendInterval = 30f;

        [Header("Metrics")]
        public bool trackFPS = true;
        public bool trackMemory = true;
        public bool trackNetwork = true;
        public bool trackGameplay = true;

        [Header("Sampling")]
        public float samplingRate = 0.1f;
        public int maxEventsPerBatch = 100;
    }
}
