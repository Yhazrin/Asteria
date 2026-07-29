using System.Collections.Generic;
using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Database of creature definitions for the game.
    /// Contains 6 creature behavior types as required by WORLD_CONTENT_MATRIX.md §8.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Creature Database")]
    public sealed class CreatureDatabase : ScriptableObject
    {
        [Header("Creatures")]
        public CreatureData[] creatures = new CreatureData[]
        {
            new CreatureData
            {
                creatureId = "curious_creature",
                displayName = "好奇生物",
                description = "主动靠近玩家，模仿、赠物、拍照。制造喜剧与陪伴。",
                behavior = "Curious",
                moveSpeed = 3f,
                detectionRadius = 10f,
                interactionRadius = 3f,
                bodyColor = new Color(0.9f, 0.8f, 0.5f),
                scale = 0.8f,
                preferredBiomes = new[] { "Wind", "Plains" },
                playerRelation = "主动靠近",
                mainInteraction = "模仿、赠物、拍照",
                designValue = "制造喜剧与陪伴"
            },
            new CreatureData
            {
                creatureId = "shy_creature",
                displayName = "胆小生物",
                description = "保持距离，慢速接近、环境安抚。路线与耐心。",
                behavior = "Shy",
                moveSpeed = 4f,
                detectionRadius = 8f,
                interactionRadius = 2f,
                bodyColor = new Color(0.6f, 0.7f, 0.9f),
                scale = 0.6f,
                preferredBiomes = new[] { "Forest", "Mist" },
                playerRelation = "保持距离",
                mainInteraction = "慢速接近、环境安抚",
                designValue = "路线与耐心"
            },
            new CreatureData
            {
                creatureId = "group_creature",
                displayName = "群居生物",
                description = "受整体状态影响，引导群体、保护迁徙。多人分工。",
                behavior = "Group",
                moveSpeed = 2.5f,
                detectionRadius = 15f,
                interactionRadius = 5f,
                bodyColor = new Color(0.7f, 0.8f, 0.6f),
                scale = 1f,
                preferredBiomes = new[] { "Plains", "Savanna" },
                playerRelation = "受整体状态影响",
                mainInteraction = "引导群体、保护迁徙",
                designValue = "多人分工"
            },
            new CreatureData
            {
                creatureId = "symbiotic_creature",
                displayName = "共生生物",
                description = "与植物/设施关联，Restore后出现。展示生态反馈。",
                behavior = "Symbiotic",
                moveSpeed = 1.5f,
                detectionRadius = 5f,
                interactionRadius = 2f,
                bodyColor = new Color(0.5f, 0.9f, 0.6f),
                scale = 0.7f,
                preferredBiomes = new[] { "Forest", "Bloom" },
                playerRelation = "与植物/设施关联",
                mainInteraction = "Restore后出现",
                designValue = "展示生态反馈"
            },
            new CreatureData
            {
                creatureId = "guide_creature",
                displayName = "引路生物",
                description = "知道隐藏路径，跟随声音或动作。非UI导航。",
                behavior = "Guide",
                moveSpeed = 3.5f,
                detectionRadius = 20f,
                interactionRadius = 4f,
                bodyColor = new Color(0.8f, 0.9f, 0.5f),
                scale = 1.2f,
                preferredBiomes = new[] { "Ruin", "Night" },
                playerRelation = "知道隐藏路径",
                mainInteraction = "跟随声音或动作",
                designValue = "非UI导航"
            },
            new CreatureData
            {
                creatureId = "disturbing_creature",
                displayName = "扰动生物",
                description = "改变工具/天气，观察规律而非攻击。临场意外。",
                behavior = "Disturbing",
                moveSpeed = 5f,
                detectionRadius = 12f,
                interactionRadius = 3f,
                bodyColor = new Color(0.9f, 0.5f, 0.5f),
                scale = 0.9f,
                preferredBiomes = new[] { "Ice", "Ruin" },
                playerRelation = "改变工具/天气",
                mainInteraction = "观察规律而非攻击",
                designValue = "临场意外"
            },
        };
    }

    [System.Serializable]
    public class CreatureData
    {
        public string creatureId;
        public string displayName;
        [TextArea(1, 3)] public string description;
        public string behavior;
        public float moveSpeed;
        public float detectionRadius;
        public float interactionRadius;
        public Color bodyColor;
        public float scale;
        public string[] preferredBiomes;
        public string playerRelation;
        public string mainInteraction;
        public string designValue;
    }
}
