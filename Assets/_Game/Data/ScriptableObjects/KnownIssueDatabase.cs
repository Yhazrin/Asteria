using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Known issue database for the game.
    /// Contains all known issues and workarounds.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Known Issue Database")]
    public sealed class KnownIssueDatabase : ScriptableObject
    {
        [Header("Known Issues")]
        public KnownIssue[] issues = new KnownIssue[]
        {
            new KnownIssue
            {
                issueId = "ki_cinemachine",
                description = "Cinemachine 在 Unity 6 中编译失败",
                severity = "medium",
                workaround = "使用手写相机替代",
                status = "mitigated"
            },
            new KnownIssue
            {
                issueId = "ki_asset_ratio",
                description = "代码资产比失衡，缺少正式美术资源",
                severity = "medium",
                workaround = "使用程序化资产和免费资源包",
                status = "in_progress"
            },
            new KnownIssue
            {
                issueId = "ki_network_ngo",
                description = "Netcode for GameObjects 未集成",
                severity = "low",
                workaround = "使用 LocalAuthorityAdapter 单机模式",
                status = "planned"
            },
            new KnownIssue
            {
                issueId = "ki_audio_missing",
                description = "缺少正式音效和音乐文件",
                severity = "low",
                workaround = "使用程序化音效生成",
                status = "planned"
            },
        };
    }

    [System.Serializable]
    public class KnownIssue
    {
        public string issueId;
        public string description;
        public string severity;
        public string workaround;
        public string status; // "identified", "mitigated", "fixed", "planned"
    }
}
