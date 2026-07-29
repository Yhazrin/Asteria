using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Development guide database for the game.
    /// Contains all development guidelines and workflows.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Development Guide Database")]
    public sealed class DevelopmentGuideDatabase : ScriptableObject
    {
        [Header("Guides")]
        public DevGuide[] guides = new DevGuide[]
        {
            new DevGuide
            {
                guideId = "dg_setup",
                title = "项目设置",
                description = "如何设置开发环境",
                steps = new[]
                {
                    "安装 Unity 6000.5.5f1",
                    "克隆仓库",
                    "打开项目",
                    "运行 Asteria/Setup Milestone B Scenes",
                    "按 Play 测试"
                }
            },
            new DevGuide
            {
                guideId = "dg_new_feature",
                title = "添加新功能",
                description = "如何添加新功能到项目",
                steps = new[]
                {
                    "阅读 Docs/README.md",
                    "阅读相关系统文档",
                    "更新 DECISION_LOG.md（如需）",
                    "更新 ROADMAP_V2.md（如需）",
                    "实现功能",
                    "添加测试",
                    "提交并推送"
                }
            },
            new DevGuide
            {
                guideId = "dg_save_system",
                title = "存档系统",
                description = "如何使用和扩展存档系统",
                steps = new[]
                {
                    "使用 SaveService.LoadOrCreate() 加载",
                    "修改 SaveRoot 中的数据",
                    "调用 SaveService.Save() 保存",
                    "递增 schemaVersion 并编写迁移函数"
                }
            },
        };
    }

    [System.Serializable]
    public class DevGuide
    {
        public string guideId;
        public string title;
        [TextArea(1, 3)] public string description;
        public string[] steps;
    }
}
