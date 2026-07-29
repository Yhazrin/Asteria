using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Event log definitions for debugging and analytics.
    /// Contains all log event types and formats.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Event Log Database")]
    public sealed class EventLogDatabase : ScriptableObject
    {
        [Header("System Events")]
        public string logSystemStart = "System started";
        public string logSystemStop = "System stopped";
        public string logSaveComplete = "Save completed";
        public string logLoadComplete = "Load completed";

        [Header("Gameplay Events")]
        public string logDiscovery = "Discovery: {0}";
        public string logExpeditionStart = "Expedition started: {0}";
        public string logExpeditionEnd = "Expedition ended: {0}";
        public string logReturnHome = "Returned home";

        [Header("Social Events")]
        public string logInteraction = "Interaction: {0} and {1}";
        public string logSocialEvent = "Social event: {0}";
        public string logWishExpressed = "Wish expressed: {0}";
        public string logWishFulfilled = "Wish fulfilled: {0}";

        [Header("Building Events")]
        public string logFacilityBuilt = "Facility built: {0}";
        public string logFacilityDemolished = "Facility demolished: {0}";

        [Header("Error Events")]
        public string logError = "Error: {0}";
        public string logWarning = "Warning: {0}";
        public string logException = "Exception: {0}";
    }
}
