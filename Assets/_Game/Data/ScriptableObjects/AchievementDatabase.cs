using System.Collections.Generic;
using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Database of achievements for the game.
    /// Contains 15+ achievements covering all gameplay systems.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Achievement Database")]
    public sealed class AchievementDatabase : ScriptableObject
    {
        [Header("Achievements")]
        public AchievementData[] achievements = new AchievementData[]
        {
            // Discovery achievements
            new AchievementData
            {
                achievementId = "ach_first_discovery",
                displayName = "初次发现",
                description = "完成第一次观察。",
                category = "discovery",
                targetValue = 1,
                rewardType = "none",
                icon = null
            },
            new AchievementData
            {
                achievementId = "ach_explorer_10",
                displayName = "探索者",
                description = "观察10个兴趣点。",
                category = "discovery",
                targetValue = 10,
                rewardType = "title",
                icon = null
            },
            new AchievementData
            {
                achievementId = "ach_explorer_50",
                displayName = "资深探索者",
                description = "观察50个兴趣点。",
                category = "discovery",
                targetValue = 50,
                rewardType = "title",
                icon = null
            },
            // Expedition achievements
            new AchievementData
            {
                achievementId = "ach_first_expedition",
                displayName = "远征者",
                description = "完成第一次远征。",
                category = "expedition",
                targetValue = 1,
                rewardType = "none",
                icon = null
            },
            new AchievementData
            {
                achievementId = "ach_expedition_10",
                displayName = "远征专家",
                description = "完成10次远征。",
                category = "expedition",
                targetValue = 10,
                rewardType = "title",
                icon = null
            },
            // Social achievements
            new AchievementData
            {
                achievementId = "ach_social_butterfly",
                displayName = "社交蝴蝶",
                description = "与5个居民互动。",
                category = "social",
                targetValue = 5,
                rewardType = "title",
                icon = null
            },
            new AchievementData
            {
                achievementId = "ach_best_friend",
                displayName = "最好的朋友",
                description = "与一个居民达到最大亲密度。",
                category = "social",
                targetValue = 1,
                rewardType = "title",
                icon = null
            },
            // Building achievements
            new AchievementData
            {
                achievementId = "ach_home_builder",
                displayName = "建造者",
                description = "在家园建造3个设施。",
                category = "building",
                targetValue = 3,
                rewardType = "title",
                icon = null
            },
            // Survival achievements
            new AchievementData
            {
                achievementId = "ach_weather_survivor",
                displayName = "天气幸存者",
                description = "在暴风中存活。",
                category = "survival",
                targetValue = 1,
                rewardType = "title",
                icon = null
            },
            // Photo achievements
            new AchievementData
            {
                achievementId = "ach_photographer",
                displayName = "摄影师",
                description = "拍摄10张照片。",
                category = "photo",
                targetValue = 10,
                rewardType = "title",
                icon = null
            },
            // Codex achievements
            new AchievementData
            {
                achievementId = "ach_codex_complete",
                displayName = "图鉴大师",
                description = "收集所有星球类型。",
                category = "codex",
                targetValue = 6,
                rewardType = "title",
                icon = null
            },
            // Cooperation achievements
            new AchievementData
            {
                achievementId = "ach_cooperator",
                displayName = "合作者",
                description = "完成一次合作交互。",
                category = "cooperate",
                targetValue = 1,
                rewardType = "title",
                icon = null
            },
            // Resident achievements
            new AchievementData
            {
                achievementId = "ach_resident_6",
                displayName = "社区领袖",
                description = "家园有6个居民。",
                category = "residents",
                targetValue = 6,
                rewardType = "title",
                icon = null
            },
            // Tool achievements
            new AchievementData
            {
                achievementId = "ach_tool_master",
                displayName = "工具大师",
                description = "使用所有工具类型。",
                category = "tools",
                targetValue = 6,
                rewardType = "title",
                icon = null
            },
            // Time achievements
            new AchievementData
            {
                achievementId = "ach_night_owl",
                displayName = "夜猫子",
                description = "在夜晚探索。",
                category = "time",
                targetValue = 1,
                rewardType = "title",
                icon = null
            },
        };
    }

    [System.Serializable]
    public class AchievementData
    {
        public string achievementId;
        public string displayName;
        [TextArea(1, 3)] public string description;
        public string category;
        public int targetValue;
        public string rewardType;
        public Sprite icon;
    }
}
