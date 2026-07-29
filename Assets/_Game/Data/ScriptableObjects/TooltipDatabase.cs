using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Tooltip definitions for UI elements.
    /// Contains hover text for buttons, icons, and interactive elements.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Tooltip Database")]
    public sealed class TooltipDatabase : ScriptableObject
    {
        [Header("Tool Tooltips")]
        public string tipResonanceMirror = "共鸣镜：扫描周围的声音和生命信号";
        public string tipWarmLight = "暖光灯：建立安全区域，驱散黑暗";
        public string tipBeacon = "信标：标记路线，队友可以看到";
        public string tipRepairBeam = "修复束：修复损坏的设施和地表";
        public string tipTetherRope = "牵引绳：连接队友，救援和搬运";
        public string tipEcoJar = "生态瓶：收集种子和小型生物";

        [Header("UI Tooltips")]
        public string tipCodex = "图鉴：查看所有发现";
        public string tipInventory = "背包：查看收集的物品";
        public string tipMap = "地图：查看星球全景";
        public string tipPhoto = "拍照：记录美好瞬间";
        public string tipSettings = "设置：调整游戏选项";

        [Header("Status Tooltips")]
        public string tipCold = "受寒：移动速度降低";
        public string tipLost = "迷失：方向感减弱";
        public string tipSpore = "孢子影响：视听提示失真";
        public string tipUnstable = "失衡：更容易滑落";
        public string tipRescue = "等待救援中...";
    }
}
