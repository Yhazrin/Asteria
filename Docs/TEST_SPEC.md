# Asteria — 测试规范

> 状态：Active
> 目标：定义测试策略、测试类型、验收标准和回归流程，确保每次变更不破坏已有功能。

## 1. 测试原则

- 每次运行时代码变更后必须执行通用回归。
- 每个新系统必须有对应的 EditMode 或 PlayMode 测试。
- 测试必须能在无 Unity Editor 的 CI 环境中运行（EditMode）。
- 不得用"后续再测试"替代当前可执行的最小验证。
- 无法运行 Unity 时，必须明确写"仅静态审查"。

## 2. 测试类型

### 2.1 EditMode 测试

纯逻辑测试，不需要运行游戏。

**适用范围：**

- 球面数学（重力方向、切面投影、大圆距离）
- 性格/关系评分计算
- 事件候选过滤与排序
- 存档序列化/反序列化
- 存档迁移
- 资源上限检查
- 标签匹配逻辑
- 冷却管理
- ID 格式验证

**目录：**

```text
Assets/_Game/Tests/EditMode/
├── SphericalMathTests.cs
├── PersonalityScoringTests.cs
├── RelationshipScoringTests.cs
├── EventCandidateQueryTests.cs
├── SaveSerializationTests.cs
├── SaveMigrationTests.cs
├── ResourceLimitTests.cs
├── TagMatchingTests.cs
├── CooldownRegistryTests.cs
└── IdFormatTests.cs
```

### 2.2 PlayMode 测试

需要运行游戏场景的测试。

**适用范围：**

- 球面移动：穿越极点、背面移动、不穿地
- Observe：单次解锁、不重复计数
- Restore：状态机完成
- 居民：执行日程、不拥堵
- 场景流：家园→远征→返家
- 网络：Host/Client 同步

**目录：**

```text
Assets/_Game/Tests/PlayMode/
├── SphericalMovementTests.cs
├── ObserveInteractionTests.cs
├── RestoreInteractionTests.cs
├── ResidentScheduleTests.cs
├── SceneFlowTests.cs
└── NetworkSyncTests.cs
```

### 2.3 人工回归

无法自动化的验收项。

## 3. 通用回归清单

每次运行时代码变更后必须检查：

| # | 检查项 | 通过条件 |
|---|--------|----------|
| 1 | 编译 | Console 无 Error |
| 2 | 打开场景 | `SphereMoveDemo.unity` 可打开 |
| 3 | 移动 | WASD、Shift、Space、鼠标控制正常 |
| 4 | W 前进 | 不原地自旋 |
| 5 | 极点穿越 | 能经过极点、走到星球背面 |
| 6 | 相机 | 不持续翻转、不穿地 |
| 7 | Observe | 提示和记录正常 |
| 8 | 视觉 | 无粉色材质、Missing Script、丢失引用 |
| 9 | Editor 工具 | 重复运行不制造重复对象 |

## 4. 分系统验收

### 4.1 存档系统

| 测试 | 类型 | 验收条件 |
|------|------|----------|
| 保存后加载 | EditMode | 数据一致 |
| 备份恢复 | EditMode | 损坏时回退到备份 |
| 迁移 v1→v2 | EditMode | 数据完整、新字段有默认值 |
| 原子写入 | EditMode | 写入失败不破坏原文件 |
| schemaVersion 校验 | EditMode | 无效版本抛出异常 |

### 4.2 居民系统

| 测试 | 类型 | 验收条件 |
|------|------|----------|
| 日程执行 | PlayMode | 10 分钟内完成日程并至少互动一次 |
| 极点移动 | PlayMode | 不翻转、不穿入星球 |
| 关系计算 | EditMode | 高亲近+高紧张产生正确状态 |
| 性格漂移 | EditMode | 漂移值在合理范围内 |
| 记忆保存 | PlayMode | 重启后关键记忆保留 |

### 4.3 建设系统

| 测试 | 类型 | 验收条件 |
|------|------|----------|
| 锚点放置 | PlayMode | 只在合法锚点放置 |
| 设施影响 | PlayMode | 建设后居民行为变化 |
| 删除/替换 | PlayMode | 不留下悬空引用 |
| 存档恢复 | EditMode | 设施和居民引用完整 |

### 4.4 轻生存系统

| 测试 | 类型 | 验收条件 |
|------|------|----------|
| 压力预告 | PlayMode | 有清楚预告 |
| 状态恢复 | PlayMode | 不留长期惩罚 |
| 单人可完成 | PlayMode | 单人可完成，多人更流畅 |
| 工具消耗 | EditMode | 能量在合理范围 |

### 4.5 联机系统

| 测试 | 类型 | 验收条件 |
|------|------|----------|
| 单机独立 | PlayMode | 不依赖网络包可正常 Play |
| 主机权威 | PlayMode | 一次交互只由主机结算一次 |
| 跨极点同步 | PlayMode | 远端角色无明显翻转爆跳 |
| 掉线重连 | PlayMode | 30 秒内恢复权威快照 |

## 5. 测试数据管理

### 5.1 测试存档样本

```text
Assets/_Game/Tests/Fixtures/
├── save_v1_basic.json          # 基础存档
├── save_v1_full.json           # 满载存档（12居民、50关系）
├── save_v1_corrupted.json      # 损坏存档
├── save_v1_empty.json          # 空存档
└── save_v1_migration_test.json # 迁移测试用
```

### 5.2 测试 ScriptableObject

```text
Assets/_Game/Tests/Fixtures/Data/
├── TestResident_a.asset
├── TestResident_b.asset
├── TestPersonality_extrovert.asset
├── TestFacility_observatory.asset
└── TestEvent_daily.asset
```

## 6. 性能测试

| 指标 | 目标 | 测试方法 |
|------|------|----------|
| 存档序列化 | < 100ms | EditMode 计时 |
| 存档反序列化 | < 200ms | EditMode 计时 |
| 事件评估 | < 5ms | EditMode 计时 |
| 居民 AI Tick | < 2ms/人 | PlayMode Profiler |
| 球面导航路径计算 | < 1ms | EditMode 计时 |
| 帧率 | 60 FPS | PlayMode Profiler |

## 7. asmdef 配置

### 7.1 测试程序集

```text
Assets/_Game/Tests/EditMode/Asteria.Tests.EditMode.asmdef
  - references: Asteria.Core, Asteria.Planet, Asteria.Interaction, Asteria.Residents, Asteria.Persistence
  - includePlatforms: Editor
  - defineConstraints: UNITY_INCLUDE_TESTS

Assets/_Game/Tests/PlayMode/Asteria.Tests.PlayMode.asmdef
  - references: 所有运行时程序集
  - includePlatforms: Editor
  - defineConstraints: UNITY_INCLUDE_TESTS
```

## 8. CI 集成

### 8.1 本地 CI

```bash
# EditMode 测试
unity -runTests -testPlatform EditMode -testResults Results/editmode.xml

# PlayMode 测试
unity -runTests -testPlatform PlayMode -testResults Results/playmode.xml
```

### 8.2 回归触发

以下变更必须触发完整回归：

- 修改 `PlanetBody`、`SphericalGravityBody`、`SphericalMotor` 或相机脚本
- 修改存档 DTO 或迁移逻辑
- 修改事件导演评分或查询逻辑
- 修改居民 AI 或日程逻辑
- 修改网络同步代码

## 9. 测试覆盖率目标

首版目标：

- 存档系统：100% 路径覆盖
- 事件导演：90% 路径覆盖
- 居民 AI：80% 路径覆盖
- 球面数学：100% 路径覆盖
- UI：不做自动化测试，人工验收
