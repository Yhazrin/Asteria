using System.Collections.Generic;
using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Quality gate definitions for the Alpha build.
    /// Contains all quality requirements from ROADMAP_V2.md Milestone I.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Quality Gate Database")]
    public sealed class QualityGateDatabase : ScriptableObject
    {
        [Header("Quality Gates")]
        public QualityGate[] gates = new QualityGate[]
        {
            new QualityGate
            {
                gateId = "gate_no_blocker",
                displayName = "无阻断Bug",
                description = "30分钟内不出现阻断Bug。",
                category = "stability",
                threshold = 1f,
                unit = "bugs",
                currentValue = 0f,
                isPassed = true
            },
            new QualityGate
            {
                gateId = "gate_save_upgrade",
                displayName = "存档升级",
                description = "存档连续升级与备份可用。",
                category = "persistence",
                threshold = 1f,
                unit = "pass/fail",
                currentValue = 1f,
                isPassed = true
            },
            new QualityGate
            {
                gateId = "gate_fps",
                displayName = "帧率稳定",
                description = "目标平台稳定60 FPS。",
                category = "performance",
                threshold = 60f,
                unit = "fps",
                currentValue = 60f,
                isPassed = true
            },
            new QualityGate
            {
                gateId = "gate_visual_consistency",
                displayName = "视觉统一",
                description = "UI、音频和三渲二视觉达到统一风格。",
                category = "art",
                threshold = 1f,
                unit = "pass/fail",
                currentValue = 0.5f,
                isPassed = false
            },
            new QualityGate
            {
                gateId = "gate_onboarding",
                displayName = "新手引导",
                description = "新玩家无需开发者说明即可完成首次返家闭环。",
                category = "ux",
                threshold = 1f,
                unit = "pass/fail",
                currentValue = 0.7f,
                isPassed = false
            },
            new QualityGate
            {
                gateId = "gate_save_robustness",
                displayName = "存档健壮性",
                description = "存档损坏时能回退到备份，不覆盖好档。",
                category = "persistence",
                threshold = 1f,
                unit = "pass/fail",
                currentValue = 1f,
                isPassed = true
            },
            new QualityGate
            {
                gateId = "gate_scene_flow",
                displayName = "场景流畅",
                description = "家园→远征→家园场景流可重复5次。",
                category = "stability",
                threshold = 5f,
                unit = "repeats",
                currentValue = 5f,
                isPassed = true
            },
        };
    }

    [System.Serializable]
    public class QualityGate
    {
        public string gateId;
        public string displayName;
        [TextArea(1, 3)] public string description;
        public string category;
        public float threshold;
        public string unit;
        public float currentValue;
        public bool isPassed;
    }
}
