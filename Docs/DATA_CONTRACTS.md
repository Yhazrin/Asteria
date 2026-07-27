# Asteria — 数据契约

> 状态：Canonical
> 目标：定义所有运行时数据结构、ScriptableObject 定义与纯 C# DTO，确保存档、网络与内容创作使用统一数据层。

## 1. 原则

- 所有持久化内容使用稳定字符串 ID，不使用 Unity InstanceID。
- 静态内容定义用 ScriptableObject，运行时状态用纯 C# 类。
- 存档 DTO 不引用 Transform、GameObject、Material 或任何 UnityEngine.Object。
- Vector3/Quaternion 使用可序列化值对象。
- 所有数据结构必须包含 `schemaVersion` 字段。
- 关系、记忆和事件使用引用 ID，不内嵌完整对象。

## 2. 稳定 ID 规范

```text
格式：{namespace}.{type}.{slug}
示例：
  resident.lian
  facility.observatory
  event.wind_bell_concert
  biome.wind_grassland
  poi.wind_bell_stone_01
  tool.warm_light
  wish.hear_wind_bell
```

- ID 一旦发布不得修改。
- 同一类型内 ID 唯一。
- 存档中只保存 ID，不保存 Unity 路径或 InstanceID。

## 3. 值对象

### 3.1 SerializableVector3

```csharp
[System.Serializable]
public struct SerializableVector3
{
    public float x, y, z;

    public Vector3 ToVector3() => new Vector3(x, y, z);
    public static SerializableVector3 From(Vector3 v) =>
        new SerializableVector3 { x = v.x, y = v.y, z = v.z };
}
```

### 3.2 SerializableQuaternion

```csharp
[System.Serializable]
public struct SerializableQuaternion
{
    public float x, y, z, w;

    public Quaternion ToQuaternion() => new Quaternion(x, y, z, w);
    public static SerializableQuaternion From(Quaternion q) =>
        new SerializableQuaternion { x = q.x, y = q.y, z = q.z, w = q.w };
}
```

## 4. ScriptableObject 定义

### 4.1 ResidentDefinition

```csharp
[CreateAssetMenu(menuName = "Asteria/Resident Definition")]
public class ResidentDefinition : ScriptableObject
{
    [Header("Identity")]
    public string residentId;          // stable ID
    public string displayName;
    public string pronouns;
    public string originDescription;

    [Header("Appearance")]
    public SerializableVector3 bodyColor;  // placeholder for color
    public string voiceProfileId;

    [Header("Personality Preset")]
    public PersonalityPreset personalityPreset;

    [Header("Quirks")]
    public QuirkDefinition[] quirks;   // 2–3 per resident

    [Header("Preferences")]
    public PreferenceDefinition preferences;

    [Header("Default Schedule")]
    public ScheduleTemplate defaultSchedule;
}
```

### 4.2 PersonalityPreset

```csharp
[CreateAssetMenu(menuName = "Asteria/Personality Preset")]
public class PersonalityPreset : ScriptableObject
{
    public string presetId;
    public string displayName;

    [Range(-1f, 1f)] public float sociability;   // 独处 ↔ 合群
    [Range(-1f, 1f)] public float curiosity;     // 稳定 ↔ 探索
    [Range(-1f, 1f)] public float warmth;         // 克制 ↔ 热情
    [Range(-1f, 1f)] public float order;          // 随性 ↔ 计划
    [Range(-1f, 1f)] public float boldness;       // 谨慎 ↔ 冒险
}
```

### 4.3 QuirkDefinition

```csharp
[CreateAssetMenu(menuName = "Asteria/Quirk Definition")]
public class QuirkDefinition : ScriptableObject
{
    public string quirkId;
    public string displayName;
    public string description;
    public string[] triggerTags;        // event tags that activate this quirk
    public string[] behaviorModifiers;  // behavioral changes when active
}
```

### 4.4 PreferenceDefinition

```csharp
[CreateAssetMenu(menuName = "Asteria/Preference Definition")]
public class PreferenceDefinition : ScriptableObject
{
    public string[] likedBiomes;
    public string[] dislikedBiomes;
    public string[] likedWeather;
    public string[] dislikedWeather;
    public string[] likedActivities;
    public string[] giftPreferences;    // item tags
    public string[] creaturePreferences;
}
```

### 4.5 FacilityDefinition

```csharp
[CreateAssetMenu(menuName = "Asteria/Facility Definition")]
public class FacilityDefinition : ScriptableObject
{
    public string facilityId;
    public string displayName;
    public string description;
    public FacilitySize size;           // Large, Medium, Small
    public string[] allowedAnchorTypes;

    [Header("Behavioral Impact")]
    public string[] unlockedScheduleSlots;
    public string[] unlockedEventIds;
    public string[] unlockedWishIds;

    [Header("Visual")]
    public string prefabId;
    public SerializableVector3 previewOffset;
}
```

### 4.6 ToolDefinition

```csharp
[CreateAssetMenu(menuName = "Asteria/Tool Definition")]
public class ToolDefinition : ScriptableObject
{
    public string toolId;
    public string displayName;
    public string description;
    public ToolSlotType slotType;       // Active1, Active2, SharedBeacon
    public float maxEnergy;
    public float rechargeRate;
    public string[] interactionTags;    // what this tool can interact with
}
```

### 4.7 PlanetArchetypeDefinition

```csharp
[CreateAssetMenu(menuName = "Asteria/Planet Archetype")]
public class PlanetArchetypeDefinition : ScriptableObject
{
    public string archetypeId;
    public string displayName;
    public string description;
    public float planetRadius;
    public BiomeDefinition[] biomes;
    public PoiSlotDefinition[] poiSlots;
    public EventDeckEntry[] eventDeck;
    public string[] requiredTools;
}
```

### 4.8 BiomeDefinition

```csharp
[CreateAssetMenu(menuName = "Asteria/Biome Definition")]
public class BiomeDefinition : ScriptableObject
{
    public string biomeId;
    public string displayName;
    public BiomeType biomeType;         // Wind, Mist, Night, Ice, Bloom, Ruin
    public string[] moodTags;
    public string[] pressureTypes;
    public SerializableVector3 ambientColor;
    public string[] decorationSets;
}
```

### 4.9 PoiDefinition

```csharp
[CreateAssetMenu(menuName = "Asteria/POI Definition")]
public class PoiDefinition : ScriptableObject
{
    public string poiId;
    public string displayName;
    public PoiType poiType;             // Observe, Restore, Cooperate, Shelter, Social, Choice, Vista
    public SerializableVector3 localDirection;  // unit vector from planet center
    public string[] requiredTools;
    public string[] contentTags;
    public string linkedEventId;
}
```

### 4.10 SocialEventDefinition

```csharp
[CreateAssetMenu(menuName = "Asteria/Social Event Definition")]
public class SocialEventDefinition : ScriptableObject
{
    public string eventId;
    public string title;
    public string description;
    public EventCategory category;      // Daily, Relationship, Conflict, Community, ExpeditionFollowUp, Surprise

    [Header("Preconditions")]
    public int minParticipants;
    public int maxParticipants;
    public string[] requiredPersonalityTags;
    public string[] requiredRelationshipTags;
    public string[] requiredMemoryTags;
    public string[] requiredLocationTags;
    public string[] requiredWeatherTags;

    [Header("Content")]
    public string openingBeatDescription;
    public PlayerInterventionOption[] playerOptions;
    public AutonomousOutcome[] autonomousOutcomes;

    [Header("Effects")]
    public RelationshipEffect[] relationshipEffects;
    public string[] followUpSeedIds;

    [Header("Constraints")]
    public float cooldownDays;
    public bool isUnique;
}
```

### 4.11 WorldEventDefinition

```csharp
[CreateAssetMenu(menuName = "Asteria/World Event Definition")]
public class WorldEventDefinition : ScriptableObject
{
    public string eventId;
    public string title;
    public string[] biomeTags;
    public string[] moodTags;
    public string[] requiredPoiTypes;
    public int minPlayers;
    public int maxPlayers;
    public string[] requiredResidentTraits;
    public ExpeditionPhase phase;
    public float durationMinSeconds;
    public float durationMaxSeconds;
    public string[] worldStateConditions;
    public EventSetupAction[] setupActions;
    public EventObjective[] runtimeObjectives;
    public EventOutcome successOutcome;
    public EventOutcome partialOutcome;
    public string[] followUpSeeds;
    public float cooldownDays;
}
```

### 4.12 PressureDefinition

```csharp
[CreateAssetMenu(menuName = "Asteria/Pressure Definition")]
public class PressureDefinition : ScriptableObject
{
    public string pressureId;
    public string displayName;
    public PressureType pressureType;   // Wind, Cold, Dark, Spores, Instability
    public float warningDurationSeconds;
    public float activeDurationSeconds;
    public string[] affectedStates;     // player states this triggers
    public string[] counterTools;       // tools that mitigate
    public string recoveryMethod;       // how to recover after
}
```

### 4.13 ExpeditionDefinition

```csharp
[CreateAssetMenu(menuName = "Asteria/Expedition Definition")]
public class ExpeditionDefinition : ScriptableObject
{
    public string expeditionId;
    public string displayName;
    public string planetArchetypeId;
    public string[] availableBiomes;
    public int minPoiCount;
    public int maxPoiCount;
    public string[] eventPhaseSequence;
    public float targetDurationMinutes;
    public string[] rewardCategories;
}
```

### 4.14 ScheduleTemplate

```csharp
[CreateAssetMenu(menuName = "Asteria/Schedule Template")]
public class ScheduleTemplate : ScriptableObject
{
    public string templateId;
    public ScheduleSlot[] slots;

    [System.Serializable]
    public class ScheduleSlot
    {
        public TimeOfDay time;
        public string activityType;     // rest, work, social, explore, solitary
        public string preferredLocationTag;
        public float durationHours;
    }
}
```

## 5. 运行时状态 DTO

### 5.1 ResidentState (存档用)

```csharp
[System.Serializable]
public class ResidentStateDTO
{
    public string residentId;
    public string templateId;
    public PersonalityStateDTO personality;
    public string[] quirks;
    public PreferenceStateDTO preferences;
    public string homeAnchorId;
    public string currentRole;
    public RelationshipEdgeDTO[] relationshipEdges;
    public string[] importantMemoryIds;
    public string[] currentWishIds;
    public NeedStateDTO needs;
    public ScheduleStateDTO currentSchedule;
}

[System.Serializable]
public class PersonalityStateDTO
{
    public float sociability;
    public float curiosity;
    public float warmth;
    public float order;
    public float boldness;
    public float[] driftValues;  // personality drift over time
}

[System.Serializable]
public class NeedStateDTO
{
    public float safety;
    public float social;
    public float solitude;
    public float expression;
    public float exploration;
    public float belonging;
}
```

### 5.2 RelationshipEdgeDTO

```csharp
[System.Serializable]
public class RelationshipEdgeDTO
{
    public string residentIdA;
    public string residentIdB;
    public float familiarity;
    public float affinity;
    public float trust;
    public float admiration;
    public float tension;
    public string[] tags;               // roommate, close_friend, rival, partner, family
    public string[] sharedMemoryIds;
    public float lastMeaningfulInteractionTime;
}
```

### 5.3 MemoryRecordDTO

```csharp
[System.Serializable]
public class MemoryRecordDTO
{
    public string memoryId;
    public string eventId;
    public float timestamp;
    public int worldDay;
    public string[] participantIds;
    public string locationId;
    public string emotionalTone;        // happy, tense, funny, melancholy, wondrous
    public string[] tags;
    public string[] playerChoices;
    public string expeditionId;
    public string photoReference;
    public float importance;            // 0–1, affects decay
    public bool isPermanent;
}
```

### 5.4 HomePlanetStateDTO

```csharp
[System.Serializable]
public class HomePlanetStateDTO
{
    public int planetSeed;
    public int worldDay;
    public string[] unlockedDistricts;
    public BuildAnchorStateDTO[] buildAnchors;
    public EcologyStateDTO ecologyState;
    public string[] activeCommunityEventIds;
    public VisitorPermissionDTO[] visitorPermissions;
}

[System.Serializable]
public class BuildAnchorStateDTO
{
    public string anchorId;
    public AnchorSize size;
    public SerializableVector3 localDirection;
    public string installedFacilityId;  // null if empty
    public float rotationAngle;
    public string colorVariantId;
}
```

### 5.5 ExpeditionCheckpointDTO

```csharp
[System.Serializable]
public class ExpeditionCheckpointDTO
{
    public string expeditionId;
    public string planetArchetypeId;
    public int seed;
    public ExpeditionPhase phase;
    public PlayerStateDTO[] playerStates;
    public PoiStateDTO[] poiStates;
    public PlacedToolDTO[] placedTools;
    public float publicResources;
    public string[] securedDiscoveryIds;
    public EventDirectorStateDTO eventDirectorState;
}

[System.Serializable]
public class PlayerStateDTO
{
    public string playerId;
    public SerializableVector3 position;
    public SerializableQuaternion rotation;
    public float health;
    public float temperature;
    public string[] activeStates;
    public string[] inventoryToolIds;
}

[System.Serializable]
public class PoiStateDTO
{
    public string poiId;
    public PoiInteractionState state;   // Undiscovered, Observed, Restored, Cooperated
    public float progress;
    public string[] contributorIds;
}
```

### 5.6 DiscoveryRecordDTO

```csharp
[System.Serializable]
public class DiscoveryRecordDTO
{
    public string discoveryId;
    public string poiId;
    public string biomeId;
    public string expeditionId;
    public float timestamp;
    public string discovererId;
    public string[] witnessIds;
    public DiscoveryType type;          // Observe, Restore, Cooperate, Creature, Vista
    public string photoReference;
    public string notes;
    public bool isDisplayed;
    public string displayAnchorId;
}
```

## 6. 枚举定义

```csharp
public enum FacilitySize { Large, Medium, Small }
public enum AnchorSize { Large, Medium, Small }
public enum ToolSlotType { Active1, Active2, SharedBeacon }
public enum BiomeType { Wind, Mist, Night, Ice, Bloom, Ruin }
public enum PoiType { Observe, Restore, Cooperate, Shelter, Social, Choice, Vista }
public enum EventCategory { Daily, Relationship, Conflict, Community, ExpeditionFollowUp, Surprise }
public enum ExpeditionPhase { Arrival, Invitation, Complication, Pressure, Resolution, Aftermath }
public enum PressureType { Wind, Cold, Dark, Spores, Instability }
public enum TimeOfDay { Dawn, Morning, Noon, Afternoon, Dusk, Night }
public enum PoiInteractionState { Undiscovered, Observed, Restored, Cooperated }
public enum DiscoveryType { Observe, Restore, Cooperate, Creature, Vista }
public enum RelationshipTag { Roommate, CloseFriend, Rival, Partner, Family, Acquaintance, Mentor }
```

## 7. 数据验证规则

- 所有 ID 非空且符合 `{namespace}.{type}.{slug}` 格式。
- 性格维度值在 `-1..1` 范围内。
- 关系边两端不能是同一居民。
- 记忆至少包含 1 个参与者。
- 设施必须关联至少 1 个行为影响（日程、事件或愿望）。
- POI 方向向量必须是单位向量。
- 存档 schemaVersion 必须大于 0。

## 8. 与 TECHNICAL_ARCHITECTURE.md 的关系

本文档定义数据形状。`TECHNICAL_ARCHITECTURE.md` 定义模块边界与服务接口。运行时服务（如 `IResidentRepository`）消费这些 DTO，但不修改其结构。

存档迁移逻辑在 `SAVE_SCHEMA.md` 中定义。
