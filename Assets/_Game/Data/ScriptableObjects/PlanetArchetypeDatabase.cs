using System.Collections.Generic;
using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Database of planet archetypes for expedition generation.
    /// Contains 6 archetypes as required by WORLD_CONTENT_MATRIX.md §3.2.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Planet Archetype Database")]
    public sealed class PlanetArchetypeDatabase : ScriptableObject
    {
        [Header("Archetypes")]
        public ArchetypeData[] archetypes = new ArchetypeData[]
        {
            new ArchetypeData
            {
                archetypeId = "wind_grassland",
                displayName = "风之草原",
                description = "开阔坡地、风带、峡谷。主要压力：强风、失衡。",
                terrainType = "open_hills",
                primaryBiome = "Wind",
                secondaryBiomes = new[] { "Grass", "Rock" },
                mainPressure = "Wind",
                sphericalFeature = "沿纬度风带滑翔",
                homeReward = "风铃种子、风帆设施",
                color = new Color(0.5f, 0.7f, 0.5f)
            },
            new ArchetypeData
            {
                archetypeId = "mist_forest",
                displayName = "雾声森林",
                description = "密林、低能见度。主要压力：迷失、孢子。",
                terrainType = "dense_forest",
                primaryBiome = "Mist",
                secondaryBiomes = new[] { "Forest", "Swamp" },
                mainPressure = "Mist",
                sphericalFeature = "越过地平线听声定位",
                homeReward = "发光植物、声音档案",
                color = new Color(0.3f, 0.5f, 0.3f)
            },
            new ArchetypeData
            {
                archetypeId = "night_valley",
                displayName = "星砂夜谷",
                description = "暗色沙丘、发光路径。主要压力：黑暗、低温。",
                terrainType = "dark_dunes",
                primaryBiome = "Night",
                secondaryBiomes = new[] { "Sand", "Crystal" },
                mainPressure = "Dark",
                sphericalFeature = "昼夜线改变路线",
                homeReward = "星砂灯、夜间活动",
                color = new Color(0.2f, 0.2f, 0.4f)
            },
            new ArchetypeData
            {
                archetypeId = "ice_tide",
                displayName = "浮冰潮汐星",
                description = "冰壳、裂隙、热泉。主要压力：受寒、地表断裂。",
                terrainType = "ice_shell",
                primaryBiome = "Ice",
                secondaryBiomes = new[] { "Snow", "Water" },
                mainPressure = "Cold",
                sphericalFeature = "全球潮汐让路线周期变化",
                homeReward = "温泉设施、冰晶生物",
                color = new Color(0.7f, 0.8f, 0.9f)
            },
            new ArchetypeData
            {
                archetypeId = "bloom_garden",
                displayName = "花粉云庭",
                description = "巨花、漂浮孢团。主要压力：视听失真。",
                terrainType = "giant_flowers",
                primaryBiome = "Bloom",
                secondaryBiomes = new[] { "Forest", "Mist" },
                mainPressure = "Spores",
                sphericalFeature = "上风/下风半球差异",
                homeReward = "香气工坊、园艺角色",
                color = new Color(0.9f, 0.7f, 0.8f)
            },
            new ArchetypeData
            {
                archetypeId = "ruin_mech",
                displayName = "失落机械星",
                description = "遗迹、轨道装置。主要压力：能量不足、机关。",
                terrainType = "ruins",
                primaryBiome = "Ruin",
                secondaryBiomes = new[] { "Rock", "Crystal" },
                mainPressure = "Instability",
                sphericalFeature = "星球两侧同步修复",
                homeReward = "工坊模块、机械居民线索",
                color = new Color(0.5f, 0.5f, 0.5f)
            },
        };
    }

    [System.Serializable]
    public class ArchetypeData
    {
        public string archetypeId;
        public string displayName;
        [TextArea(2, 4)] public string description;
        public string terrainType;
        public string primaryBiome;
        public string[] secondaryBiomes;
        public string mainPressure;
        public string sphericalFeature;
        public string homeReward;
        public Color color;
    }
}
