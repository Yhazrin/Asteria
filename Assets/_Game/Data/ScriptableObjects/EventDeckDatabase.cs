using System.Collections.Generic;
using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Database of event decks for the wind grassland expedition.
    /// Contains 8 events as required by WORLD_CONTENT_MATRIX.md §7.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Event Deck Database")]
    public sealed class EventDeckDatabase : ScriptableObject
    {
        [Header("Event Decks")]
        public EventDeckData[] events = new EventDeckData[]
        {
            new EventDeckData
            {
                eventId = "wind_direction_test",
                title = "风向初测",
                description = "测试风向，多人可从不同半球校准。",
                phase = "Arrival",
                systems = new[] { "Observe" },
                multiplayerDifference = "多人可从不同半球校准",
                homeReward = "解锁当天风图",
                duration = 120f,
                requiredPoiType = "Observe"
            },
            new EventDeckData
            {
                eventId = "silent_bell",
                title = "失声的风铃石",
                description = "风铃石没有声音，需要找到原因。",
                phase = "Invitation",
                systems = new[] { "Observe", "Care" },
                multiplayerDifference = "一人寻找，一人维持照明",
                homeReward = "居民想制作风铃",
                duration = 180f,
                requiredPoiType = "Observe"
            },
            new EventDeckData
            {
                eventId = "wind_beast_migration",
                title = "风兽迁徙",
                description = "观察风兽的迁徙路线。",
                phase = "Invitation",
                systems = new[] { "Traverse" },
                multiplayerDifference = "分头占据观测点",
                homeReward = "纪念馆新增照片组",
                duration = 150f,
                requiredPoiType = "Vista"
            },
            new EventDeckData
            {
                eventId = "lost_traveler",
                title = "迷路的小旅人",
                description = "帮助一个迷路的小旅人找到回家的路。",
                phase = "Complication",
                systems = new[] { "Social", "Traverse" },
                multiplayerDifference = "队伍需要包围式引导",
                homeReward = "新星友邀请线索",
                duration = 200f,
                requiredPoiType = "Social"
            },
            new EventDeckData
            {
                eventId = "tower_blades",
                title = "风塔叶片散落",
                description = "修复散落的风塔叶片。",
                phase = "Complication",
                systems = new[] { "Restore" },
                multiplayerDifference = "搬运与安装分工",
                homeReward = "解锁观测台模块",
                duration = 240f,
                requiredPoiType = "Restore"
            },
            new EventDeckData
            {
                eventId = "global_wind",
                title = "全球强风",
                description = "全球强风来袭，需要找到安全路线。",
                phase = "Pressure",
                systems = new[] { "Traverse" },
                multiplayerDifference = "信标链与牵引绳更重要",
                homeReward = "星友讨论谁最可靠",
                duration = 180f,
                requiredPoiType = "Shelter"
            },
            new EventDeckData
            {
                eventId = "bipolar_resonance",
                title = "双极共鸣",
                description = "在星球两侧同时激活共鸣装置。",
                phase = "Resolution",
                systems = new[] { "Cooperate" },
                multiplayerDifference = "两侧同时完成",
                homeReward = "家园出现一夜极光",
                duration = 300f,
                requiredPoiType = "Cooperate"
            },
            new EventDeckData
            {
                eventId = "seed_or_nest",
                title = "留下种子或修复巢穴",
                description = "选择带走种子还是修复风兽的巢穴。",
                phase = "Resolution",
                systems = new[] { "Care", "Restore" },
                multiplayerDifference = "全队投票/房主确认",
                homeReward = "不同生态与居民事件",
                duration = 120f,
                requiredPoiType = "Choice"
            },
        };
    }

    [System.Serializable]
    public class EventDeckData
    {
        public string eventId;
        public string title;
        [TextArea(1, 3)] public string description;
        public string phase;
        public string[] systems;
        public string multiplayerDifference;
        public string homeReward;
        public float duration;
        public string requiredPoiType;
    }
}
