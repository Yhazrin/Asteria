# Asteria — 术语表

> 状态：Active
> 目标：统一项目术语，避免歧义，确保团队（包括 AI 工具）使用一致语言。

## A

**ADR (Architecture Decision Record)**
架构决策记录。记录重要产品或技术决策的文档格式。见 `DECISION_LOG.md`。

**Affinity**
亲近感。关系边的一个维度，表示两居民之间的情感亲近程度。范围 -1..1。

**Anchor / BuildAnchor**
建设锚点。家园星球表面预设的设施放置位置，有大小类型（Large/Medium/Small）和允许设施类型限制。

**Archetype / Planet Archetype**
星球原型。远征星球的基础类型定义，包含地形、生态区、POI 槽位和事件牌组。首版只有"风之草原"。

## B

**Biome / BiomeTag**
生态区/生态标签。星球表面的功能区域划分，如 Wind（风之草原）、Mist（雾声森林）等。

**Bootstrap**
启动场景/启动流程。游戏启动时加载的第一个场景，负责配置、存档、会话和全局 UI。

## C

**Care**
安抚/照料。玩家动作之一，用于安抚生物、喂养、照料和陪伴。

**Checkpoint / Expedition Checkpoint**
远征检查点。远征过程中的存档点，用于掉线恢复。

**Cooperate**
合作。玩家动作之一，多人共同完成空间任务。

**Cooldown**
冷却。事件触发后的等待期，防止同一事件重复触发。

**Core Gameplay Loop**
核心玩法循环。每分钟、每局、长期三层循环结构。

## D

**Director / Event Director**
事件导演。负责根据条件选择和调度事件的系统。

**Discovery**
发现。玩家通过 Observe、Restore 或 Cooperate 获得的记录。

**DiscoveryJournal / IDiscoveryRepository**
发现图鉴/发现仓库。记录玩家所有发现的系统。

**DTO (Data Transfer Object)**
数据传输对象。纯 C# 数据类，用于存档、网络传输等。

## E

**EditMode Test**
编辑模式测试。Unity 编辑器中运行的纯逻辑测试，不需要运行游戏。

**Event Deck / EventDeckEntry**
事件牌组。远征星球可用事件的集合。

**Event Definition**
事件定义。ScriptableObject 形式的事件模板，包含前置条件、内容和效果。

**Expedition**
远征。20–40 分钟一局的可重复球形探索空间。

**Expedition Planet**
远征星球。远征发生的星球，可重复进入。

## F

**Facility / FacilityDefinition**
设施/设施定义。家园中的可建造建筑，如观测台、共享厨房等。

**FollowUpSeed**
后续种子。事件完成后的后续事件触发器。

## G

**GameBootstrap**
游戏启动器。负责初始化游戏系统的组合根。

## H

**Home Planet**
家园星球。长期保存的社区空间，居民在此生活。

## I

**IInteractable**
可交互接口。所有可被玩家交互的对象实现的接口。

**ID**
标识符。稳定字符串标识，格式为 `{namespace}.{type}.{slug}`。

**InteractionDetector**
交互检测器。检测玩家附近可交互对象的组件。

**InventorySlotDTO**
背包槽 DTO。玩家背包中的物品槽位数据。

## M

**Memory / MemoryRecord**
记忆/记忆记录。居民或玩家经历的重要事件记录。

**MemoryTag**
记忆标签。记忆的分类标签，如 Friendship、Rescue 等。

**Milestone**
里程碑。路线图中的阶段目标，如 Milestone A（固化 Observe 基线）。

**MoodTag**
情绪标签。事件或场景的情绪分类，如 Cozy、Curious 等。

## N

**Navigation / Spherical Navigation**
球面导航。在球面上寻路的系统，使用预定义路网节点。

**Netcode for GameObjects (NGO)**
Unity 的网络同步框架。

## O

**Observe**
观察。玩家动作之一，识别与记录环境中的事物。

**ObserveEntry**
观察条目。图鉴中的单条观察记录数据。

**Originality Boundary**
原创边界。Asteria 不得复制的内容范围。

## P

**Persistence / Persist**
持久化。将游戏状态保存到磁盘。

**Personality / PersonalityState**
性格/性格状态。居民的 5 维连续性格模型（Sociability、Curiosity、Warmth、Order、Boldness）。

**PlanetArchetypeDefinition**
星球原型定义。远征星球类型的 ScriptableObject 定义。

**PlayMode Test**
游戏模式测试。Unity 中运行游戏场景的测试。

**POI (Point of Interest)**
兴趣点。星球上的交互位置，如风铃石、风塔等。

**Pressure / PressureDefinition**
压力/压力定义。事件型轻生存中的环境压力，如强风、低温等。

## Q

**Quirk / QuirkDefinition**
怪癖/怪癖定义。居民的离散个性特征，如"会给所有植物取名字"。

## R

**Relationship / RelationshipEdge**
关系/关系边。两居民之间的多维度关系数据。

**Resident / ResidentDefinition**
星友/居民/居民定义。家园中的自主生活角色。

**Restore**
修复。玩家动作之一，修复装置和生态节点。

## S

**Save / SaveRoot**
存档/存档根。游戏持久化数据的顶层结构。

**SchemaVersion**
存档版本。存档格式版本号，用于迁移。

**Schedule / ScheduleTemplate**
日程/日程模板。居民每日活动计划。

**ScriptableObject (SO)**
Unity 的数据容器资产，用于定义静态内容。

**SocialEvent**
社会事件。家园中由居民自主触发的事件。

## T

**Test Matrix**
测试矩阵。定义各系统的测试类型和验收标准。

**Tool / ToolDefinition**
工具/工具定义。玩家携带的主动工具，如共鸣镜、暖光灯等。

**Trace / TraceLimit**
痕迹/痕迹上限。玩家在远征中放置的临时工具数量限制。

## U

**URP (Universal Render Pipeline)**
Unity 通用渲染管线。

## V

**Vertical Slice**
垂直切片。可独立运行和验收的最小功能集。

## W

**Wish / WishTemplate**
愿望/愿望模板。居民提出的可被玩家理解的具体诉求。

**WorldEvent**
世界事件。远征中由事件导演触发的环境事件。

## 中文术语对照

| 中文 | 英文 | 说明 |
|------|------|------|
| 星友 | Resident | 家园居民 |
| 远征 | Expedition | 短局探索 |
| 家园 | Home Planet | 长期社区空间 |
| 观察 | Observe | 识别与记录 |
| 修复 | Restore | 修复装置和生态 |
| 合作 | Cooperate | 多人共同完成 |
| 照料 | Care | 安抚、喂养、陪伴 |
| 事件导演 | Event Director | 事件调度系统 |
| 建设锚点 | Build Anchor | 设施放置位置 |
| 性格 | Personality | 居民性格维度 |
| 怪癖 | Quirk | 居民离散特征 |
| 关系边 | Relationship Edge | 居民关系数据 |
| 记忆 | Memory | 重要事件记录 |
| 愿望 | Wish | 居民具体诉求 |
| 压力 | Pressure | 环境生存压力 |
| 生态区 | Biome | 星球区域类型 |
| 兴趣点 | POI | 交互位置 |
| 设施 | Facility | 可建造建筑 |
| 工具 | Tool | 玩家主动工具 |
