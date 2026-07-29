using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Troubleshooting guide database for the game.
    /// Contains all common issues and solutions.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Troubleshooting Database")]
    public sealed class TroubleshootingDatabase : ScriptableObject
    {
        [Header("Troubleshooting")]
        public TroubleshootEntry[] entries = new TroubleshootEntry[]
        {
            new TroubleshootEntry
            {
                issue = "场景打开后有 Console Error",
                solution = "检查 URP 包是否安装，运行 Asteria/Setup Phase 1 Demo",
                category = "setup"
            },
            new TroubleshootEntry
            {
                issue = "玩家原地转圈",
                solution = "检查 SphericalThirdPersonCamera 的 PlanarForward 是否正确初始化",
                category = "gameplay"
            },
            new TroubleshootEntry
            {
                issue = "存档损坏",
                solution = "系统会自动回退到备份存档，检查 save.json.bak",
                category = "persistence"
            },
            new TroubleshootEntry
            {
                issue = "粉色材质",
                solution = "URP 着色器缺失，检查 Universal Render Pipeline/Lit 是否可用",
                category = "rendering"
            },
            new TroubleshootEntry
            {
                issue = "Missing Script",
                solution = "检查 .meta 文件 GUID 是否正确，使用 Unity Editor 重新关联",
                category = "editor"
            },
            new TroubleshootEntry
            {
                issue = "极点翻转",
                solution = "检查 SphericalMotor 的 GetTangentMoveDirection 是否正确处理极点",
                category = "gameplay"
            },
        };
    }

    [System.Serializable]
    public class TroubleshootEntry
    {
        public string issue;
        public string solution;
        public string category;
    }
}
