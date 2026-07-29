using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Decision log database for the game.
    /// Contains all ADR (Architecture Decision Records).
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Decision Log Database")]
    public sealed class DecisionLogDatabase : ScriptableObject
    {
        [Header("Decisions")]
        public DecisionRecord[] decisions = new DecisionRecord[]
        {
            new DecisionRecord
            {
                adrId = "ADR-001",
                title = "保留真实球面作为核心空间",
                date = "2026-07-26",
                status = "Accepted",
                context = "球面移动、地平线、极点、背面、全球天气和远距离协作是 Asteria 的核心差异化。",
                decision = "玩家真实生活在闭合球面，重力指向星球中心；不使用平面地图加球形视觉包装。",
                consequences = "所有导航、建造、网络同步和POI设计必须考虑局部切面与星球中心。"
            },
            new DecisionRecord
            {
                adrId = "ADR-002",
                title = "不推倒当前球面移动与 Observe 基线",
                date = "2026-07-26",
                status = "Accepted",
                context = "当前底座已经解决最关键的运动和相机风险。大重构会引入 Unity GUID、场景引用和极点回归风险。",
                decision = "保留 PlanetBody、球面重力、电机、手写相机、现有场景、Observe 和 Editor Upgrade 工具。",
                consequences = "只有明确的回归测试失败才能触发核心运动学重写。"
            },
            new DecisionRecord
            {
                adrId = "ADR-003",
                title = "产品升级为双星球循环",
                date = "2026-07-27",
                status = "Accepted",
                context = "单一世界很难同时满足低压力生活模拟与有节奏的多人危机。",
                decision = "Asteria 采用长期保存的家园星球和20-40分钟的远征星球。",
                consequences = "PRODUCT_VISION_V2.md 成为产品北极星。"
            },
            new DecisionRecord
            {
                adrId = "ADR-004",
                title = "生存是事件，不是日常维护",
                date = "2026-07-27",
                status = "Accepted",
                context = "Asteria 的目标玩家偏好合作、探索和人物故事。持续维护条会把注意力从球面空间和社交故事中夺走。",
                decision = "生存压力只在天气、生态和远征阶段中临时出现。禁止持续饥饿、口渴、装备耐久、死亡掉落和离线惩罚。",
                consequences = "允许光、庇护和工具能量等少量会话资源；危机结束后状态应快速恢复。"
            },
            new DecisionRecord
            {
                adrId = "ADR-005",
                title = "建设采用固定锚点与功能设施",
                date = "2026-07-27",
                status = "Accepted",
                context = "无限自由建造会导致球面碰撞、网络同步、存档和内容表达复杂度失控。",
                decision = "家园使用有限 BuildAnchor 与模块化设施。设施必须改变居民日程、事件或社区功能。",
                consequences = "不做体素挖掘、任意墙体、工业流水线和无限物体堆叠。"
            },
            new DecisionRecord
            {
                adrId = "ADR-006",
                title = "星友使用自主社会模拟，不做任务板NPC",
                date = "2026-07-27",
                status = "Accepted",
                context = "角色关系与意外故事是生活模拟的核心留存来源。",
                decision = "居民拥有性格、偏好、关系边、日程和记忆，并能自主触发数据驱动事件。",
                consequences = "首版先做 2 名居民和 1 个完整事件，不提前追求大语言模型自由对话。"
            },
            new DecisionRecord
            {
                adrId = "ADR-007",
                title = "首版采用房主权威 2-4 人联机",
                date = "2026-07-27",
                status = "Accepted",
                context = "目标是小规模好友协作，不需要大型常驻服务器。",
                decision = "单机优先，好友房间采用房主权威。房主家园保存公共状态，访客保存个人发现与回忆。",
                consequences = "在存档、领域状态与权限接口完成前，不接入运行时 Netcode。"
            },
            new DecisionRecord
            {
                adrId = "ADR-008",
                title = "先预制可控内容，再做有限组合生成",
                date = "2026-07-27",
                status = "Accepted",
                context = "球面移动、路径、多人事件和相机都需要稳定测试。完全生成会放大不可复现问题。",
                decision = "远征星球采用人工验证的球体拓扑、POI 槽位、事件卡和数据化组合。",
                consequences = "程序化主要用于选择、散布、种子和阶段组合；关键地形与事件仍需人工验收。"
            },
            new DecisionRecord
            {
                adrId = "ADR-009",
                title = "不直接复制生活模拟参考作品",
                date = "2026-07-27",
                status = "Accepted",
                context = "Asteria 必须形成自己的产品身份，也要避免知识产权和体验同质化风险。",
                decision = "只吸收高层设计思想，不复制现有作品的角色表现、UI、文本、事件、建筑和创作系统。",
                consequences = "所有新内容必须至少体现球面空间、远征后续、多人记忆或生态恢复中的一项 Asteria 特征。"
            },
        };
    }

    [System.Serializable]
    public class DecisionRecord
    {
        public string adrId;
        public string title;
        public string date;
        public string status;
        [TextArea(2, 4)] public string context;
        [TextArea(2, 4)] public string decision;
        [TextArea(2, 4)] public string consequences;
    }
}
