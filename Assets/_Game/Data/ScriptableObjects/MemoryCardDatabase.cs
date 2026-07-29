using System.Collections.Generic;
using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Database of shared memory cards for expeditions.
    /// Contains 4+ memory cards as required by CORE_GAMEPLAY_AND_SYSTEMS.md §8.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Memory Card Database")]
    public sealed class MemoryCardDatabase : ScriptableObject
    {
        [Header("Memory Cards")]
        public MemoryCardData[] cards = new MemoryCardData[]
        {
            new MemoryCardData
            {
                cardId = "mem_wind_bell_discovery",
                title = "风铃石的颤音",
                description = "我们在风之草原发现了一块会发声的石头。靠近时能听见很轻的金属颤音。",
                expeditionId = "exp_wind_grassland",
                planetName = "风之草原",
                weatherCondition = "晴朗",
                keyDiscovery = "发现风铃石",
                whoHelpedWhom = "大家一起发现",
                chosenEnding = "带回种子",
                triggeredHomeEvents = new[] { "wind_bell_installation" },
                affectedRelationships = new[] { "all_affinity +0.05" }
            },
            new MemoryCardData
            {
                cardId = "mem_storm_rescue",
                title = "风暴中的救援",
                description = "全球强风来袭时，有人被困在峡谷里。我们用信标和牵引绳把他救了出来。",
                expeditionId = "exp_wind_grassland",
                planetName = "风之草原",
                weatherCondition = "暴风",
                keyDiscovery = "成功救援",
                whoHelpedWhom = "凯救了莲",
                chosenEnding = "全员安全撤离",
                triggeredHomeEvents = new[] { "rescue_discussion" },
                affectedRelationships = new[] { "kai_lian_trust +0.2" }
            },
            new MemoryCardData
            {
                cardId = "mem_bipolar_aurora",
                title = "双极极光",
                description = "我们在星球两侧同时激活了共鸣装置，天空出现了壮丽的极光。",
                expeditionId = "exp_wind_grassland",
                planetName = "风之草原",
                weatherCondition = "夜晚",
                keyDiscovery = "双极共鸣完成",
                whoHelpedWhom = "双方同步完成",
                chosenEnding = "家园出现极光",
                triggeredHomeEvents = new[] { "aurora_night" },
                affectedRelationships = new[] { "all_affinity +0.1" }
            },
            new MemoryCardData
            {
                cardId = "mem_seed_choice",
                title = "种子的去向",
                description = "我们选择把风铃石的种子带回家，种在了温室里。",
                expeditionId = "exp_wind_grassland",
                planetName = "风之草原",
                weatherCondition = "晴朗",
                keyDiscovery = "带回种子",
                whoHelpedWhom = "大家一起决定",
                chosenEnding = "种在温室",
                triggeredHomeEvents = new[] { "greenhouse_planting" },
                affectedRelationships = new[] { "all_affinity +0.05" }
            },
            new MemoryCardData
            {
                cardId = "mem_mist_path",
                title = "雾中引路",
                description = "在雾声森林中，一只引路生物带我们找到了隐藏的小径。",
                expeditionId = "exp_mist_forest",
                planetName = "雾声森林",
                weatherCondition = "浓雾",
                keyDiscovery = "发现隐藏路径",
                whoHelpedWhom = "引路生物帮忙",
                chosenEnding = "找到古老树木",
                triggeredHomeEvents = new[] { "forest_story" },
                affectedRelationships = new[] { "all_curiosity +0.1" }
            },
        };
    }

    [System.Serializable]
    public class MemoryCardData
    {
        public string cardId;
        public string title;
        [TextArea(2, 4)] public string description;
        public string expeditionId;
        public string planetName;
        public string weatherCondition;
        public string keyDiscovery;
        public string whoHelpedWhom;
        public string chosenEnding;
        public string[] triggeredHomeEvents;
        public string[] affectedRelationships;
    }
}
