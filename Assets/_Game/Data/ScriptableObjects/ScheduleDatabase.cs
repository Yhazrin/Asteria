using System.Collections.Generic;
using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Database of schedule templates for residents.
    /// Contains default schedules for different personality types.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Schedule Database")]
    public sealed class ScheduleDatabase : ScriptableObject
    {
        [Header("Schedules")]
        public ScheduleData[] schedules = new ScheduleData[]
        {
            new ScheduleData
            {
                scheduleId = "schedule_social",
                displayName = "社交型日程",
                description = "适合外向居民的日程安排。",
                slots = new ScheduleSlotData[]
                {
                    new ScheduleSlotData { time = "morning", activity = "rest", location = "home", duration = 2f },
                    new ScheduleSlotData { time = "late_morning", activity = "social", location = "plaza", duration = 3f },
                    new ScheduleSlotData { time = "afternoon", activity = "cook", location = "kitchen", duration = 2f },
                    new ScheduleSlotData { time = "evening", activity = "social", location = "plaza", duration = 3f },
                    new ScheduleSlotData { time = "night", activity = "rest", location = "home", duration = 2f },
                }
            },
            new ScheduleData
            {
                scheduleId = "schedule_explorer",
                displayName = "探索型日程",
                description = "适合好奇居民的日程安排。",
                slots = new ScheduleSlotData[]
                {
                    new ScheduleSlotData { time = "morning", activity = "rest", location = "home", duration = 2f },
                    new ScheduleSlotData { time = "late_morning", activity = "explore", location = "observatory", duration = 3f },
                    new ScheduleSlotData { time = "afternoon", activity = "observe", location = "high_place", duration = 3f },
                    new ScheduleSlotData { time = "evening", activity = "rest", location = "home", duration = 2f },
                    new ScheduleSlotData { time = "night", activity = "stargaze", location = "observatory", duration = 2f },
                }
            },
            new ScheduleData
            {
                scheduleId = "schedule_homebody",
                displayName = "居家型日程",
                description = "适合内向居民的日程安排。",
                slots = new ScheduleSlotData[]
                {
                    new ScheduleSlotData { time = "morning", activity = "rest", location = "home", duration = 3f },
                    new ScheduleSlotData { time = "late_morning", activity = "garden", location = "greenhouse", duration = 2f },
                    new ScheduleSlotData { time = "afternoon", activity = "personal_time", location = "home", duration = 3f },
                    new ScheduleSlotData { time = "evening", activity = "cook", location = "kitchen", duration = 2f },
                    new ScheduleSlotData { time = "night", activity = "rest", location = "home", duration = 2f },
                }
            },
            new ScheduleData
            {
                scheduleId = "schedule_worker",
                displayName = "工作型日程",
                description = "适合有条理居民的日程安排。",
                slots = new ScheduleSlotData[]
                {
                    new ScheduleSlotData { time = "morning", activity = "rest", location = "home", duration = 2f },
                    new ScheduleSlotData { time = "late_morning", activity = "craft", location = "workshop", duration = 3f },
                    new ScheduleSlotData { time = "afternoon", activity = "organize", location = "plaza", duration = 3f },
                    new ScheduleSlotData { time = "evening", activity = "social", location = "plaza", duration = 2f },
                    new ScheduleSlotData { time = "night", activity = "rest", location = "home", duration = 2f },
                }
            },
            new ScheduleData
            {
                scheduleId = "schedule_dreamer",
                displayName = "梦幻型日程",
                description = "适合温暖居民的日程安排。",
                slots = new ScheduleSlotData[]
                {
                    new ScheduleSlotData { time = "morning", activity = "rest", location = "home", duration = 3f },
                    new ScheduleSlotData { time = "late_morning", activity = "dream", location = "high_place", duration = 3f },
                    new ScheduleSlotData { time = "afternoon", activity = "garden", location = "greenhouse", duration = 2f },
                    new ScheduleSlotData { time = "evening", activity = "observe", location = "observatory", duration = 2f },
                    new ScheduleSlotData { time = "night", activity = "stargaze", location = "observatory", duration = 2f },
                }
            },
        };
    }

    [System.Serializable]
    public class ScheduleData
    {
        public string scheduleId;
        public string displayName;
        [TextArea(1, 3)] public string description;
        public ScheduleSlotData[] slots;
    }

    [System.Serializable]
    public class ScheduleSlotData
    {
        public string time;
        public string activity;
        public string location;
        public float duration;
    }
}
