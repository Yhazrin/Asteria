# Asteria — 技术架构 V2

> 状态：Active  
> 原则：保留当前球面底座；以模块、数据契约和垂直切片逐步演进。

## 1. 当前基线

当前可直接复用：

| 模块 | 现有实现 |
|---|---|
| 星球 | `PlanetBody`、平滑球体网格、球面散布 |
| 玩家 | `SphericalGravityBody`、`SphericalMotor`、`SphericalThirdPersonCamera` |
| 交互 | `IInteractable`、`InteractionDetector`、`ObserveInteractable` |
| 发现 | `DiscoveryJournal`、`ObserveEntry` |
| 配置 | `PlayerMotorConfig`、`TraceLimitsConfig` |
| UI | 当前 `GameHud` / Demo HUD |
| 工具 | `Phase1Bootstrap`、Observe/Planet Dressing Upgrade |
| 场景 | `SphereMoveDemo.unity` |

主要技术债：

- `DiscoveryJournal` 仅内存保存
- 部分全局访问依赖静态单例
- 无 asmdef / 自有测试
- 旧 Input Manager 与 `OnGUI` 仍是 Demo 方案
- 无场景流、存档、居民 AI、事件导演和运行时联机
- 文档中的 Phase 状态需要跟随代码持续更新

## 2. 目标模块

建议保留 `Assets/_Game`，新增而非大搬家：

```text
Assets/_Game
├── Core/
│   ├── Bootstrap/
│   ├── Events/
│   ├── Time/
│   └── Settings/
├── Planet/
│   ├── Runtime/
│   ├── Generation/
│   ├── Navigation/
│   └── Scenes/
├── Player/
│   ├── Runtime/
│   ├── Input/
│   └── Presentation/
├── Interaction/
│   ├── Observe/
│   ├── Care/
│   ├── Restore/
│   └── Cooperate/
├── Residents/
│   ├── Runtime/
│   ├── Scheduling/
│   ├── Relationships/
│   └── Presentation/
├── Events/
│   ├── World/
│   ├── Social/
│   └── Director/
├── Building/
│   ├── Anchors/
│   └── Facilities/
├── Expedition/
│   ├── Flow/
│   ├── Pressure/
│   └── Results/
├── Persistence/
│   ├── SaveData/
│   ├── Migration/
│   └── Repositories/
├── Multiplayer/
│   ├── Runtime/
│   ├── Replication/
│   └── Lobby/
├── Data/
├── UI/
├── Audio/
├── Editor/
└── Tests/
```

暂不强制重命名 `Planet`、`Multiplayer`、`Environment`，避免 Unity GUID 和引用风险。

## 3. 程序集边界

逐步增加 asmdef，避免一次性拆分全部脚本。

建议顺序：

1. `Asteria.Core`
2. `Asteria.Planet`
3. `Asteria.Interaction`
4. `Asteria.Residents`
5. `Asteria.Persistence`
6. `Asteria.Multiplayer`
7. `Asteria.Editor`
8. 对应 Tests

依赖方向：

```text
Core
├── Planet
├── Data
├── Persistence
└── Presentation contracts

Player → Core, Planet, Interaction
Residents → Core, Planet, Data
Events → Core, Residents, Interaction
Building → Core, Planet, Residents
Expedition → Core, Planet, Events, Interaction
Multiplayer → Core contracts + runtime adapters
UI → read-only presentation models / events
Editor → all runtime assemblies (Editor only)
```

禁止 Runtime 反向依赖 Editor；禁止 Residents 直接调用具体网络 SDK。

## 4. 服务与数据所有权

避免新的万能 `GameManager`。使用小型服务和场景组合根。

### 核心服务接口

```csharp
public interface IGameClock { }
public interface IWorldStateService { }
public interface IDiscoveryRepository { }
public interface IResidentRepository { }
public interface IRelationshipService { }
public interface IEventDirector { }
public interface ISaveService { }
public interface ISessionAuthority { }
```

服务通过 `GameBootstrap` 或场景 `CompositionRoot` 组装。首版可以手动序列化引用，不必引入依赖注入框架。

### ScriptableObject 负责

- 静态内容定义
- 平衡参数
- 标签与资源引用
- Editor 创作入口

### 运行时状态类负责

- 当前数值和进度
- 存档序列化
- 网络快照
- 临时缓存

不要把会变化的玩家存档直接写回 ScriptableObject。

## 5. 内容定义

建议新增：

```text
ResidentDefinition
PersonalityPreset
QuirkDefinition
PreferenceDefinition
SocialEventDefinition
WorldEventDefinition
PlanetArchetypeDefinition
BiomeDefinition
PoiDefinition
FacilityDefinition
ToolDefinition
PressureDefinition
ExpeditionDefinition
```

所有定义需有稳定字符串 ID，存档只保存 ID 与运行时差异，不保存 Unity InstanceID。

## 6. 星友运行时架构

```text
ResidentAgent
├── ResidentIdentity
├── PersonalityState
├── NeedState
├── RelationshipMemory
├── ScheduleController
├── ResidentNavigator
├── InteractionParticipant
└── ResidentPresentation
```

### 更新频率

- 视野内居民：正常 Animator/Nav 更新
- 同星球远距离居民：低频逻辑 Tick
- 不在当前场景居民：按事件/时间片模拟，不运行 GameObject

不要为每个离线居民保持完整 MonoBehaviour 和 NavMeshAgent。

### 球面导航

首版可采用：

- 预定义球面路网节点
- 节点存单位方向向量
- 边权重为大圆距离
- 到达局部区域后使用切平面移动

在需要前不要直接尝试让 Unity NavMesh 覆盖完整闭合球体。

## 7. 事件导演

事件导演由纯数据条件 + 可测试执行器组成。

```text
EventDirector
- CandidateQuery
- Scoring
- ConflictResolver
- EventInstanceFactory
- CooldownRegistry
- FollowUpQueue
```

评分考虑：

- 当前阶段
- 地点和天气
- 参与者关系/性格
- 最近已发生事件
- 玩家停留时间
- 内容重复度
- 性能预算

导演只决定“哪张已定义事件卡开始”，不生成任意不可控代码或自由文本逻辑。

## 8. 交互系统演进

保留 `IInteractable`，逐步扩展上下文：

```csharp
public readonly struct InteractionContext
{
    public Transform Actor { get; }
    public string ActorId { get; }
    public string SessionId { get; }
    public bool HasAuthority { get; }
}
```

增加：

- 交互类型标签
- 预计持续时间
- 工具要求
- 多人参与槽位
- 权威验证
- 可取消与完成事件

长时 Restore/Cooperate 不应只使用一次 `Interact()`；应有 `InteractionInstance` 状态机。

## 9. 场景流

建议场景：

```text
Bootstrap
HomePlanet
ExpeditionPlanet
Frontend（后期）
```

`Bootstrap` 常驻：

- 配置加载
- 存档
- 会话
- 音频
- 全局 UI 层

家园与远征场景按需加载。不要把所有玩法永久放在一个 `SphereMoveDemo` 场景中。

## 10. 持久化边界

使用纯 C# DTO：

- 无 `Transform`、`GameObject`、`Material`
- Vector3/Quaternion 使用可序列化值对象
- 所有版本具备 `schemaVersion`
- 写入前生成快照，避免边运行边序列化可变集合
- 迁移测试覆盖至少前一个版本

`DiscoveryJournal` 应改为实现 `IDiscoveryRepository` 的运行时适配器，并由存档恢复。

## 11. 网络适配层

玩法层不直接依赖 NGO 类型：

```text
Gameplay Request
→ ISessionAuthority
→ LocalAuthorityAdapter 或 NetworkAuthorityAdapter
→ Domain Result
→ Event / Snapshot
```

因此单机、本地主机和未来 Dedicated Server 可复用同一领域逻辑。

## 12. 输入与 UI

### 输入

- 当前旧 Input Manager 可继续支持下一个小切片。
- 在正式联机前抽出 `IPlayerInputSource`。
- 迁移 Input System 时保留同一输入接口。

### UI

短期：当前 HUD 可继续回归验证。  
中期：采用 UGUI/TMP 或 UI Toolkit 二选一，不双线铺开。

UI 只显示 ViewModel，不直接修改居民关系、事件阶段或存档。

## 13. 测试矩阵

### EditMode

- 球面 up/重力方向
- 大圆距离与路径
- 性格/关系评分
- 事件候选过滤
- 存档迁移
- 资源上限

### PlayMode

- 穿越极点
- Observe 解锁一次
- Restore 状态机
- 居民执行日程
- 家园→远征→返家流程
- 掉线重连快照

### 人工回归

- W 不自旋
- 地平线/相机稳定
- 无 Missing Script / 粉色材质
- 10 分钟居民不拥堵/不抖动
- Host/Client 跨极点不翻转

## 14. 性能预算

首版目标以桌面端 60 FPS 为基准：

- 活跃居民 12 名以内
- 远征生物 20–30 个轻量实体
- 球面散布使用静态批处理/实例化，避免每个装饰独立 Update
- AI 逻辑分帧
- 远距离居民降频或抽象模拟
- 网络高频同步只给玩家和少量关键实体
- 事件导演每 0.5–2 秒评估一次，不逐帧扫描

## 15. 近期最小架构改动

在不碰联机包的前提下，下一步只做：

1. 新增稳定 ID 和存档 DTO。
2. 把 `DiscoveryJournal` 接入可保存 Repository。
3. 增加 `GameBootstrap` 组合根。
4. 建立 `HomePlanet` 空白切片场景。
5. 创建 2 名数据驱动居民和球面路网原型。
6. 用一个愿望事件连接家园与现有 Observe 星球。

这组改动完成前，不开始完整天气、建设、关系图或网络 SDK 集成。
