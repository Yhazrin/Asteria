using System.Collections.Generic;
using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Database of all facilities for the home planet.
    /// Contains 8 facilities as required by CORE_GAMEPLAY_AND_SYSTEMS.md §5.1.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Facility Database")]
    public sealed class FacilityDatabase : ScriptableObject
    {
        [Header("Facilities")]
        public FacilityData[] facilities = new FacilityData[]
        {
            new FacilityData
            {
                facilityId = "observatory",
                displayName = "观测台",
                description = "展示从远征带回的发现。高处视野开阔。",
                facilityType = "observation",
                requiredAnchorSize = "Large",
                unlockedScheduleSlots = new[] { "observe", "stargaze" },
                unlockedEvents = new[] { "stargazing_together" },
                unlockedWishes = new[] { "wish_see_aurora" },
                behavioralImpact = "居民可以在高处观察和发呆"
            },
            new FacilityData
            {
                facilityId = "shared_kitchen",
                displayName = "共享厨房",
                description = "居民一起做饭、分享食物的地方。",
                facilityType = "social",
                requiredAnchorSize = "Large",
                unlockedScheduleSlots = new[] { "cook", "eat_together" },
                unlockedEvents = new[] { "cooking_competition", "recipe_sharing" },
                unlockedWishes = new[] { "wish_taste_new_food" },
                behavioralImpact = "居民可以一起做饭和吃饭"
            },
            new FacilityData
            {
                facilityId = "wind_bell_plaza",
                displayName = "风铃广场",
                description = "社区核心空间。可举办聚会、冲突调解和庆典。",
                facilityType = "social",
                requiredAnchorSize = "Large",
                unlockedScheduleSlots = new[] { "socialize", "celebrate" },
                unlockedEvents = new[] { "community_festival", "conflict_mediation" },
                unlockedWishes = new[] { "wish_make_wind_chime" },
                behavioralImpact = "居民可以举办聚会和庆典"
            },
            new FacilityData
            {
                facilityId = "greenhouse",
                displayName = "温室",
                description = "种植从远征带回的植物。居民可以照料和研究。",
                facilityType = "ecology",
                requiredAnchorSize = "Medium",
                unlockedScheduleSlots = new[] { "garden", "research" },
                unlockedEvents = new[] { "plant_growth", "seed_exchange" },
                unlockedWishes = new[] { "wish_blue_plant" },
                behavioralImpact = "居民可以种植和照料植物"
            },
            new FacilityData
            {
                facilityId = "workshop",
                displayName = "工坊",
                description = "制作物品和工具的地方。居民可以一起工作。",
                facilityType = "production",
                requiredAnchorSize = "Medium",
                unlockedScheduleSlots = new[] { "craft", "repair" },
                unlockedEvents = new[] { "crafting_together" },
                unlockedWishes = new[] { "wish_make_something" },
                behavioralImpact = "居民可以一起制作和修理"
            },
            new FacilityData
            {
                facilityId = "memorial_hall",
                displayName = "纪念馆",
                description = "展示共同回忆和远征纪念品。",
                facilityType = "memory",
                requiredAnchorSize = "Medium",
                unlockedScheduleSlots = new[] { "remember", "reflect" },
                unlockedEvents = new[] { "memory_sharing" },
                unlockedWishes = new[] { "wish_remember_past" },
                behavioralImpact = "居民可以回忆过去的故事"
            },
            new FacilityData
            {
                facilityId = "transport_tower",
                displayName = "交通塔",
                description = "快速移动到星球其他区域。",
                facilityType = "transport",
                requiredAnchorSize = "Large",
                unlockedScheduleSlots = new[] { "travel" },
                unlockedEvents = new[] { "travel_together" },
                unlockedWishes = new[] { "wish_explore_home" },
                behavioralImpact = "居民可以快速移动到不同区域"
            },
            new FacilityData
            {
                facilityId = "residence",
                displayName = "居民住宅",
                description = "居民的私人生活空间。",
                facilityType = "residential",
                requiredAnchorSize = "Medium",
                unlockedScheduleSlots = new[] { "rest", "sleep", "personal_time" },
                unlockedEvents = new[] { "roommate_conflict", "housewarming" },
                unlockedWishes = new[] { "wish_own_room" },
                behavioralImpact = "居民有私人空间休息和生活"
            },
        };
    }

    [System.Serializable]
    public class FacilityData
    {
        public string facilityId;
        public string displayName;
        [TextArea(2, 4)] public string description;
        public string facilityType;
        public string requiredAnchorSize;
        public string[] unlockedScheduleSlots;
        public string[] unlockedEvents;
        public string[] unlockedWishes;
        public string behavioralImpact;
    }
}
