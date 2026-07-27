# Asteria 文档矩阵

本目录把产品愿景、系统设计、工程架构和开发执行分开维护，避免单份超长提示词同时承担所有职责。

## 1. 资料优先级

发生冲突时，按以下顺序解释：

1. `DECISION_LOG.md` 中最新且状态为 Accepted 的决策
2. `PRODUCT_VISION_V2.md`
3. `CORE_GAMEPLAY_AND_SYSTEMS.md` / `SOCIAL_SIMULATION.md`
4. `MULTIPLAYER_PERSISTENCE.md` / `TECHNICAL_ARCHITECTURE.md`
5. `ROADMAP_V2.md`
6. 旧版 `GAME_DESIGN.md` / `IMPLEMENTATION_PLAN.md`

旧版文档仍用于保存球面移动、Observe、工程安全和“不要推倒重来”等有效原则，但其“完全不做生存/建设”的产品边界已由 V2 方向修订。

## 2. 文档矩阵

| 层级 | 文档 | 回答的问题 | 主要读者 |
|---|---|---|---|
| 产品 | `PRODUCT_VISION_V2.md` | 这款游戏究竟是什么、为什么好玩、绝不做什么 | 全员 |
| 玩法 | `CORE_GAMEPLAY_AND_SYSTEMS.md` | 玩家每分钟、每局、长期分别做什么 | 策划、程序 |
| 社会模拟 | `SOCIAL_SIMULATION.md` | 星友如何产生性格、关系、日程和故事 | 策划、AI/程序 |
| 世界内容 | `WORLD_CONTENT_MATRIX.md` | 星球、生态区、POI、事件如何批量扩展 | 关卡、美术、策划 |
| 联机保存 | `MULTIPLAYER_PERSISTENCE.md` | 多人如何组局、谁有权限、什么需要同步和保存 | 网络、后端、程序 |
| 工程 | `TECHNICAL_ARCHITECTURE.md` | Unity 代码模块、数据边界和技术债如何处理 | 程序、技术美术 |
| 执行 | `ROADMAP_V2.md` | 下一步先做什么、每阶段如何验收 | 制作人、开发者 |
| 决策 | `DECISION_LOG.md` | 为什么这样设计、哪些方向已被否决 | 全员 |
| Agent | 根目录 `AGENTS.md` | AI 编程工具每次任务必须遵守什么 | Cursor/Codex/Claude |

## 3. 当前工程事实

当前 `main` 基线已经具备：

- `PlanetBody`、`SphericalGravityBody`、`SphericalMotor`、手写球面第三人称相机
- `SphereMoveDemo` 可运行场景
- 球面装饰散布、信标与多个 Observe 兴趣点
- `IInteractable`、`InteractionDetector`、`ObserveInteractable`
- `DiscoveryJournal` 的内存记录原型
- `ObserveEntry`、`PlayerMotorConfig`、`TraceLimitsConfig` 等 ScriptableObject
- 简单 HUD 与 Editor Bootstrap/Upgrade 工具

当前尚未具备：

- 持久化存档
- 家园星球与远征星球切换
- 星友居民模拟
- 轻生存事件
- 固定节点式建设
- 正式联机运行时
- 正式角色、美术、动画、音频与 UI

因此，任何任务都应从现有 Observe 切片继续演进，而不是重新创建 Unity 项目或重写球面移动核心。

## 4. 需求变更流程

新增大型系统前必须完成：

1. 在对应设计文档补充目标、非目标和验收。
2. 若改变产品边界，在 `DECISION_LOG.md` 新增 ADR。
3. 在 `ROADMAP_V2.md` 指定所属阶段。
4. 在 `TECHNICAL_ARCHITECTURE.md` 标明模块、数据所有权和网络所有权。
5. 再拆为最小可运行任务。

## 5. 文档状态约定

- **Canonical**：当前权威来源。
- **Active**：正在用于当前阶段。
- **Reference**：保留历史和工程细节。
- **Legacy**：已被新方向部分替代，不应单独作为实现依据。

| 文档 | 状态 |
|---|---|
| `PRODUCT_VISION_V2.md` | Canonical |
| `CORE_GAMEPLAY_AND_SYSTEMS.md` | Canonical |
| `SOCIAL_SIMULATION.md` | Canonical |
| `WORLD_CONTENT_MATRIX.md` | Active |
| `MULTIPLAYER_PERSISTENCE.md` | Active |
| `TECHNICAL_ARCHITECTURE.md` | Active |
| `ROADMAP_V2.md` | Active |
| `GAME_DESIGN.md` | Reference / 部分 Legacy |
| `IMPLEMENTATION_PLAN.md` | Reference / 已由 V2 Roadmap 延伸 |
| `PROJECT_AUDIT.md` | Reference，工程事实需按提交更新 |
| `PHASE1_RUN.md` | Active，当前切片运行手册 |
