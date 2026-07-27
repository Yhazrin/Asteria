# Asteria — 新成员入门指南

> 状态：Active
> 目标：帮助新开发者（包括 AI 工具）快速理解项目结构、设计方向和开发流程。

## 1. 欢迎

欢迎来到 Asteria 项目！

Asteria 是一款以真实球面世界为舞台的 2–4 人风格化社交探索与轻生存游戏。

本文档将帮助你快速了解项目的核心概念、文档结构和开发流程。

## 2. 五分钟速览

### 2.1 这是什么游戏？

- 玩家在微型星球表面生活、探索和社交
- 有自主生活的"星友"居民
- 有短局远征和长期家园
- 有事件型轻生存压力
- 有 2–4 人好友联机

### 2.2 核心循环

```text
家园（长期）：居民生活 → 产生愿望 → 远征动机
远征（短期）：探索 → 观察/修复/合作 → 带回发现
返回家园：展示发现 → 居民反应 → 社区变化
```

### 2.3 技术栈

- Unity 6 (6000.5.5f1)
- URP 17.5.0
- 旧 Input Manager（首版）
- 手写球面物理和相机
- JSON 存档

## 3. 必读文档

按顺序阅读：

| 顺序 | 文档 | 用途 | 阅读时间 |
|------|------|------|----------|
| 1 | `README.md` | 项目总览 | 2 分钟 |
| 2 | `Docs/README.md` | 文档矩阵和优先级 | 3 分钟 |
| 3 | `Docs/PRODUCT_VISION_V2.md` | 产品北极星 | 5 分钟 |
| 4 | `AGENTS.md` | 开发执行规则 | 5 分钟 |
| 5 | 与任务相关的系统文档 | 具体系统设计 | 按需 |

## 4. 项目结构

### 4.1 文档结构

```text
Asteria/
├── README.md                       # 项目总览
├── AGENTS.md                       # AI 工具执行规则
└── Docs/
    ├── README.md                   # 文档矩阵索引
    ├── PRODUCT_VISION_V2.md        # 产品愿景（Canonical）
    ├── CORE_GAMEPLAY_AND_SYSTEMS.md # 核心玩法（Canonical）
    ├── SOCIAL_SIMULATION.md        # 社会模拟（Canonical）
    ├── WORLD_CONTENT_MATRIX.md     # 世界内容（Active）
    ├── MULTIPLAYER_PERSISTENCE.md  # 联机持久化（Active）
    ├── TECHNICAL_ARCHITECTURE.md   # 技术架构（Active）
    ├── ROADMAP_V2.md               # 开发路线图（Active）
    ├── DECISION_LOG.md             # 决策记录
    ├── DATA_CONTRACTS.md           # 数据契约
    ├── SAVE_SCHEMA.md              # 存档架构
    ├── EVENT_DIRECTOR.md           # 事件导演
    ├── CONTENT_TAGGING.md          # 内容标签
    ├── TEST_SPEC.md                # 测试规范
    ├── EDITOR_TOOLING.md           # 编辑器工具
    ├── ART_STYLE_GUIDE.md          # 美术风格
    ├── AUDIO_DESIGN.md             # 音频设计
    ├── GLOSSARY.md                 # 术语表
    ├── ONBOARDING.md               # 本文件
    ├── GAME_DESIGN.md              # 旧版设计（Reference）
    ├── PROJECT_AUDIT.md            # 工程审计（Reference）
    ├── IMPLEMENTATION_PLAN.md      # 旧版计划（Reference）
    └── PHASE1_RUN.md               # 运行手册
```

### 4.2 代码结构

```text
Assets/_Game/
├── Core/           # 启动、事件、通用工具、Settings
├── Planet/         # 星球、区域、地表
├── Player/         # 角色、相机、输入
├── Interaction/    # Observe、Restore、Cooperate
├── Environment/    # 材质、装饰
├── Multiplayer/    # 联机（当前为空）
├── UI/             # 用户界面
├── Data/           # ScriptableObject 配置
├── Audio/          # 音频资产
├── Editor/         # 编辑器工具
└── Tests/          # 测试
```

### 4.3 关键脚本

| 脚本 | 位置 | 作用 |
|------|------|------|
| `PlanetBody.cs` | Planet/Scripts | 星球中心、半径、重力强度 |
| `SphericalGravityBody.cs` | Player/Scripts | 向心重力 |
| `SphericalMotor.cs` | Player/Scripts | 球面移动 |
| `SphericalThirdPersonCamera.cs` | Player/Scripts | 球面相机 |
| `IInteractable.cs` | Interaction/Scripts | 可交互接口 |
| `InteractionDetector.cs` | Interaction/Scripts | 交互检测 |
| `ObserveInteractable.cs` | Interaction/Scripts | Observe 交互 |
| `DiscoveryJournal.cs` | Data/Scripts | 发现图鉴 |
| `Phase1Bootstrap.cs` | Editor | 场景搭建工具 |

## 5. 运行项目

### 5.1 环境要求

- Unity 6000.5.5f1
- Windows 10/11 或 macOS
- 至少 8GB RAM

### 5.2 打开项目

```bash
# 使用 Unity Hub
# 文件 → 打开项目 → 选择 Asteria 文件夹

# 或使用 Unity CLI
unity open /path/to/Asteria
```

### 5.3 运行 Demo

1. 打开 `Assets/_Game/Planet/Scenes/SphereMoveDemo.unity`
2. 按 Play
3. 使用 WASD 移动、鼠标控制视角
4. 走近亮色石头按 E 观察

### 5.4 操作说明

| 输入 | 作用 |
|------|------|
| WASD | 球面移动 |
| Shift | 奔跑 |
| Space | 跳跃 |
| 鼠标 | 视角 |
| E | 观察兴趣点 |
| Esc | 释放鼠标 |
| 左键 | 重新锁定鼠标 |

## 6. 开发流程

### 6.1 任务开始

1. 阅读 `Docs/README.md`
2. 阅读 `Docs/PRODUCT_VISION_V2.md`
3. 阅读与任务相关的系统文档
4. 阅读 `Docs/ROADMAP_V2.md` 了解当前阶段
5. 阅读相关代码和场景

### 6.2 任务输出

开始前输出：

```text
目标:
当前基线:
涉及模块:
预计修改文件:
主要风险:
最小验收:
不做什么:
```

完成后输出：

```text
修改文件:
完成内容:
未完成内容:
验证方式与结果:
已知问题:
架构/产品影响:
回滚方式:
建议下一步:
```

### 6.3 通用回归

每次运行时代码变更后检查：

1. Console 无 Error
2. 打开 `SphereMoveDemo.unity` 可 Play
3. WASD、Shift、Space、鼠标控制正常
4. W 前进不原地自旋
5. 玩家能经过极点、走到星球背面
6. 相机不持续翻转、不穿地
7. Observe 提示和记录正常
8. 无粉色材质、Missing Script、丢失引用

## 7. 绝对禁止

- 新建另一个 Unity 项目替换当前仓库
- 无证据重写核心球面脚本
- 手写大段 Unity YAML
- 手改 .meta GUID
- 创建万能 GameManager.cs
- 擅自安装第三方包
- 把游戏改成体素挖掘、无限建造、持续饥饿口渴、PVP 或数值 MMO
- 复制参考游戏的具体 UI、文本、角色、美术、事件和资源

## 8. 架构原则

### 8.1 代码风格

- 小组件、小服务、接口、事件和 ScriptableObject 优先
- 静态内容定义与运行时状态分离
- 存档使用纯 C# DTO
- 玩法逻辑不直接依赖具体网络 SDK
- UI 只消费 ViewModel/事件
- 所有持久化内容使用稳定字符串 ID

### 8.2 数据流

```text
ScriptableObject (定义) → 运行时状态 (内存) → 存档 DTO (磁盘)
                                          → 网络快照 (网络)
```

### 8.3 服务模式

```text
GameBootstrap (组合根)
├── IGameClock
├── IWorldStateService
├── IDiscoveryRepository
├── IResidentRepository
├── IRelationshipService
├── IEventDirector
├── ISaveService
└── ISessionAuthority
```

## 9. 常见问题

### Q: 可以修改 PlanetBody.cs 吗？

A: 只有在回归测试失败时才能修改。核心球面脚本是项目的基石。

### Q: 可以引入新包吗？

A: 必须在 `Docs/IMPLEMENTATION_PLAN.md` §8 包引入策略中登记并获得确认。

### Q: 可以重命名目录吗？

A: 可以，但必须使用 Unity Editor 或 `git mv`，不得手改 .meta GUID。重命名后必须 Play 回归。

### Q: 存档格式可以修改吗？

A: 可以，但必须递增 schemaVersion 并编写迁移函数。见 `Docs/SAVE_SCHEMA.md`。

### Q: 可以增加新的事件类型吗？

A: 可以，但必须在 `Docs/CONTENT_TAGGING.md` 中登记标签，并在 `Docs/EVENT_DIRECTOR.md` 中更新查询逻辑。

## 10. 路线图概览

当前处于 **Milestone A — 固化 Observe 基线** 阶段。

完整路线图见 `Docs/ROADMAP_V2.md`。

```text
Milestone A  固化当前 Observe 基线        ← 当前
Milestone B  可保存的家园雏形
Milestone C  两名会生活的星友
Milestone D  家园愿望连接远征
Milestone E  Restore + 事件型轻生存
Milestone F  固定节点式社区建设
Milestone G  首个 2 人联机切片
Milestone H  Cooperate 与多人故事
Milestone I  可对外测试 Alpha
```

## 11. 联系方式

- 项目仓库：Yhazrin/Asteria
- 文档问题：在仓库 Issue 中提出
- 代码问题：在 PR 中讨论

## 12. 更新日志

| 日期 | 变更 |
|------|------|
| 2026-07-27 | 初始版本 |
