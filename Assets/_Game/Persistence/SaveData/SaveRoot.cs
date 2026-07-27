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

        // Home planet state (placeholder for Milestone C+)
        public HomePlanetStateDTO homePlanet = new();
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
}
