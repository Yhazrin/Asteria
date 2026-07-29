using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Monitoring configuration database for the game.
    /// Contains all monitoring parameters.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Monitoring Config Database")]
    public sealed class MonitoringConfigDatabase : ScriptableObject
    {
        [Header("Monitoring")]
        public bool enableMonitoring = true;
        public float monitoringInterval = 10f;

        [Header("Metrics")]
        public bool monitorFPS = true;
        public bool monitorMemory = true;
        public bool monitorNetwork = true;
        public bool monitorGameplay = true;

        [Header("Alerts")]
        public bool enableAlerts = true;
        public float fpsAlertThreshold = 30f;
        public float memoryAlertThreshold = 0.8f; // 80%
    }
}
