using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Risk register database for the game.
    /// Contains all identified risks and mitigation strategies.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Risk Register Database")]
    public sealed class RiskRegisterDatabase : ScriptableObject
    {
        [Header("Risks")]
        public RiskEntry[] risks = new RiskEntry[]
        {
            new RiskEntry
            {
                riskId = "risk_guid_ref",
                description = "目录/GUID 重整破坏引用",
                severity = "high",
                likelihood = "medium",
                mitigation = "少迁；必须迁时用 Unity 或 git mv，并立刻 Play 回归",
                status = "mitigated"
            },
            new RiskEntry
            {
                riskId = "risk_pole_stability",
                description = "极点附近相机/朝向稳定性",
                severity = "medium",
                likelihood = "medium",
                mitigation = "固定验收路径（赤道→北极→背面→南极）",
                status = "mitigated"
            },
            new RiskEntry
            {
                riskId = "risk_sandbox_drift",
                description = "产品漂移成沙盒",
                severity = "high",
                likelihood = "medium",
                mitigation = "严格禁止项清单",
                status = "controlled"
            },
            new RiskEntry
            {
                riskId = "risk_premature_networking",
                description = "过早上联机",
                severity = "medium",
                likelihood = "low",
                mitigation = "Phase 3 前不装 Netcode",
                status = "mitigated"
            },
            new RiskEntry
            {
                riskId = "risk_cinemachine_conflict",
                description = "Cinemachine 再引入编译失败",
                severity = "medium",
                likelihood = "low",
                mitigation = "Phase 1 继续用手写相机",
                status = "mitigated"
            },
            new RiskEntry
            {
                riskId = "risk_asset_ratio",
                description = "代码资产比失衡",
                severity = "medium",
                likelihood = "high",
                mitigation = "补充程序化资产和免费资源包",
                status = "in_progress"
            },
        };
    }

    [System.Serializable]
    public class RiskEntry
    {
        public string riskId;
        public string description;
        public string severity; // "low", "medium", "high", "critical"
        public string likelihood; // "low", "medium", "high"
        public string mitigation;
        public string status; // "identified", "mitigated", "controlled", "in_progress"
    }
}
