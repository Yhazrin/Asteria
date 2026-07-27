# Asteria — 事件导演系统

> 状态：Active
> 目标：定义远征事件导演与家园社会事件的调度逻辑，确保事件可预测、可测试且产生有意义的玩家体验。

## 1. 设计原则

- 导演只组合预制事件模块，不在运行时生成不可测试的任意剧情。
- 事件评分基于数据条件，不依赖随机黑箱。
- 每个事件必须有明确的前置条件、触发阶段和后续种子。
- 同一事件在冷却期内不得重复触发。
- 导演评估频率为 0.5–2 秒，不逐帧扫描。

## 2. 远征事件导演

### 2.1 架构

```text
EventDirector
├── CandidateQuery         # 从事件池中筛选当前可用事件
├── Scoring                # 对候选事件评分
├── ConflictResolver       # 解决同阶段多个高分事件的冲突
├── EventInstanceFactory   # 创建事件实例
├── CooldownRegistry       # 管理事件冷却
└── FollowUpQueue          # 管理后续事件种子
```

### 2.2 远征阶段

```csharp
public enum ExpeditionPhase
{
    Arrival,        // 天气温和，建立方向感
    Invitation,     // 出现多个可选目标
    Complication,   // 生态或人物行为制造意外
    Pressure,       // 天气/环境进入生存窗口
    Resolution,     // 完成修复、救援或主动撤离
    Aftermath       // 生成远征摘要与家园后续种子
}
```

### 2.3 阶段转换规则

| 当前阶段 | 转换条件 | 下一阶段 |
|----------|----------|----------|
| Arrival | 玩家完成首次 Observe 或 3 分钟已过 | Invitation |
| Invitation | 完成 2+ 个 POI 交互或 8 分钟已过 | Complication |
| Complication | 完成关键事件或 15 分钟已过 | Pressure |
| Pressure | 生存窗口结束或完成 Resolution 目标 | Resolution |
| Resolution | 所有玩家到达撤离点或主动撤离 | Aftermath |

阶段转换由 `PhaseTransitionEvaluator` 每 2 秒评估一次。

### 2.4 候选查询

```csharp
public class CandidateQuery
{
    public List<WorldEventDefinition> Query(
        ExpeditionPhase phase,
        string[] activeBiomeTags,
        string[] activeMoodTags,
        int playerCount,
        string[] availablePoiTypes,
        string[] residentTraitTags,
        HashSet<string> cooldownRegistry)
    {
        // 1. 按阶段过滤
        // 2. 按生物群系标签匹配
        // 3. 按玩家数量过滤
        // 4. 按 POI 类型过滤
        // 5. 按居民特性过滤
        // 6. 排除冷却中的事件
        // 7. 排除已触发的唯一事件
    }
}
```

### 2.5 评分函数

```csharp
public class EventScorer
{
    public float Score(WorldEventDefinition candidate, ScoringContext ctx)
    {
        float score = 0;

        // 阶段匹配度 (0–30)
        score += PhaseMatchScore(candidate.phase, ctx.currentPhase);

        // 情绪曲线平衡 (0–20)
        score += MoodBalanceScore(candidate.moodTags, ctx.recentMoods);

        // 玩家停留时间 (0–15)
        score += DwellTimeScore(ctx.timeSinceLastEvent);

        // 内容多样性 (0–15)
        score += DiversityScore(candidate.eventId, ctx.recentEventIds);

        // POI 可达性 (0–10)
        score += AccessibilityScore(candidate.requiredPoiTypes, ctx.availablePois);

        // 前置条件满足度 (0–10)
        score += PreconditionScore(candidate, ctx);

        return score;
    }
}
```

### 2.6 冲突解决

当同阶段多个事件评分接近（差距 < 10 分）时：

1. 优先选择与最近事件类型不同的事件。
2. 优先选择涉及更多玩家的事件。
3. 优先选择有后续种子的事件。
4. 仍然冲突时按事件 ID 字典序选择（确定性）。

### 2.7 事件实例化

```csharp
public class ExpeditionEventInstance
{
    public string instanceId;
    public WorldEventDefinition definition;
    public ExpeditionPhase triggerPhase;
    public float startTime;
    public EventState state;

    // 运行时数据
    public string[] participantIds;
    public PoiStateDTO[] involvedPois;
    public float progress;
    public string chosenOutcomeId;
}

public enum EventState
{
    Setup,          // 正在初始化
    Active,         // 正在进行
    Completing,     // 正在结算
    Completed,      // 已完成
    Failed,         // 已失败
    Expired         // 超时
}
```

## 3. 家园社会事件导演

### 3.1 架构

```text
SocialEventDirector
├── SocialCandidateQuery
├── SocialScoring
├── SocialScheduler
└── SocialEventInstance
```

### 3.2 触发时机

家园社会事件在以下时机评估：

- 游戏内每日开始（世界时间 06:00）。
- 居民完成日程活动后。
- 玩家完成远征返回家园时。
- 新设施建成后。
- 新居民加入后。

### 3.3 候选查询

```csharp
public class SocialCandidateQuery
{
    public List<SocialEventDefinition> Query(
        ResidentStateDTO[] residents,
        RelationshipEdgeDTO[] relationships,
        MemoryRecordDTO[] recentMemories,
        string[] availableFacilities,
        TimeOfDay currentTime,
        string currentWeather,
        HashSet<string> cooldownRegistry)
    {
        // 1. 按参与者数量过滤
        // 2. 按性格前置条件过滤
        // 3. 按关系前置条件过滤
        // 4. 按记忆标签过滤
        // 5. 按位置/设施过滤
        // 6. 按时间/天气过滤
        // 7. 排除冷却中的事件
    }
}
```

### 3.4 评分函数

```csharp
public class SocialEventScorer
{
    public float Score(SocialEventDefinition candidate, SocialScoringContext ctx)
    {
        float score = 0;

        // 关系状态匹配度 (0–25)
        score += RelationshipMatchScore(candidate, ctx.relationships);

        // 性格组合有趣度 (0–20)
        score += PersonalityComboScore(candidate, ctx.residents);

        // 最近记忆相关性 (0–20)
        score += MemoryRelevanceScore(candidate, ctx.recentMemories);

        // 内容多样性 (0–15)
        score += DiversityScore(candidate.eventId, ctx.recentEventIds);

        // 设施可用性 (0–10)
        score += FacilityAvailabilityScore(candidate, ctx.availableFacilities);

        // 时间适宜性 (0–10)
        score += TimeAppropriatenessScore(candidate, ctx.currentTime);

        return score;
    }
}
```

## 4. 后续种子系统

### 4.1 种子结构

```csharp
[System.Serializable]
public class FollowUpSeed
{
    public string seedId;
    public string sourceEventId;
    public string targetSystem;     // "social", "expedition", "wish", "facility"
    public float delayDays;         // 最少等待天数
    public float priority;
    public string[] conditions;     // 额外触发条件
    public string templateId;       // 要使用的事件/愿望模板
}
```

### 4.2 种子队列

```csharp
public class FollowUpQueue
{
    private List<FollowUpSeed> _pending = new();

    public void Enqueue(FollowUpSeed seed) => _pending.Add(seed);

    public List<FollowUpSeed> Harvest(float currentDay, string[] activeConditions)
    {
        var ready = _pending
            .Where(s => currentDay >= s.delayDays
                     && ConditionsMet(s.conditions, activeConditions))
            .OrderByDescending(s => s.priority)
            .ToList();

        foreach (var seed in ready)
            _pending.Remove(seed);

        return ready;
    }
}
```

## 5. 冷却管理

### 5.1 冷却规则

| 事件类型 | 默认冷却 | 说明 |
|----------|----------|------|
| 日常趣事 | 0.5 天 | 频繁触发保持社区活力 |
| 关系升温 | 2 天 | 避免速刷关系 |
| 轻冲突 | 3 天 | 避免持续紧张 |
| 社区事件 | 5 天 | 特殊感 |
| 远征后续 | 7 天 | 留出消化时间 |
| 唯一事件 | ∞ | 触发一次后永久移除 |

### 5.2 冷却注册

```csharp
public class CooldownRegistry
{
    private Dictionary<string, float> _cooldowns = new();

    public void Register(string eventId, float cooldownDays, float currentDay)
    {
        _cooldowns[eventId] = currentDay + cooldownDays;
    }

    public bool IsOnCooldown(string eventId, float currentDay)
    {
        return _cooldowns.TryGetValue(eventId, out var readyDay)
            && currentDay < readyDay;
    }
}
```

## 6. 测试策略

### 6.1 单元测试

- CandidateQuery：给定条件返回正确候选集。
- Scoring：给定上下文返回预期排序。
- ConflictResolver：同分事件选择确定性。
- CooldownRegistry：冷却注册与查询。
- FollowUpQueue：种子入队、出队与条件过滤。
- PhaseTransitionEvaluator：阶段转换条件。

### 6.2 集成测试

- 完整远征流程：Arrival → Aftermath 生成合理事件序列。
- 家园一天：居民完成日程并触发至少 1 个社会事件。
- 远征后续：完成远征后家园出现后续事件种子。

### 6.3 人工验收

- 10 分钟远征中事件节奏感合理（不连续触发同类事件）。
- 家园 10 分钟观察中至少出现 2 次自主互动。
- 同一事件在不同性格组合下有不同表达。

## 7. 性能预算

- 事件评估间隔：0.5–2 秒（不逐帧）。
- 单次评估耗时：< 5ms。
- 候选池大小：远征 30–50 个事件定义，家园 50–100 个事件定义。
- 活跃事件实例：远征最多 3 个并行，家园最多 2 个并行。
