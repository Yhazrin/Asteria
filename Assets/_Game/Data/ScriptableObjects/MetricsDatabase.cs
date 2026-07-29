using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Metrics tracking database.
    /// Contains all metrics definitions for analytics.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Metrics Database")]
    public sealed class MetricsDatabase : ScriptableObject
    {
        [Header("Session Metrics")]
        public string metricSessionDuration = "session_duration";
        public string metricPlayTime = "play_time";
        public string metricIdleTime = "idle_time";

        [Header("Discovery Metrics")]
        public string metricDiscoveryCount = "discovery_count";
        public string metricExpeditionCount = "expedition_count";
        public string metricReturnCount = "return_count";

        [Header("Social Metrics")]
        public string metricInteractionCount = "interaction_count";
        public string metricWishCount = "wish_count";
        public string metricEventCount = "event_count";

        [Header("Building Metrics")]
        public string metricBuildCount = "build_count";
        public string metricDemolishCount = "demolish_count";
        public string metricFacilityCount = "facility_count";

        [Header("Performance Metrics")]
        public string metricAvgFps = "avg_fps";
        public string metricMinFps = "min_fps";
        public string metricMemoryUsage = "memory_usage";
        public string metricLoadTime = "load_time";
    }
}
