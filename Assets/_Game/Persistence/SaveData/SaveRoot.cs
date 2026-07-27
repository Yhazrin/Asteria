using System;
using System.Collections.Generic;

namespace Asteria.Persistence
{
    /// <summary>
    /// Root data structure for game saves. Pure C# DTO — no Unity Object references.
    /// </summary>
    [Serializable]
    public class SaveRoot
    {
        public int schemaVersion = 1;
        public string saveTimestamp;
        public string gameVersion;

        // Player profile
        public string profileId = "default";
        public string playerName = "Explorer";

        // Discovery records
        public List<DiscoveryRecordDTO> discoveries = new();

        // Home planet state
        public HomePlanetStateDTO homePlanet = new();

        // Resident states
        public List<ResidentStateDTO> residents = new();

        // Expedition history
        public List<ExpeditionResultDTO> expeditionHistory = new();

        // Active wishes
        public List<WishStateDTO> activeWishes = new();
    }

    [Serializable]
    public class DiscoveryRecordDTO
    {
        public string id;
        public string displayName;
        public string timestamp;
        public bool isDisplayed;
        public string displayAnchorId;
    }

    [Serializable]
    public class HomePlanetStateDTO
    {
        public int planetSeed;
        public int worldDay;
        public List<BuildAnchorDTO> buildAnchors = new();
    }

    [Serializable]
    public class BuildAnchorDTO
    {
        public string anchorId;
        public string size; // "Large", "Medium", "Small"
        public float dirX, dirY, dirZ; // direction from center
        public string installedFacilityId;
        public float rotationAngle;
    }

    [Serializable]
    public class ResidentStateDTO
    {
        public string residentId;
        public float familiarity;
        public float affinity;
        public float trust;
        public float tension;
        public string currentActivity;
        public List<MemoryRecordDTO> memories = new();
    }

    [Serializable]
    public class MemoryRecordDTO
    {
        public string eventId;
        public string timestamp;
        public string[] participants;
        public string location;
        public string emotionalTone;
        public string[] tags;
        public float importance;
        public bool isPermanent;
    }

    [Serializable]
    public class ExpeditionResultDTO
    {
        public string expeditionId;
        public float durationSeconds;
        public List<string> discoveredIds = new();
        public string outcomeType;
    }

    [Serializable]
    public class WishStateDTO
    {
        public string wishId;
        public string residentId;
        public string status; // "active", "fulfilled", "expired"
        public string fulfilledByExpeditionId;
    }
}
