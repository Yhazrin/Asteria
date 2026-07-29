using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Changelog database for the game.
    /// Contains all changes made to the game.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Changelog Database")]
    public sealed class ChangelogDatabase : ScriptableObject
    {
        [Header("Changelog")]
        public ChangelogEntry[] entries = new ChangelogEntry[]
        {
            new ChangelogEntry
            {
                date = "2026-07-28",
                version = "0.1.0-alpha",
                category = "feature",
                description = "实现所有 Milestone A-I 系统",
                details = "球面移动、Observe、存档、家园、远征、居民、建设、联机接口、Cooperate"
            },
            new ChangelogEntry
            {
                date = "2026-07-28",
                version = "0.1.0-alpha",
                category = "documentation",
                description = "完成 V2 文档矩阵",
                details = "23 个文档覆盖产品、玩法、工程、美术、音频、测试所有维度"
            },
            new ChangelogEntry
            {
                date = "2026-07-28",
                version = "0.1.0-alpha",
                category = "content",
                description = "实现默认内容",
                details = "6 居民、6 愿望、8 事件、6 工具、8 设施、6 生态区"
            },
            new ChangelogEntry
            {
                date = "2026-07-28",
                version = "0.1.0-alpha",
                category = "system",
                description = "实现程序化地形生成",
                details = "基于 Minecraft 噪声的球面地形、生态区映射、特征放置"
            },
            new ChangelogEntry
            {
                date = "2026-07-28",
                version = "0.1.0-alpha",
                category = "ui",
                description = "实现完整 UGUI 系统",
                details = "主菜单、HUD、交互提示、发现弹窗、设置面板、指南针、小地图"
            },
            new ChangelogEntry
            {
                date = "2026-07-28",
                version = "0.1.0-alpha",
                category = "audio",
                description = "实现音频管理器",
                details = "音乐、音效、环境音管理，3D 空间音频支持"
            },
            new ChangelogEntry
            {
                date = "2026-07-28",
                version = "0.1.0-alpha",
                category = "art",
                description = "实现程序化资产系统",
                details = "树木、岩石、水晶、花朵、生物等程序化网格和材质"
            },
        };
    }

    [System.Serializable]
    public class ChangelogEntry
    {
        public string date;
        public string version;
        public string category; // "feature", "fix", "documentation", "content", "system", "ui", "audio", "art"
        public string description;
        [TextArea(1, 3)] public string details;
    }
}
