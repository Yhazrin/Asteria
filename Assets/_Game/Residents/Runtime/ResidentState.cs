using System;
using System.Collections.Generic;

namespace Asteria.Residents
{
    /// <summary>
    /// Runtime state of a resident. Serializable for save/load.
    /// </summary>
    [Serializable]
    public class ResidentState
    {
        public string residentId;
        public float familiarity;  // 熟悉度 (general social energy)
        public float affinity;     // 亲近感 (toward community)
        public float trust;        // 信任
        public float tension;      // 紧张

        // Current schedule
        public string currentActivity;
        public string currentDestination;
        public float activityStartTime;

        // Memory
        public List<MemoryRecord> memories = new();

        // Needs (background, not displayed as bars)
        public float safety = 0.7f;
        public float social = 0.5f;
        public float solitude = 0.3f;
        public float expression = 0.5f;
        public float exploration = 0.4f;
    }

    [Serializable]
    public class MemoryRecord
    {
        public string eventId;
        public string timestamp;
        public string[] participants;
        public string location;
        public string emotionalTone; // happy, tense, funny, melancholy
        public string[] tags;
        public float importance; // 0-1, affects decay
        public bool isPermanent;
    }
}
