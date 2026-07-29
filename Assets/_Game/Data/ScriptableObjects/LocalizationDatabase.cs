using System.Collections.Generic;
using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Localization database for the game.
    /// Contains all translatable strings.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Localization Database")]
    public sealed class LocalizationDatabase : ScriptableObject
    {
        [Header("UI Strings")]
        public string gameTitle = "Asteria";
        public string startButton = "开始冒险";
        public string joinButton = "加入好友";
        public string settingsButton = "设置";
        public string quitButton = "退出";

        [Header("HUD Strings")]
        public string hudControls = "WASD 移动 · Shift 跑 · Space 跳 · 鼠标视角 · E 观察";
        public string hudCodexPrefix = "图鉴记录：";
        public string hudDayPrefix = "第";
        public string hudDaySuffix = "天";

        [Header("Interaction Strings")]
        public string interactObserve = "按 E 观察";
        public string interactRestore = "按住 E 修复";
        public string interactCooperate = "需要多人 · 按 E";
        public string interactDeparture = "按 E 出发远征";
        public string interactReturn = "按 E 返回家园";
        public string interactExamine = "按 E 查看";

        [Header("Discovery Strings")]
        public string discoveryPrefix = "发现 · ";
        public string discoveryToastPrefix = "图鉴 +1 · ";
        public string alreadyObserved = "已观察 · ";

        [Header("Expedition Strings")]
        public string expeditionStart = "出发前往远征星球...";
        public string expeditionReturn = "返回家园...";
        public string expeditionComplete = "远征完成！";

        [Header("Social Strings")]
        public string greetingWarm = "你好呀！";
        public string greetingShy = "...你好。";
        public string farewell = "再见！";
        public string celebration = "太棒了！";

        [Header("Menu Strings")]
        public string menuTitle = "设置";
        public string musicVolume = "音乐音量";
        public string sfxVolume = "音效音量";
        public string mouseSensitivity = "鼠标灵敏度";
        public string invertY = "反转Y轴";
        public string quality = "画质";
        public string fullscreen = "全屏";
        public string apply = "应用";
        public string close = "关闭";
    }
}
