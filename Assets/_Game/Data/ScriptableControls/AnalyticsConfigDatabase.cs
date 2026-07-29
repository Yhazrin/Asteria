using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Analytics configuration database for the game.
    /// Contains all analytics parameters.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Analytics Config Database")]
    public sealed class AnalyticsConfigDatabase : ScriptableObject
    {
        [Header("Analytics")]
        public bool enableAnalytics = false;
        public string analyticsEndpoint = "";
        public float sendInterval = 60f;

        [Header("Events")]
        public bool trackSessionEvents = true;
        public bool trackGameplayEvents = true;
        public bool trackPerformanceEvents = true;
        public bool trackErrorEvents = true;

        [Header("Privacy")]
        public bool anonymizeData = true;
        public bool requireConsent = true;
    }
}
