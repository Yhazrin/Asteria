using System.Collections.Generic;
using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Database of all tools available in the game.
    /// Contains 6 tools as required by CORE_GAMEPLAY_AND_SYSTEMS.md §4.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Tool Database")]
    public sealed class ToolDatabase : ScriptableObject
    {
        [Header("Tools")]
        public ToolData[] tools = new ToolData[]
        {
            new ToolData
            {
                toolId = "resonance_mirror",
                displayName = "共鸣镜",
                description = "扫描声音、生命和遗迹信号。可越过地平线显示模糊方向。",
                slotType = "Active1",
                maxEnergy = 100f,
                rechargeRate = 5f,
                interactionTags = new[] { "observe", "scan", "detect" },
                sphericalValue = "可越过地平线显示模糊方向",
                upgrades = new[] { "extended_range", "signal_filter" }
            },
            new ToolData
            {
                toolId = "warm_light",
                displayName = "暖光灯",
                description = "建立短时安全区。形成绕星球的灯链。",
                slotType = "Active2",
                maxEnergy = 100f,
                rechargeRate = 3f,
                interactionTags = new[] { "cold", "dark", "safe_zone", "light" },
                sphericalValue = "形成绕星球的灯链",
                upgrades = new[] { "larger_radius", "longer_duration" }
            },
            new ToolData
            {
                toolId = "beacon",
                displayName = "信标",
                description = "标记路线和安全点。可被其他队友看到。",
                slotType = "SharedBeacon",
                maxEnergy = 100f,
                rechargeRate = 10f,
                interactionTags = new[] { "navigation", "rescue", "mark" },
                sphericalValue = "标记路线和安全点",
                upgrades = new[] { "visible_range", "signal_strength" }
            },
            new ToolData
            {
                toolId = "repair_beam",
                displayName = "修复束",
                description = "修复节点、稳定地表。多人叠加提高效率。",
                slotType = "Active1",
                maxEnergy = 100f,
                rechargeRate = 4f,
                interactionTags = new[] { "restore", "repair", "stabilize" },
                sphericalValue = "多人叠加提高效率",
                upgrades = new[] { "faster_repair", "multi_target" }
            },
            new ToolData
            {
                toolId = "tether_rope",
                displayName = "牵引绳",
                description = "救援、搬运、连接队友。极点和陡坡协作。",
                slotType = "Active2",
                maxEnergy = 100f,
                rechargeRate = 6f,
                interactionTags = new[] { "rescue", "carry", "connect" },
                sphericalValue = "极点和陡坡协作",
                upgrades = new[] { "longer_range", "stronger_grip" }
            },
            new ToolData
            {
                toolId = "eco_jar",
                displayName = "生态瓶",
                description = "暂存种子或小型生物。返家后影响生态展示。",
                slotType = "SharedBeacon",
                maxEnergy = 100f,
                rechargeRate = 8f,
                interactionTags = new[] { "collect", "store", "transport" },
                sphericalValue = "返家后影响生态展示",
                upgrades = new[] { "larger_capacity", "preservation" }
            },
        };
    }

    [System.Serializable]
    public class ToolData
    {
        public string toolId;
        public string displayName;
        [TextArea(1, 3)] public string description;
        public string slotType;
        public float maxEnergy;
        public float rechargeRate;
        public string[] interactionTags;
        public string sphericalValue;
        public string[] upgrades;
    }
}
