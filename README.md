# Asteria

> 一款以真实球面世界为舞台的 2–4 人风格化社交探索与轻生存游戏。

Asteria 的目标不是制作另一个传统生存沙盒，而是把三种体验融合成一个清晰的循环：

- **球面探索**：玩家真正生活在微型星球表面，能够绕行极点、走到背面，并感受独特的地平线与昼夜变化。
- **朋友生活模拟**：玩家创建、邀请和认识具有独立性格、关系与日程的“星友”，观察他们自主生活并轻度介入。
- **低压力多人远征**：2–4 名玩家共同面对天气、生态和临时危机，通过观察、修复、协作和照料完成一局远征。

## 核心结构

Asteria 采用“双星球循环”：

1. **家园星球**：长期保存的社区空间。星友在这里居住、交友、争吵、和解、举办活动并提出愿望。
2. **远征星球**：可重复进入的短局球形地图。玩家探索未知生态、处理环境危机、发现故事，并带回种子、记忆、装饰和新的居民线索。

远征让家园发生变化，家园中的人物关系又为下一次远征提供动机。

## 当前可运行切片

当前仓库已经包含：

- Unity 6 + URP 工程
- 真实球面重力与第三人称移动
- 平滑球体、球面散布与地标
- Observe 观察交互
- 简单发现图鉴与 HUD
- 可通过 Editor 菜单 / Unity CLI 重建当前切片

运行方式见 [`Docs/PHASE1_RUN.md`](Docs/PHASE1_RUN.md)。

## 文档入口

所有新开发工作应先阅读 [`Docs/README.md`](Docs/README.md)。

| 文档 | 作用 |
|---|---|
| [`Docs/PRODUCT_VISION_V2.md`](Docs/PRODUCT_VISION_V2.md) | 产品北极星、核心幻想与边界 |
| [`Docs/CORE_GAMEPLAY_AND_SYSTEMS.md`](Docs/CORE_GAMEPLAY_AND_SYSTEMS.md) | 核心循环、轻生存、建造与成长系统 |
| [`Docs/SOCIAL_SIMULATION.md`](Docs/SOCIAL_SIMULATION.md) | 星友、关系、日程、事件与玩家介入方式 |
| [`Docs/WORLD_CONTENT_MATRIX.md`](Docs/WORLD_CONTENT_MATRIX.md) | 家园/远征星球、生态区、事件和内容矩阵 |
| [`Docs/MULTIPLAYER_PERSISTENCE.md`](Docs/MULTIPLAYER_PERSISTENCE.md) | 联机模型、权限、保存与掉线恢复 |
| [`Docs/TECHNICAL_ARCHITECTURE.md`](Docs/TECHNICAL_ARCHITECTURE.md) | Unity 模块、数据边界与代码演进路线 |
| [`Docs/ROADMAP_V2.md`](Docs/ROADMAP_V2.md) | 从当前 Observe 切片到可玩 Alpha 的阶段计划 |
| [`AGENTS.md`](AGENTS.md) | Cursor / Codex / Claude Code 的仓库执行规则 |

## 设计底线

Asteria 不做：

- 方块挖掘和无限自由堆砌
- 持续饥饿、口渴、耐久等高频惩罚表
- PVP、排名、抢资源和装备碾压
- 以日常任务、签到和数值膨胀驱动的 MMO 循环
- 复制其他生活模拟游戏的角色、美术、UI、文本或具体事件

Asteria 要做的是：

> 让朋友们在一颗小星球上共同生活、经历意外、认识有趣的人，并留下会被世界记住的故事。
