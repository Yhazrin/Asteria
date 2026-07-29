using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Glossary database for the game.
    /// Contains all game terms and definitions.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Glossary Database")]
    public sealed class GlossaryDatabase : ScriptableObject
    {
        [Header("Terms")]
        public GlossaryEntry[] entries = new GlossaryEntry[]
        {
            new GlossaryEntry { term = "Observe", definition = "观察。玩家动作之一，识别与记录环境中的事物。" },
            new GlossaryEntry { term = "Restore", definition = "修复。玩家动作之一，修复装置和生态节点。" },
            new GlossaryEntry { term = "Cooperate", definition = "合作。玩家动作之一，多人共同完成空间任务。" },
            new GlossaryEntry { term = "Care", definition = "安抚/照料。玩家动作之一，用于安抚生物、喂养、照料和陪伴。" },
            new GlossaryEntry { term = "Traverse", definition = "移动。玩家动作之一，行走、跑、跳、滑行/滑翔。" },
            new GlossaryEntry { term = "Place", definition = "放置。玩家动作之一，放置受限痕迹和临时工具。" },
            new GlossaryEntry { term = "Socialize", definition = "社交。玩家动作之一，表情、对话建议、合影、赠礼。" },
            new GlossaryEntry { term = "Resident", definition = "星友。家园中的自主生活角色。" },
            new GlossaryEntry { term = "Expedition", definition = "远征。20–40 分钟一局的可重复球形探索空间。" },
            new GlossaryEntry { term = "Home Planet", definition = "家园星球。长期保存的社区空间。" },
            new GlossaryEntry { term = "POI", definition = "兴趣点。星球上的交互位置。" },
            new GlossaryEntry { term = "Biome", definition = "生态区。星球表面的功能区域划分。" },
            new GlossaryEntry { term = "Pressure", definition = "压力。事件型轻生存中的环境压力。" },
            new GlossaryEntry { term = "Wish", definition = "愿望。居民提出的可被玩家理解的具体诉求。" },
            new GlossaryEntry { term = "Memory", definition = "记忆。居民或玩家经历的重要事件记录。" },
            new GlossaryEntry { term = "Relationship", definition = "关系。两居民之间的多维度关系数据。" },
            new GlossaryEntry { term = "Facility", definition = "设施。家园中的可建造建筑。" },
            new GlossaryEntry { term = "Tool", definition = "工具。玩家携带的主动工具。" },
            new GlossaryEntry { term = "BuildAnchor", definition = "建设锚点。家园星球表面预设的设施放置位置。" },
            new GlossaryEntry { term = "Event Director", definition = "事件导演。负责根据条件选择和调度事件的系统。" },
        };
    }

    [System.Serializable]
    public class GlossaryEntry
    {
        public string term;
        [TextArea(1, 3)] public string definition;
    }
}
