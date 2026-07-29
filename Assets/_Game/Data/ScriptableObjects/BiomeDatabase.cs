using System.Collections.Generic;
using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Database of biome definitions for the game.
    /// Contains 6 biomes as required by WORLD_CONTENT_MATRIX.md §3.2.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Biome Database")]
    public sealed class BiomeDatabase : ScriptableObject
    {
        [Header("Biomes")]
        public BiomeData[] biomes = new BiomeData[]
        {
            new BiomeData
            {
                biomeId = "wind_grassland",
                displayName = "风之草原",
                biomeType = "Wind",
                moodTags = new[] { "Curious", "Wondrous", "Cozy" },
                pressureTypes = new[] { "Wind" },
                ambientColor = new Color(0.55f, 0.7f, 0.55f),
                decorationSets = new[] { "grass", "rocks", "flowers" },
                ambientSoundId = "wind_grassland_ambient",
                description = "开阔的坡地，风带和峡谷。适合滑翔和探索。"
            },
            new BiomeData
            {
                biomeId = "mist_forest",
                displayName = "雾声森林",
                biomeType = "Mist",
                moodTags = new[] { "Mysterious", "Quiet", "Tense" },
                pressureTypes = new[] { "Mist", "Spores" },
                ambientColor = new Color(0.3f, 0.5f, 0.3f),
                decorationSets = new[] { "trees", "moss", "mushrooms" },
                ambientSoundId = "mist_forest_ambient",
                description = "密林中的低能见度区域。声音是唯一的导航方式。"
            },
            new BiomeData
            {
                biomeId = "night_valley",
                displayName = "星砂夜谷",
                biomeType = "Night",
                moodTags = new[] { "Wondrous", "Melancholy", "Curious" },
                pressureTypes = new[] { "Dark", "Cold" },
                ambientColor = new Color(0.2f, 0.2f, 0.4f),
                decorationSets = new[] { "sand", "crystals", "glowing_paths" },
                ambientSoundId = "night_valley_ambient",
                description = "暗色沙丘和发光路径。夜晚带来变化。"
            },
            new BiomeData
            {
                biomeId = "ice_tide",
                displayName = "浮冰潮汐星",
                biomeType = "Ice",
                moodTags = new[] { "Wondrous", "Tense", "Cozy" },
                pressureTypes = new[] { "Cold", "Instability" },
                ambientColor = new Color(0.7f, 0.8f, 0.9f),
                decorationSets = new[] { "ice", "hot_springs", "crystals" },
                ambientSoundId = "ice_tide_ambient",
                description = "冰壳、裂隙和热泉。潮汐改变路线。"
            },
            new BiomeData
            {
                biomeId = "bloom_garden",
                displayName = "花粉云庭",
                biomeType = "Bloom",
                moodTags = new[] { "Wondrous", "Cozy", "Funny" },
                pressureTypes = new[] { "Spores" },
                ambientColor = new Color(0.9f, 0.7f, 0.8f),
                decorationSets = new[] { "giant_flowers", "pollen", "vines" },
                ambientSoundId = "bloom_garden_ambient",
                description = "巨花和漂浮孢团。上下风半球差异。"
            },
            new BiomeData
            {
                biomeId = "ruin_mech",
                displayName = "失落机械星",
                biomeType = "Ruin",
                moodTags = new[] { "Tense", "Curious", "Melancholy" },
                pressureTypes = new[] { "Instability" },
                ambientColor = new Color(0.5f, 0.5f, 0.5f),
                decorationSets = new[] { "ruins", "machines", "orbits" },
                ambientSoundId = "ruin_mech_ambient",
                description = "遗迹和轨道装置。需要双人同步修复。"
            },
        };
    }

    [System.Serializable]
    public class BiomeData
    {
        public string biomeId;
        public string displayName;
        public string biomeType;
        public string[] moodTags;
        public string[] pressureTypes;
        public Color ambientColor;
        public string[] decorationSets;
        public string ambientSoundId;
        [TextArea(1, 3)] public string description;
    }
}
