using System.Collections.Generic;
using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Database of tool upgrades for the game.
    /// Contains upgrades for all 6 tools.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Upgrade Database")]
    public sealed class UpgradeDatabase : ScriptableObject
    {
        [Header("Upgrades")]
        public UpgradeData[] upgrades = new UpgradeData[]
        {
            // Resonance Mirror upgrades
            new UpgradeData
            {
                upgradeId = "mirror_extended_range",
                displayName = "扩展范围",
                description = "共鸣镜的扫描范围增加50%。",
                targetToolId = "resonance_mirror",
                requiredLevel = 1,
                energyBonus = 20f,
                requiredDiscoveries = new[] { "wind_bell_01" },
                effectDescription = "扫描范围 +50%"
            },
            new UpgradeData
            {
                upgradeId = "mirror_signal_filter",
                displayName = "信号过滤",
                description = "共鸣镜可以过滤干扰信号。",
                targetToolId = "resonance_mirror",
                requiredLevel = 2,
                energyBonus = 15f,
                requiredDiscoveries = new[] { "mist_path" },
                effectDescription = "过滤干扰信号"
            },
            // Warm Light upgrades
            new UpgradeData
            {
                upgradeId = "light_larger_radius",
                displayName = "扩大范围",
                description = "暖光灯的安全区范围增加30%。",
                targetToolId = "warm_light",
                requiredLevel = 1,
                energyBonus = 15f,
                requiredDiscoveries = new[] { "night_path" },
                effectDescription = "安全区范围 +30%"
            },
            new UpgradeData
            {
                upgradeId = "light_longer_duration",
                displayName = "延长持续",
                description = "暖光灯的持续时间增加50%。",
                targetToolId = "warm_light",
                requiredLevel = 2,
                energyBonus = 20f,
                requiredDiscoveries = new[] { "ice_shelter" },
                effectDescription = "持续时间 +50%"
            },
            // Beacon upgrades
            new UpgradeData
            {
                upgradeId = "beacon_visible_range",
                displayName = "可见范围",
                description = "信标的可见范围增加100%。",
                targetToolId = "beacon",
                requiredLevel = 1,
                energyBonus = 10f,
                requiredDiscoveries = new[] { "wind_bell_01" },
                effectDescription = "可见范围 +100%"
            },
            new UpgradeData
            {
                upgradeId = "beacon_signal_strength",
                displayName = "信号强度",
                description = "信标的信号强度增加，可以穿透障碍物。",
                targetToolId = "beacon",
                requiredLevel = 2,
                energyBonus = 15f,
                requiredDiscoveries = new[] { "ruin_signal" },
                effectDescription = "穿透障碍物"
            },
            // Repair Beam upgrades
            new UpgradeData
            {
                upgradeId = "beam_faster_repair",
                displayName = "快速修复",
                description = "修复束的修复速度增加40%。",
                targetToolId = "repair_beam",
                requiredLevel = 1,
                energyBonus = 15f,
                requiredDiscoveries = new[] { "tower_repair" },
                effectDescription = "修复速度 +40%"
            },
            new UpgradeData
            {
                upgradeId = "beam_multi_target",
                displayName = "多重修复",
                description = "修复束可以同时修复多个目标。",
                targetToolId = "repair_beam",
                requiredLevel = 2,
                energyBonus = 25f,
                requiredDiscoveries = new[] { "bloom_restore" },
                effectDescription = "同时修复3个目标"
            },
            // Tether Rope upgrades
            new UpgradeData
            {
                upgradeId = "rope_longer_range",
                displayName = "更长绳索",
                description = "牵引绳的长度增加60%。",
                targetToolId = "tether_rope",
                requiredLevel = 1,
                energyBonus = 10f,
                requiredDiscoveries = new[] { "canyon_crossing" },
                effectDescription = "绳索长度 +60%"
            },
            new UpgradeData
            {
                upgradeId = "rope_stronger_grip",
                displayName = "强力抓握",
                description = "牵引绳可以拉动更重的物体。",
                targetToolId = "tether_rope",
                requiredLevel = 2,
                energyBonus = 20f,
                requiredDiscoveries = new[] { "heavy_object" },
                effectDescription = "拉力 +100%"
            },
            // Eco Jar upgrades
            new UpgradeData
            {
                upgradeId = "jar_larger_capacity",
                displayName = "大容量",
                description = "生态瓶可以存储更多种子和生物。",
                targetToolId = "eco_jar",
                requiredLevel = 1,
                energyBonus = 15f,
                requiredDiscoveries = new[] { "rare_seed" },
                effectDescription = "容量 +50%"
            },
            new UpgradeData
            {
                upgradeId = "jar_preservation",
                displayName = "保存能力",
                description = "生态瓶可以更好地保存珍稀样本。",
                targetToolId = "eco_jar",
                requiredLevel = 2,
                energyBonus = 20f,
                requiredDiscoveries = new[] { "ancient_specimen" },
                effectDescription = "保存时间 +100%"
            },
        };
    }

    [System.Serializable]
    public class UpgradeData
    {
        public string upgradeId;
        public string displayName;
        [TextArea(1, 3)] public string description;
        public string targetToolId;
        public int requiredLevel;
        public float energyBonus;
        public string[] requiredDiscoveries;
        public string effectDescription;
    }
}
