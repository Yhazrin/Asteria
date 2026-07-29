using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Release notes database.
    /// Contains all release notes for the game.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Release Notes Database")]
    public sealed class ReleaseNotesDatabase : ScriptableObject
    {
        [Header("Release Notes")]
        public ReleaseNote[] notes = new ReleaseNote[]
        {
            new ReleaseNote
            {
                version = "0.1.0-alpha",
                date = "2026-07-28",
                title = "Alpha 初始版本",
                changes = new[]
                {
                    "球面移动系统",
                    "Observe 交互系统",
                    "家园星球系统",
                    "远征星球系统",
                    "居民模拟系统",
                    "存档系统",
                    "基础 UI 系统",
                    "程序化地形生成",
                    "天气系统",
                    "生物系统"
                }
            },
        };
    }

    [System.Serializable]
    public class ReleaseNote
    {
        public string version;
        public string date;
        public string title;
        [TextArea(3, 6)] public string[] changes;
    }
}
