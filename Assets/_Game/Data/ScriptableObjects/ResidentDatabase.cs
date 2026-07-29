using System.Collections.Generic;
using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Database of all resident definitions for the home planet.
    /// Contains 6+ residents as required by Milestone I.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Resident Database")]
    public sealed class ResidentDatabase : ScriptableObject
    {
        [Header("Residents")]
        public ResidentData[] residents = new ResidentData[]
        {
            new ResidentData
            {
                residentId = "lian",
                displayName = "莲",
                pronouns = "she/her",
                description = "温暖、合群的居民。会给所有植物取名字。",
                sociability = 0.6f,
                curiosity = 0.4f,
                warmth = 0.7f,
                order = 0.3f,
                boldness = 0.2f,
                quirks = new[] { "会给所有植物取名字" },
                bodyColor = new Color(0.85f, 0.75f, 0.8f),
                preferredActivities = new[] { "social", "garden", "cooking" },
                dislikedActivities = new[] { "explore_alone" }
            },
            new ResidentData
            {
                residentId = "kai",
                displayName = "凯",
                pronouns = "he/him",
                description = "好奇、大胆的居民。害怕下坡却喜欢高处。",
                sociability = -0.3f,
                curiosity = 0.8f,
                warmth = 0.1f,
                order = 0.6f,
                boldness = 0.7f,
                quirks = new[] { "害怕下坡却喜欢高处" },
                bodyColor = new Color(0.7f, 0.8f, 0.85f),
                preferredActivities = new[] { "explore", "observe", "high_places" },
                dislikedActivities = new[] { "cooking", "gardening" }
            },
            new ResidentData
            {
                residentId = "qing",
                displayName = "晴",
                pronouns = "she/her",
                description = "开朗、有条理的居民。总想把严肃场合变成合影。",
                sociability = 0.5f,
                curiosity = 0.2f,
                warmth = 0.6f,
                order = 0.8f,
                boldness = 0.1f,
                quirks = new[] { "总想把严肃场合变成合影" },
                bodyColor = new Color(0.95f, 0.85f, 0.7f),
                preferredActivities = new[] { "social", "organize", "photo" },
                dislikedActivities = new[] { "messy_activities" }
            },
            new ResidentData
            {
                residentId = "shuang",
                displayName = "霜",
                pronouns = "they/them",
                description = "内向、好奇的居民。对风铃声异常敏感。",
                sociability = -0.5f,
                curiosity = 0.9f,
                warmth = -0.3f,
                order = 0.4f,
                boldness = 0.5f,
                quirks = new[] { "对风铃声异常敏感" },
                bodyColor = new Color(0.75f, 0.85f, 0.9f),
                preferredActivities = new[] { "observe", "listen", "solitude" },
                dislikedActivities = new[] { "crowds", "loud_events" }
            },
            new ResidentData
            {
                residentId = "yan",
                displayName = "岩",
                pronouns = "he/him",
                description = "大胆、有条理的居民。一紧张就开始整理东西。",
                sociability = 0.1f,
                curiosity = 0.3f,
                warmth = 0.2f,
                order = 0.7f,
                boldness = 0.8f,
                quirks = new[] { "一紧张就开始整理东西" },
                bodyColor = new Color(0.8f, 0.75f, 0.65f),
                preferredActivities = new[] { "build", "organize", "explore" },
                dislikedActivities = new[] { "waiting", "doing_nothing" }
            },
            new ResidentData
            {
                residentId = "yun",
                displayName = "云",
                pronouns = "she/her",
                description = "梦幻、温暖的居民。喜欢在高处发呆。",
                sociability = 0.3f,
                curiosity = 0.6f,
                warmth = 0.8f,
                order = -0.2f,
                boldness = -0.1f,
                quirks = new[] { "喜欢在高处发呆" },
                bodyColor = new Color(0.9f, 0.88f, 0.92f),
                preferredActivities = new[] { "high_places", "observe", "dream" },
                dislikedActivities = new[] { "rushing", "deadlines" }
            },
        };
    }

    [System.Serializable]
    public class ResidentData
    {
        public string residentId;
        public string displayName;
        public string pronouns;
        [TextArea(2, 4)] public string description;

        [Header("Personality")]
        [Range(-1f, 1f)] public float sociability;
        [Range(-1f, 1f)] public float curiosity;
        [Range(-1f, 1f)] public float warmth;
        [Range(-1f, 1f)] public float order;
        [Range(-1f, 1f)] public float boldness;

        [Header("Quirks")]
        public string[] quirks;

        [Header("Appearance")]
        public Color bodyColor = new(0.9f, 0.85f, 0.8f);

        [Header("Preferences")]
        public string[] preferredActivities;
        public string[] dislikedActivities;
    }
}
