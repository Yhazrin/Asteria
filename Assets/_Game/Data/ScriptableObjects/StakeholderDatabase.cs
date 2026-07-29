using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Stakeholder database for the game.
    /// Contains all stakeholder roles and responsibilities.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Stakeholder Database")]
    public sealed class StakeholderDatabase : ScriptableObject
    {
        [Header("Stakeholders")]
        public Stakeholder[] stakeholders = new Stakeholder[]
        {
            new Stakeholder
            {
                role = "Game Designer",
                responsibilities = new[] { "产品愿景", "核心玩法", "内容规划" },
                documents = new[] { "PRODUCT_VISION_V2.md", "CORE_GAMEPLAY_AND_SYSTEMS.md" }
            },
            new Stakeholder
            {
                role = "Programmer",
                responsibilities = new[] { "技术架构", "系统实现", "性能优化" },
                documents = new[] { "TECHNICAL_ARCHITECTURE.md", "DATA_CONTRACTS.md" }
            },
            new Stakeholder
            {
                role = "Artist",
                responsibilities = new[] { "视觉风格", "材质制作", "环境设计" },
                documents = new[] { "ART_STYLE_GUIDE.md" }
            },
            new Stakeholder
            {
                role = "Sound Designer",
                responsibilities = new[] { "音效设计", "音乐制作", "空间音频" },
                documents = new[] { "AUDIO_DESIGN.md" }
            },
            new Stakeholder
            {
                role = "QA",
                responsibilities = new[] { "测试策略", "回归测试", "Bug 报告" },
                documents = new[] { "TEST_SPEC.md" }
            },
            new Stakeholder
            {
                role = "Producer",
                responsibilities = new[] { "项目管理", "进度跟踪", "风险控制" },
                documents = new[] { "ROADMAP_V2.md", "DECISION_LOG.md" }
            },
        };
    }

    [System.Serializable]
    public class Stakeholder
    {
        public string role;
        public string[] responsibilities;
        public string[] documents;
    }
}
