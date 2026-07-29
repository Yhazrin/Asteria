using System.Collections.Generic;
using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Database of expedition definitions for the game.
    /// Contains the wind grassland expedition and future expansions.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Expedition Database")]
    public sealed class ExpeditionDatabase : ScriptableObject
    {
        [Header("Expeditions")]
        public ExpeditionData[] expeditions = new ExpeditionData[]
        {
            new ExpeditionData
            {
                expeditionId = "exp_wind_grassland",
                displayName = "风之草原远征",
                description = "前往风之草原，探索开阔的坡地、风带和峡谷。",
                planetArchetypeId = "wind_grassland",
                sceneName = "SphereMoveDemo",
                targetDurationMinutes = 25f,
                minPlayers = 1,
                maxPlayers = 4,
                availableBiomes = new[] { "Wind", "Grass", "Rock" },
                requiredTools = new[] { "resonance_mirror", "warm_light" },
                rewards = new[] { "wind_bell_seed", "wind_tower_blueprint" },
                events = new[]
                {
                    "wind_direction_test",
                    "silent_bell",
                    "wind_beast_migration",
                    "lost_traveler",
                    "tower_blades",
                    "global_wind",
                    "bipolar_resonance",
                    "seed_or_nest"
                }
            },
            new ExpeditionData
            {
                expeditionId = "exp_mist_forest",
                displayName = "雾声森林远征",
                description = "前往雾声森林，在密林中寻找隐藏的路径和声音。",
                planetArchetypeId = "mist_forest",
                sceneName = "SphereMoveDemo",
                targetDurationMinutes = 30f,
                minPlayers = 1,
                maxPlayers = 4,
                availableBiomes = new[] { "Mist", "Forest", "Swamp" },
                requiredTools = new[] { "resonance_mirror", "beacon" },
                rewards = new[] { "glowing_plant", "sound_archive" },
                events = new[]
                {
                    "mist_navigation",
                    "sound_puzzle",
                    "hidden_path",
                    "forest_creature",
                    "ancient_tree",
                    "swamp_crossing",
                    "echo_chamber",
                    "fog_clearing"
                }
            },
            new ExpeditionData
            {
                expeditionId = "exp_night_valley",
                displayName = "星砂夜谷远征",
                description = "前往星砂夜谷，在黑暗中跟随发光的路径前行。",
                planetArchetypeId = "night_valley",
                sceneName = "SphereMoveDemo",
                targetDurationMinutes = 25f,
                minPlayers = 2,
                maxPlayers = 4,
                availableBiomes = new[] { "Night", "Sand", "Crystal" },
                requiredTools = new[] { "warm_light", "beacon" },
                rewards = new[] { "star_sand_lamp", "night_activity" },
                events = new[]
                {
                    "glowing_path",
                    "star_sand_collection",
                    "night_creature",
                    "crystal_cave",
                    "dawn_arrival",
                    "sand_storm",
                    "constellation_puzzle",
                    "night_shelter"
                }
            },
        };
    }

    [System.Serializable]
    public class ExpeditionData
    {
        public string expeditionId;
        public string displayName;
        [TextArea(2, 4)] public string description;
        public string planetArchetypeId;
        public string sceneName;
        public float targetDurationMinutes;
        public int minPlayers;
        public int maxPlayers;
        public string[] availableBiomes;
        public string[] requiredTools;
        public string[] rewards;
        public string[] events;
    }
}
