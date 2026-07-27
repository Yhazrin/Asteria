using System;
using System.Collections.Generic;

namespace Asteria.Expedition
{
    /// <summary>
    /// Result of a completed expedition. Used for settlement and follow-up events.
    /// </summary>
    [Serializable]
    public class ExpeditionResult
    {
        public string expeditionId;
        public float durationSeconds;
        public List<string> discoveredIds = new();
        public List<string> restoredIds = new();
        public List<string> cooperatedIds = new();
        public int rescueCount;
        public string outcomeType; // "success", "partial", "evacuation"
        public string sharedMemoryPhoto;
    }
}
