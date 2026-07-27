# Asteria — 内容标签系统

> 状态：Active
> 目标：定义统一的内容标签体系，供事件导演、生成器、社会模拟和 UI 筛选使用。

## 1. 设计原则

- 标签使用 enum + ScriptableObject 列表，首版不构建复杂标签服务器。
- 标签必须覆盖所有可被事件导演查询的内容维度。
- 新增标签类型需要在本文档登记并说明用途。
- 标签值必须稳定，不得在发布后修改含义。

## 2. 标签维度

### 2.1 生态区标签 (BiomeTag)

```csharp
public enum BiomeTag
{
    Wind,       // 风之草原：开阔、风带、滑翔
    Mist,       // 雾声森林：密林、低能见度、声音导航
    Night,      // 星砂夜谷：暗色沙丘、发光路径
    Ice,        // 浮冰潮汐星：冰壳、裂隙、热泉
    Bloom,      // 花粉云庭：巨花、漂浮孢团
    Ruin        // 失落机械星：遗迹、轨道装置
}
```

### 2.2 情绪标签 (MoodTag)

```csharp
public enum MoodTag
{
    Cozy,       // 温暖、安全、放松
    Curious,    // 好奇、探索、发现
    Funny,      // 搞笑、意外、荒诞
    Tense,      // 紧张、危险、紧迫
    Wondrous,   // 惊叹、壮丽、神秘
    Melancholy  // 忧郁、怀念、感伤
}
```

### 2.3 动作标签 (ActionTag)

```csharp
public enum ActionTag
{
    Observe,    // 观察、记录、识别
    Care,       // 安抚、喂养、照料
    Restore,    // 修复、稳定、重建
    Cooperate,  // 同步、搬运、分工
    Traverse,   // 移动、滑翔、导航
    Social      // 互动、对话、赠礼
}
```

### 2.4 压力标签 (PressureTag)

```csharp
public enum PressureTag
{
    None,           // 无压力
    Wind,           // 强风：推移、失衡
    Cold,           // 低温：减速、需要热源
    Dark,           // 黑暗：视野受限、需要光源
    Spores,         // 孢子：视听失真、需要引导
    Instability     // 地表不稳定：裂缝、滑落
}
```

### 2.5 组队规模标签 (GroupSizeTag)

```csharp
public enum GroupSizeTag
{
    Solo,       // 单人可完成
    Duo,        // 双人最佳
    TrioPlus    // 三人以上更有趣
}
```

### 2.6 时间标签 (TimeTag)

```csharp
public enum TimeTag
{
    Dawn,       // 黎明
    Morning,    // 上午
    Noon,       // 正午
    Afternoon,  // 下午
    Dusk,       // 黄昏
    Night,      // 夜晚
    Any         // 任何时间
}
```

### 2.7 记忆标签 (MemoryTag)

```csharp
public enum MemoryTag
{
    Friendship,     // 友谊相关
    Rescue,         // 救援相关
    Discovery,      // 发现相关
    Conflict,       // 冲突相关
    Celebration,    // 庆典相关
    Expedition,     // 远征相关
    Building,       // 建设相关
    Wish            // 愿望相关
}
```

### 2.8 关系标签 (RelationshipTag)

```csharp
public enum RelationshipTag
{
    Acquaintance,   // 熟人
    CloseFriend,    // 密友
    Roommate,       // 室友
    Rival,          // 对手
    Partner,        // 伴侣
    Family,         // 家人
    Mentor          // 导师
}
```

### 2.9 性格标签 (PersonalityTag)

用于事件前置条件匹配：

```csharp
public enum PersonalityTag
{
    Extroverted,    // 外向
    Introverted,    // 内向
    Curious,        // 好奇
    Cautious,       // 谨慎
    Warm,           // 热情
    Reserved,       // 克制
    Organized,      // 有条理
    Spontaneous,    // 随性
    Bold,           // 大胆
    Timid           // 胆小
}
```

## 3. 标签组合规则

### 3.1 事件卡标签要求

每个 `WorldEventDefinition` 必须包含：

- 至少 1 个 biomeTag
- 至少 1 个 moodTag
- 至少 1 个 actionTag
- 恰好 1 个 phase（不是标签，但必须指定）
- 可选：pressureTag、groupSizeTag、timeTag

### 3.2 社会事件标签要求

每个 `SocialEventDefinition` 必须包含：

- 至少 1 个 category（不是标签，但必须指定）
- 所需参与者必须有 personalityTag 匹配
- 可选：memoryTag（前置条件）、facilityTag（位置要求）

### 3.3 POI 标签要求

每个 `PoiDefinition` 必须包含：

- 恰好 1 个 poiType
- 至少 1 个 contentTag
- 可选：biomeTag、pressureTag

## 4. 标签查询示例

### 4.1 事件导演查询

```csharp
// "在风之草原的 Arrival 阶段，找一个有 Observe 动作、情绪为 Curious 的事件"
var candidates = eventDeck
    .Where(e => e.biomeTags.Contains(BiomeTag.Wind))
    .Where(e => e.phase == ExpeditionPhase.Arrival)
    .Where(e => e.actionTags.Contains(ActionTag.Observe))
    .Where(e => e.moodTags.Contains(MoodTag.Curious));
```

### 4.2 社会模拟查询

```csharp
// "找一个需要两名外向居民、在广场发生的日常事件"
var candidates = socialEvents
    .Where(e => e.category == EventCategory.Daily)
    .Where(e => e.minParticipants <= 2 && e.maxParticipants >= 2)
    .Where(e => e.requiredPersonalityTags.Contains(PersonalityTag.Extroverted))
    .Where(e => e.requiredLocationTags.Contains("plaza"));
```

## 5. 标签扩展规则

### 5.1 新增标签值

1. 在对应 enum 中追加值。
2. 在本文档登记说明。
3. 更新所有引用该 enum 的 ScriptableObject 资产。
4. 确保存档兼容（新值在旧存档中不出现，不会导致反序列化失败）。

### 5.2 新增标签维度

1. 在本文档新增维度定义。
2. 创建对应 enum。
3. 更新 `WorldEventDefinition`、`SocialEventDefinition` 或 `PoiDefinition` 以包含新维度。
4. 更新事件导演查询逻辑。
5. 更新测试。

## 6. 首版内容标签预算

### 风之草原标签覆盖

| 内容 | biome | mood | action | pressure | group | time |
|------|-------|------|--------|----------|-------|------|
| 风向初测 | Wind | Curious | Observe | None | Any | Any |
| 失声的风铃石 | Wind | Curious | Observe, Care | None | Duo | Any |
| 风兽迁徙 | Wind | Wondrous | Traverse | None | Any | Dusk |
| 迷路的小旅人 | Wind | Funny | Social | None | Any | Any |
| 风塔叶片散落 | Wind | Tense | Restore | None | Duo | Any |
| 全球强风 | Wind | Tense | Traverse | Wind | Any | Any |
| 双极共鸣 | Wind | Wondrous | Cooperate | None | Duo | Night |
| 留下种子或修复巢穴 | Wind | Melancholy | Care, Restore | None | Any | Any |

## 7. 与事件导演的关系

标签系统是事件导演的查询基础。`CandidateQuery` 使用标签过滤候选池，`Scoring` 使用标签匹配度计算评分。标签不直接控制游戏逻辑，只用于筛选和匹配。
