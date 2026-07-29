using System.Collections.Generic;
using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Database of pressure definitions for the game.
    /// Contains 5 pressure types as required by CORE_GAMEPLAY_AND_SYSTEMS.md §3.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Pressure Database")]
    public sealed class PressureDatabase : ScriptableObject
    {
        [Header("Pressures")]
        public PressureData[] pressures = new PressureData[]
        {
            new PressureData
            {
                pressureId = "pressure_wind",
                displayName = "强风",
                description = "全球强风影响移动和平衡。需要信标和牵引绳。",
                pressureType = "Wind",
                warningDurationSeconds = 10f,
                activeDurationSeconds = 180f,
                affectedStates = new[] { "unbalanced", "pushed" },
                counterTools = new[] { "beacon", "tether_rope" },
                recoveryMethod = "move_to_shelter",
                recoveryTimeSeconds = 5f,
                visualEffect = "wind_particles",
                audioEffect = "wind_howling"
            },
            new PressureData
            {
                pressureId = "pressure_cold",
                displayName = "低温",
                description = "温度下降影响移动速度。需要暖光灯和篝火。",
                pressureType = "Cold",
                warningDurationSeconds = 15f,
                activeDurationSeconds = 120f,
                affectedStates = new[] { "cold", "slow" },
                counterTools = new[] { "warm_light", "beacon" },
                recoveryMethod = "approach_heat_source",
                recoveryTimeSeconds = 8f,
                visualEffect = "frost_overlay",
                audioEffect = "cold_wind"
            },
            new PressureData
            {
                pressureId = "pressure_dark",
                displayName = "黑暗",
                description = "视野受限，需要光源导航。",
                pressureType = "Dark",
                warningDurationSeconds = 5f,
                activeDurationSeconds = 240f,
                affectedStates = new[] { "blinded", "lost" },
                counterTools = new[] { "warm_light", "beacon" },
                recoveryMethod = "find_light_source",
                recoveryTimeSeconds = 3f,
                visualEffect = "darkness_overlay",
                audioEffect = "eerie_silence"
            },
            new PressureData
            {
                pressureId = "pressure_spores",
                displayName = "孢子",
                description = "视听提示失真，需要队友引导。",
                pressureType = "Spores",
                warningDurationSeconds = 8f,
                activeDurationSeconds = 150f,
                affectedStates = new[] { "disoriented", "hallucinating" },
                counterTools = new[] { "eco_jar", "beacon" },
                recoveryMethod = "move_to_clean_air",
                recoveryTimeSeconds = 10f,
                visualEffect = "spore_overlay",
                audioEffect = "muffled_sounds"
            },
            new PressureData
            {
                pressureId = "pressure_instability",
                displayName = "地表不稳定",
                description = "地表裂缝和滑落，需要小心移动。",
                pressureType = "Instability",
                warningDurationSeconds = 12f,
                activeDurationSeconds = 90f,
                affectedStates = new[] { "unstable", "falling" },
                counterTools = new[] { "repair_beam", "tether_rope" },
                recoveryMethod = "find_stable_ground",
                recoveryTimeSeconds = 5f,
                visualEffect = "ground_shake",
                audioEffect = "rumbling"
            },
        };
    }

    [System.Serializable]
    public class PressureData
    {
        public string pressureId;
        public string displayName;
        [TextArea(1, 3)] public string description;
        public string pressureType;
        public float warningDurationSeconds;
        public float activeDurationSeconds;
        public string[] affectedStates;
        public string[] counterTools;
        public string recoveryMethod;
        public float recoveryTimeSeconds;
        public string visualEffect;
        public string audioEffect;
    }
}
