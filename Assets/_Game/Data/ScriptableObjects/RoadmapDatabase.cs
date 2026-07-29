using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Roadmap database for the game.
    /// Contains all milestone definitions and progress.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Roadmap Database")]
    public sealed class RoadmapDatabase : ScriptableObject
    {
        [Header("Milestones")]
        public MilestoneData[] milestones = new MilestoneData[]
        {
            new MilestoneData
            {
                milestoneId = "ms_a",
                displayName = "Milestone A — 固化 Observe 基线",
                status = "complete",
                tasks = new[] { "更新审计", "asmdef", "测试", "输入抽象", "ID校验", "幂等性" },
                completedTasks = 6,
                totalTasks = 6
            },
            new MilestoneData
            {
                milestoneId = "ms_b",
                displayName = "Milestone B — 可保存的家园雏形",
                status = "complete",
                tasks = new[] { "Bootstrap场景", "GameBootstrap", "存档DTO", "DiscoveryRepository", "观测台", "场景流" },
                completedTasks = 6,
                totalTasks = 6
            },
            new MilestoneData
            {
                milestoneId = "ms_c",
                displayName = "Milestone C — 两名会生活的星友",
                status = "complete",
                tasks = new[] { "ResidentDefinition", "2名星友", "球面路网", "日程", "自主事件", "玩家建议", "记忆" },
                completedTasks = 7,
                totalTasks = 7
            },
            new MilestoneData
            {
                milestoneId = "ms_d",
                displayName = "Milestone D — 家园愿望连接远征",
                status = "complete",
                tasks = new[] { "愿望模板", "远征定义", "共同回忆卡", "FollowUpSeed", "家园设施" },
                completedTasks = 5,
                totalTasks = 5
            },
            new MilestoneData
            {
                milestoneId = "ms_e",
                displayName = "Milestone E — Restore + 事件型轻生存",
                status = "complete",
                tasks = new[] { "InteractionInstance", "Restore交互", "PressureDefinition", "受寒状态", "临时工具", "检查点" },
                completedTasks = 6,
                totalTasks = 6
            },
            new MilestoneData
            {
                milestoneId = "ms_f",
                displayName = "Milestone F — 固定节点式社区建设",
                status = "complete",
                tasks = new[] { "BuildAnchor", "FacilityDefinition", "预览旋转", "权限接口", "设施使用" },
                completedTasks = 5,
                totalTasks = 5
            },
            new MilestoneData
            {
                milestoneId = "ms_g",
                displayName = "Milestone G — 首个 2 人联机切片",
                status = "complete",
                tasks = new[] { "Netcode评估", "Host/Client同步", "球面Transform", "权威请求", "工具同步", "重连", "好友UI" },
                completedTasks = 7,
                totalTasks = 7
            },
            new MilestoneData
            {
                milestoneId = "ms_h",
                displayName = "Milestone H — Cooperate 与多人故事",
                status = "complete",
                tasks = new[] { "双极共鸣", "分头观察", "救援记录", "居民提及" },
                completedTasks = 4,
                totalTasks = 4
            },
            new MilestoneData
            {
                milestoneId = "ms_i",
                displayName = "Milestone I — 可对外测试 Alpha",
                status = "complete",
                tasks = new[] { "1家园", "1远征", "6-12星友", "3设施", "12事件", "6愿望", "8-10POI", "1压力", "1Restore", "1Cooperate", "2-4人联机" },
                completedTasks = 11,
                totalTasks = 11
            },
        };
    }

    [System.Serializable]
    public class MilestoneData
    {
        public string milestoneId;
        public string displayName;
        public string status; // "planned", "in_progress", "complete"
        public string[] tasks;
        public int completedTasks;
        public int totalTasks;

        public float Progress => totalTasks > 0 ? (float)completedTasks / totalTasks : 0f;
        public bool IsComplete => completedTasks >= totalTasks;
    }
}
