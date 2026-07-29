using System.Collections.Generic;
using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Database of all social events for the home planet.
    /// Contains 12+ events as required by Milestone I.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Social Event Database")]
    public sealed class SocialEventDatabase : ScriptableObject
    {
        [Header("Events")]
        public SocialEventData[] events = new SocialEventData[]
        {
            // Daily events
            new SocialEventData
            {
                eventId = "daily_cooking_fail",
                title = "做饭失败",
                description = "一位居民尝试做饭，但把锅烧糊了。其他人闻到味道赶过来。",
                category = "Daily",
                minParticipants = 2,
                maxParticipants = 2,
                cooldownDays = 0.5f,
                requiredPersonality = "any",
                outcomes = new[] { "大家帮忙清理", "一起出去吃" },
                effects = new[] { "affinity +0.05" }
            },
            new SocialEventData
            {
                eventId = "daily_gift_mixup",
                title = "误拿礼物",
                description = "两位居民不小心交换了彼此的礼物，发现后都很尴尬。",
                category = "Daily",
                minParticipants = 2,
                maxParticipants = 2,
                cooldownDays = 1f,
                requiredPersonality = "any",
                outcomes = new[] { "交换回来", "干脆送给对方" },
                effects = new[] { "affinity +0.03", "familiarity +0.05" }
            },
            new SocialEventData
            {
                eventId = "daily_window_seat",
                title = "争抢窗边位置",
                description = "两位居民都想坐在窗边看风景，互不相让。",
                category = "Conflict",
                minParticipants = 2,
                maxParticipants = 2,
                cooldownDays = 0.5f,
                requiredPersonality = "order > 0.3",
                outcomes = new[] { "轮流坐", "一起挤一挤" },
                effects = new[] { "tension +0.1" }
            },
            new SocialEventData
            {
                eventId = "daily_plant_naming",
                title = "给植物取名字",
                description = "一位居民给花园里的每棵植物都取了名字，其他人觉得很可爱。",
                category = "Daily",
                minParticipants = 1,
                maxParticipants = 3,
                cooldownDays = 2f,
                requiredPersonality = "warmth > 0.5",
                outcomes = new[] { "大家加入取名", "觉得太幼稚" },
                effects = new[] { "affinity +0.05", "mood +0.1" }
            },
            // Relationship events
            new SocialEventData
            {
                eventId = "rel_share_expedition",
                title = "分享远征故事",
                description = "一位居民向另一位讲述远征中的惊险经历。",
                category = "Relationship",
                minParticipants = 2,
                maxParticipants = 2,
                cooldownDays = 2f,
                requiredPersonality = "curiosity > 0.3",
                outcomes = new[] { "被故事吸引", "觉得太夸张" },
                effects = new[] { "affinity +0.1", "trust +0.05" }
            },
            new SocialEventData
            {
                eventId = "rel_practice_together",
                title = "一起练习",
                description = "两位居民发现彼此有相同的爱好，决定一起练习。",
                category = "Relationship",
                minParticipants = 2,
                maxParticipants = 2,
                cooldownDays = 3f,
                requiredPersonality = "sociability > 0.3",
                outcomes = new[] { "成为练习伙伴", "发现水平差距太大" },
                effects = new[] { "affinity +0.08", "familiarity +0.1" }
            },
            // Conflict events
            new SocialEventData
            {
                eventId = "conflict_late",
                title = "约定迟到",
                description = "一位居民迟到很久，另一位等得很不耐烦。",
                category = "Conflict",
                minParticipants = 2,
                maxParticipants = 2,
                cooldownDays = 1f,
                requiredPersonality = "order > 0.5",
                outcomes = new[] { "道歉并解释", "觉得对方不尊重自己" },
                effects = new[] { "tension +0.15" }
            },
            new SocialEventData
            {
                eventId = "conflict_facility_habit",
                title = "设施使用习惯不同",
                description = "两位居民对公共设施的使用习惯完全不同，产生摩擦。",
                category = "Conflict",
                minParticipants = 2,
                maxParticipants = 2,
                cooldownDays = 2f,
                requiredPersonality = "any",
                outcomes = new[] { "协商规则", "各用各的" },
                effects = new[] { "tension +0.1" }
            },
            // Community events
            new SocialEventData
            {
                eventId = "community_festival",
                title = "社区庆典",
                description = "居民们自发举办小型庆祝活动，装饰广场并分享食物。",
                category = "Community",
                minParticipants = 3,
                maxParticipants = 6,
                cooldownDays = 5f,
                requiredPersonality = "any",
                outcomes = new[] { "热闹的庆典", "有些人不太想参加" },
                effects = new[] { "affinity +0.1 for all", "mood +0.2 for all" }
            },
            new SocialEventData
            {
                eventId = "community_performance",
                title = "小型演出",
                description = "一位有才华的居民为其他人表演，大家都很开心。",
                category = "Community",
                minParticipants = 2,
                maxParticipants = 6,
                cooldownDays = 3f,
                requiredPersonality = "warmth > 0.3",
                outcomes = new[] { "精彩的表演", "有点紧张发挥失常" },
                effects = new[] { "affinity +0.08", "admiration +0.1" }
            },
            // Expedition follow-up events
            new SocialEventData
            {
                eventId = "expedition_discussion",
                title = "讨论远征结果",
                description = "居民们讨论最近一次远征的发现和收获。",
                category = "ExpeditionFollowUp",
                minParticipants = 2,
                maxParticipants = 4,
                cooldownDays = 1f,
                requiredPersonality = "curiosity > 0.2",
                outcomes = new[] { "热烈讨论", "对某些决定有分歧" },
                effects = new[] { "affinity +0.05", "exploration +0.1" }
            },
            // Surprise events
            new SocialEventData
            {
                eventId = "surprise_weather",
                title = "意外天气",
                description = "突然下起小雨，居民们匆忙找地方避雨，意外地很开心。",
                category = "Surprise",
                minParticipants = 2,
                maxParticipants = 4,
                cooldownDays = 7f,
                requiredPersonality = "any",
                outcomes = new[] { "一起淋雨玩", "找个地方躲雨聊天" },
                effects = new[] { "affinity +0.05", "mood +0.1" }
            },
        };
    }

    [System.Serializable]
    public class SocialEventData
    {
        public string eventId;
        public string title;
        [TextArea(2, 4)] public string description;
        public string category;
        public int minParticipants = 2;
        public int maxParticipants = 2;
        public float cooldownDays = 1f;
        public string requiredPersonality;
        public string[] outcomes;
        public string[] effects;
    }
}
