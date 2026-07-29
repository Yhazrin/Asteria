using System.Collections.Generic;
using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Database of POI definitions for the game.
    /// Contains 8-10 POIs for the wind grassland as required.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/POI Database")]
    public sealed class PoiDatabase : ScriptableObject
    {
        [Header("POIs")]
        public PoiData[] pois = new PoiData[]
        {
            new PoiData
            {
                poiId = "poi_wind_bell_01",
                displayName = "风铃石·东",
                description = "一块会发声的石头，靠近时能听见轻柔的金属颤音。",
                poiType = "Observe",
                biome = "wind_grassland",
                localDirection = new Vector3(1, 0, 0),
                contentTags = new[] { "observe", "sound", "wind" },
                linkedEventId = "silent_bell",
                linkedObserveEntryId = "wind_bell_01"
            },
            new PoiData
            {
                poiId = "poi_wind_bell_02",
                displayName = "风铃石·西",
                description = "另一块风铃石，声音略有不同。",
                poiType = "Observe",
                biome = "wind_grassland",
                localDirection = new Vector3(-1, 0, 0),
                contentTags = new[] { "observe", "sound", "wind" },
                linkedEventId = "silent_bell",
                linkedObserveEntryId = "wind_bell_02"
            },
            new PoiData
            {
                poiId = "poi_wind_tower",
                displayName = "风塔",
                description = "一座损坏的风塔，叶片散落各处。",
                poiType = "Restore",
                biome = "wind_grassland",
                localDirection = new Vector3(0, 0, 1),
                contentTags = new[] { "restore", "wind", "structure" },
                linkedEventId = "tower_blades",
                linkedObserveEntryId = "wind_tower"
            },
            new PoiData
            {
                poiId = "poi_bipolar_gate",
                displayName = "双极共鸣门",
                description = "需要两人分别站在星球两侧才能激活的装置。",
                poiType = "Cooperate",
                biome = "wind_grassland",
                localDirection = new Vector3(0, 1, 0),
                contentTags = new[] { "cooperate", "bipolar", "gate" },
                linkedEventId = "bipolar_resonance",
                linkedObserveEntryId = "bipolar_gate"
            },
            new PoiData
            {
                poiId = "poi_shelter_cave",
                displayName = "避风洞穴",
                description = "一个可以躲避强风的安全洞穴。",
                poiType = "Shelter",
                biome = "wind_grassland",
                localDirection = new Vector3(1, 0, 1).normalized,
                contentTags = new[] { "shelter", "wind", "safe" },
                linkedEventId = "global_wind",
                linkedObserveEntryId = "shelter_cave"
            },
            new PoiData
            {
                poiId = "poi_vista_peak",
                displayName = "观景峰",
                description = "可以看到整个星球的高点。",
                poiType = "Vista",
                biome = "wind_grassland",
                localDirection = new Vector3(0, 1, 0.5f).normalized,
                contentTags = new[] { "vista", "view", "high" },
                linkedEventId = "wind_beast_migration",
                linkedObserveEntryId = "vista_peak"
            },
            new PoiData
            {
                poiId = "poi_lost_traveler",
                displayName = "迷路旅人",
                description = "一个迷路的小旅人，需要帮助找到回家的路。",
                poiType = "Social",
                biome = "wind_grassland",
                localDirection = new Vector3(-1, 0, -1).normalized,
                contentTags = new[] { "social", "rescue", "traveler" },
                linkedEventId = "lost_traveler",
                linkedObserveEntryId = "lost_traveler"
            },
            new PoiData
            {
                poiId = "poi_seed_nest",
                displayName = "种子巢穴",
                description = "风兽的巢穴，里面有珍贵的种子。",
                poiType = "Choice",
                biome = "wind_grassland",
                localDirection = new Vector3(0, -1, 0),
                contentTags = new[] { "choice", "seed", "nest" },
                linkedEventId = "seed_or_nest",
                linkedObserveEntryId = "seed_nest"
            },
        };
    }

    [System.Serializable]
    public class PoiData
    {
        public string poiId;
        public string displayName;
        [TextArea(1, 3)] public string description;
        public string poiType;
        public string biome;
        public Vector3 localDirection;
        public string[] contentTags;
        public string linkedEventId;
        public string linkedObserveEntryId;
    }
}
