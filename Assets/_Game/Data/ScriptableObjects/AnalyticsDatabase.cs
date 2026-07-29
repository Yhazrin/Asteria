using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Analytics event definitions for the game.
    /// Tracks player behavior and game metrics.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Analytics Database")]
    public sealed class AnalyticsDatabase : ScriptableObject
    {
        [Header("Session Events")]
        public string evtSessionStart = "session_start";
        public string evtSessionEnd = "session_end";
        public string evtPlayStart = "play_start";
        public string evtPlayEnd = "play_end";

        [Header("Discovery Events")]
        public string evtDiscovery = "discovery";
        public string evtExpeditionStart = "expedition_start";
        public string evtExpeditionEnd = "expedition_end";
        public string evtReturnHome = "return_home";

        [Header("Social Events")]
        public string evtResidentInteraction = "resident_interaction";
        public string evtSocialEvent = "social_event";
        public string evtWishExpressed = "wish_expressed";
        public string evtWishFulfilled = "wish_fulfilled";

        [Header("Building Events")]
        public string evtFacilityBuilt = "facility_built";
        public string evtFacilityDemolished = "facility_demolished";

        [Header("Tool Events")]
        public string evtToolUsed = "tool_used";
        public string evtToolUpgraded = "tool_upgraded";

        [Header("System Events")]
        public string evtSaveCreated = "save_created";
        public string evtSaveLoaded = "save_loaded";
        public string evtErrorOccurred = "error_occurred";
    }
}
