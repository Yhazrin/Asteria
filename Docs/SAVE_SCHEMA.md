# Asteria — 存档架构

> 状态：Active
> 目标：定义存档格式、保存策略、迁移流程与数据安全，确保玩家进度可持久化且可升级。

## 1. 设计原则

- 存档使用纯 C# DTO，不序列化 Unity Scene/Prefab/Material。
- 所有写入先写临时文件，原子替换正式文件。
- 每次保存生成备份，最多保留 3 份。
- schemaVersion 必须存在，每次格式变更递增。
- 存档损坏时回退到最近有效备份，不覆盖好档。
- 迁移函数必须覆盖前一个版本，建议覆盖前两个版本。

## 2. 存档目录结构

```text
{persistentDataPath}/Saves/
├── slot_0/
│   ├── save.json           # 主存档
│   ├── save.json.bak       # 上次备份
│   ├── save.json.bak2      # 上上次备份
│   ├── discoveries.json    # 发现记录（可分离）
│   ├── memories.json       # 记忆记录（可分离）
│   └── metadata.json       # 存档元信息
├── slot_1/
│   └── ...
└── settings.json           # 全局设置（不在存档槽内）
```

## 3. 存档根结构

```csharp
[System.Serializable]
public class SaveRoot
{
    // === 版本 ===
    public int schemaVersion;           // 当前版本：1
    public string saveTimestamp;        // ISO 8601
    public string gameVersion;          // Application.version

    // === 玩家档案 ===
    public string profileId;
    public string playerName;
    public SerializableVector3 homePosition;

    // === 家园 ===
    public HomePlanetStateDTO homePlanet;

    // === 居民 ===
    public ResidentStateDTO[] residents;

    // === 关系 ===
    public RelationshipEdgeDTO[] relationships;

    // === 记忆 ===
    public string[] memoryIds;          // 详细记录在 memories.json

    // === 发现 ===
    public string[] discoveryIds;       // 详细记录在 discoveries.json

    // === 设施 ===
    public FacilityStateDTO[] facilities;

    // === 背包/物品 ===
    public InventorySlotDTO[] inventory;

    // === 远征历史 ===
    public ExpeditionHistoryEntry[] expeditionHistory;

    // === 玩家设置 ===
    public PlayerSettingsDTO playerSettings;

    // === 内容版本 ===
    public string contentVersion;       // 用于检测 DLC/更新
}
```

## 4. 保存策略

### 4.1 家园保存

| 触发时机 | 保存内容 | 类型 |
|----------|----------|------|
| 居民关系变化 | relationships, residents | 增量 |
| 设施建造/拆除 | homePlanet, facilities | 增量 |
| 新星友加入 | residents, relationships | 增量 |
| 愿望完成 | residents | 增量 |
| 玩家主动保存 | 全量 | 全量 |
| 退出游戏 | 全量 | 全量 |

### 4.2 远征保存

| 触发时机 | 保存内容 | 类型 |
|----------|----------|------|
| 阶段切换 | expedition checkpoint | 检查点 |
| 关键 Restore 完成 | expedition checkpoint | 检查点 |
| 主动撤离 | expedition + home | 结算 |
| 会话结束 | expedition + home | 结算 |
| 掉线重连 | 读取最近检查点 | 恢复 |

### 4.3 保存流程

```text
1. 创建 SaveRoot 快照（复制当前状态，不引用运行时对象）
2. 序列化为 JSON
3. 写入 {slot}/save.json.tmp
4. 如果写入成功：
   a. 删除 save.json.bak2
   b. 重命名 save.json.bak → save.json.bak2
   c. 重命名 save.json → save.json.bak
   d. 重命名 save.json.tmp → save.json
5. 如果写入失败：
   a. 删除 save.json.tmp
   b. 保留原 save.json 不变
```

## 5. 加载流程

```text
1. 检查 save.json 是否存在
2. 读取并反序列化
3. 验证 schemaVersion
4. 如果 schemaVersion < 当前版本：
   a. 按顺序执行迁移函数
   b. 每步迁移后验证数据完整性
   c. 迁移成功后保存新版本
5. 如果反序列化失败：
   a. 尝试 save.json.bak
   b. 再尝试 save.json.bak2
   c. 全部失败则提示玩家并提供新建存档选项
6. 验证关键引用完整性（residentId、facilityId 等）
7. 注入运行时状态
```

## 6. Schema 迁移

### 6.1 迁移函数签名

```csharp
public interface ISaveMigration
{
    int FromVersion { get; }
    int ToVersion { get; }
    string Description { get; }
    SaveRoot Migrate(SaveRoot oldSave);
}
```

### 6.2 迁移注册

```csharp
public class SaveMigrationRegistry
{
    private readonly Dictionary<int, ISaveMigration> _migrations;

    public SaveRoot MigrateToLatest(SaveRoot save, int targetVersion)
    {
        while (save.schemaVersion < targetVersion)
        {
            if (!_migrations.TryGetValue(save.schemaVersion, out var migration))
                throw new SaveMigrationException(
                    $"No migration from v{save.schemaVersion}");

            save = migration.Migrate(save);
            save.schemaVersion = migration.ToVersion;
        }
        return save;
    }
}
```

### 6.3 迁移测试要求

- 每个迁移函数必须有对应的单元测试。
- 测试必须覆盖：正常迁移、空字段处理、新增字段默认值、删除字段兼容。
- 至少保留前两个版本的测试存档样本。

## 7. 数据完整性验证

### 7.1 保存前验证

```csharp
public class SaveValidator
{
    public ValidationResult Validate(SaveRoot save)
    {
        var errors = new List<string>();

        // 基础验证
        if (save.schemaVersion <= 0)
            errors.Add("schemaVersion must be > 0");
        if (string.IsNullOrEmpty(save.profileId))
            errors.Add("profileId is required");

        // 引用完整性
        var residentIds = save.residents.Select(r => r.residentId).ToHashSet();
        foreach (var rel in save.relationships)
        {
            if (!residentIds.Contains(rel.residentIdA))
                errors.Add($"Relationship references unknown resident: {rel.residentIdA}");
            if (!residentIds.Contains(rel.residentIdB))
                errors.Add($"Relationship references unknown resident: {rel.residentIdB}");
        }

        // 值范围
        foreach (var resident in save.residents)
        {
            if (resident.personality != null)
            {
                if (resident.personality.sociability < -1 || resident.personality.sociability > 1)
                    errors.Add($"Resident {resident.residentId} sociability out of range");
            }
        }

        return new ValidationResult(errors);
    }
}
```

### 7.2 加载后验证

- 所有 residentId 在定义表中存在。
- 所有 facilityId 在定义表中存在。
- 关系边两端居民都存在。
- 记忆引用的事件 ID 有效。
- 设施锚点 ID 在家园锚点列表中。

## 8. 存档安全

### 8.1 备份策略

- 每次保存保留最多 3 份备份（save.json, save.json.bak, save.json.bak2）。
- 迁移前额外创建迁移备份：`save_v{oldVersion}_{timestamp}.json`。
- 备份文件在成功保存新版本后可清理，但建议保留至少 1 份迁移备份。

### 8.2 损坏恢复

```text
优先级：
1. save.json（最新）
2. save.json.bak（上次）
3. save.json.bak2（上上次）
4. 迁移备份（最旧的有效版本）

全部失败：
- 提示玩家存档损坏
- 提供"新建存档"和"从备份恢复"选项
- 不自动覆盖任何文件
```

### 8.3 并发保护

- 单机模式：保存期间暂停游戏逻辑写入（短暂冻结）。
- 多人模式：保存由房主权威触发，客户端不直接写入存档文件。
- 不支持多进程同时写入同一存档槽。

## 9. 性能目标

- 序列化单次保存：< 100ms（12 名居民 + 50 条关系 + 200 条记忆）。
- 反序列化加载：< 200ms。
- 存档文件大小：< 2MB（首版目标）。
- 迁移：单次迁移 < 50ms。

## 10. 设置存档

```csharp
[System.Serializable]
public class PlayerSettingsDTO
{
    public int schemaVersion;
    public float musicVolume;
    public float sfxVolume;
    public float voiceVolume;
    public float mouseSensitivity;
    public bool invertY;
    public string language;
    public bool enableRomance;
    public bool enablePhotoMode;
    public int displayResolution;
    public int qualityLevel;
    public bool fullscreen;
}
```

设置存档在 `{persistentDataPath}/settings.json`，独立于游戏存档槽。
