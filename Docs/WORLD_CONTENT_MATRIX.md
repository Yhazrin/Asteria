# Asteria — 世界与内容矩阵

> 状态：Active  
> 目标：建立可以持续扩展、但不会无序堆内容的球形世界生产框架。

## 1. 世界层级

```text
Universe Profile
├── Home Planet（长期保存）
│   ├── Community Districts
│   ├── Resident Homes
│   ├── Facilities
│   └── Memory Landmarks
└── Expedition Catalog
    ├── Planet Archetype
    ├── Biome Layout
    ├── Event Deck
    ├── POI Set
    └── Reward / Story Seeds
```

## 2. 家园星球

### 2.1 尺度

首版建议直径 250–400 米，玩家 3–6 分钟可绕行一圈。

家园必须：

- 空间紧凑，居民容易相遇
- 设施可从地平线或高点辨认
- 有 1 条自然环线和 2–3 条穿越路线
- 能通过建筑、植被和灯光看出社区逐步成长
- 不要求加载大量分区或无缝超大地形

### 2.2 区域

| 区域 | 作用 | 设施例子 | 社交倾向 |
|---|---|---|---|
| 星港坡 | 出生、远征、访客到达 | 信标、远征台、仓库 | 欢迎、送别、重逢 |
| 风铃广场 | 社区核心 | 舞台、共享厨房、公告墙 | 聚会、冲突、庆典 |
| 云芽居住带 | 居民生活 | 小屋、共享宅、庭院 | 拜访、室友、日常 |
| 观测脊 | 高处与回忆展示 | 观测台、纪念馆 | 独处、约会、回顾 |
| 温室谷 | 生态展示 | 温室、水池、育种架 | 照料、研究、礼物 |

### 2.3 建设锚点

家园表面划分有限的 `BuildAnchor`：

- Large：公共设施，首版 4–6 个
- Medium：居民住宅或功能模块，首版 8–12 个
- Small：装饰、路灯、纪念物，首版 20–30 个

每个锚点保存局部切线坐标、允许类型、朝向限制和视觉遮挡规则。所有对象仍通过星球中心法线对齐。

## 3. 远征星球生成

### 3.1 生成原则

首版不追求完全程序化地形。采用“预制球面拓扑 + 数据化组合”：

1. 选择一个经过人工验证的基础球体/高度场。
2. 按纬度、坡度、噪声和距离场划分生态区。
3. 从 POI 槽位中抽取事件地点。
4. 按球面最短路径生成信标与风险路线。
5. 由事件导演决定天气阶段和目标顺序。

这样既保留重复游玩的变化，也能确保每种组合可测试。

### 3.2 星球原型

| 原型 | 核心地形 | 主要压力 | 球面玩法 | 家园回报 |
|---|---|---|---|---|
| 风之草原 | 开阔坡地、风带、峡谷 | 强风、失衡 | 沿纬度风带滑翔 | 风铃种子、风帆设施 |
| 雾声森林 | 密林、低能见度 | 迷失、孢子 | 越过地平线听声定位 | 发光植物、声音档案 |
| 星砂夜谷 | 暗色沙丘、发光路径 | 黑暗、低温 | 昼夜线改变路线 | 星砂灯、夜间活动 |
| 浮冰潮汐星 | 冰壳、裂隙、热泉 | 受寒、地表断裂 | 全球潮汐让路线周期变化 | 温泉设施、冰晶生物 |
| 花粉云庭 | 巨花、漂浮孢团 | 视听失真 | 上风/下风半球差异 | 香气工坊、园艺角色 |
| 失落机械星 | 遗迹、轨道装置 | 能量不足、机关 | 星球两侧同步修复 | 工坊模块、机械居民线索 |

首个开发目标只做“风之草原”。其他原型仅作为扩展框架。

## 4. POI 体系

每个远征星球建议 10–16 个 POI 槽位，其中一局激活 6–10 个。

| POI 类型 | 数量建议 | 主要系统 | 示例 |
|---|---:|---|---|
| Observe | 2–4 | 图鉴、线索 | 风铃石、迁徙生物、异常云层 |
| Restore | 1–2 | 修复、环境变化 | 风塔、生态泵、古老灯塔 |
| Cooperate | 1–2 | 多人同步 | 双极机关、跨地平线共鸣器 |
| Shelter | 1–2 | 生存、安全网络 | 洞穴、暖泉、避风凹地 |
| Social | 0–2 | 星友事件 | 同行居民回忆点、陌生旅人 |
| Choice | 1 | 分支结局 | 修复哪片生态、救谁、带走什么 |
| Vista | 1–3 | 节奏、拍照 | 极点、昼夜线、云海高坡 |

## 5. 内容标签

所有内容资产应使用统一标签，供导演、生成器和社会模拟筛选：

```text
Biome: Wind / Mist / Night / Ice / Bloom / Ruin
Mood: Cozy / Curious / Funny / Tense / Wondrous / Melancholy
Action: Observe / Care / Restore / Cooperate / Traverse / Social
Pressure: None / Wind / Cold / Dark / Spores / Instability
GroupSize: Solo / Duo / TrioPlus
Time: Day / Dusk / Night / Any
Memory: Friendship / Rescue / Discovery / Conflict / Celebration
```

首版用 enum + ScriptableObject 列表即可，不要提前做复杂标签服务器。

## 6. 事件卡结构

```text
WorldEventDefinition
- id
- title
- biomeTags
- moodTags
- requiredPoiTypes
- minPlayers / maxPlayers
- requiredResidentTraits
- phase: Arrival / Invitation / Complication / Pressure / Resolution
- durationRange
- worldStateConditions
- setupActions
- runtimeObjectives
- successOutcome
- partialOutcome
- followUpSeeds
- cooldown
```

## 7. 风之草原首批内容矩阵

| 阶段 | 事件 | 系统 | 多人差异 | 返家后续 |
|---|---|---|---|---|
| Arrival | 风向初测 | Observe | 多人可从不同半球校准 | 解锁当天风图 |
| Invitation | 失声的风铃石 | Observe/Care | 一人寻找，一人维持照明 | 居民想制作风铃 |
| Invitation | 风兽迁徙 | Traverse/Photo | 分头占据观测点 | 纪念馆新增照片组 |
| Complication | 迷路的小旅人 | Social/Escort | 队伍需要包围式引导 | 新星友邀请线索 |
| Complication | 风塔叶片散落 | Restore | 搬运与安装分工 | 解锁观测台模块 |
| Pressure | 全球强风 | Survival | 信标链与牵引绳更重要 | 星友讨论谁最可靠 |
| Resolution | 双极共鸣 | Cooperate | 两侧同时完成 | 家园出现一夜极光 |
| Choice | 留下种子或修复巢穴 | Choice | 全队投票/房主确认 | 不同生态与居民事件 |

## 8. 生物矩阵

生物不以战斗掉落为核心。

| 行为型 | 玩家关系 | 主要交互 | 设计价值 |
|---|---|---|---|
| 好奇型 | 主动靠近 | 模仿、赠物、拍照 | 制造喜剧与陪伴 |
| 胆怯型 | 保持距离 | 慢速接近、环境安抚 | 路线与耐心 |
| 群居型 | 受整体状态影响 | 引导群体、保护迁徙 | 多人分工 |
| 共生型 | 与植物/设施关联 | Restore 后出现 | 展示生态反馈 |
| 引路型 | 知道隐藏路径 | 跟随声音或动作 | 非 UI 导航 |
| 扰动型 | 改变工具/天气 | 观察规律而非攻击 | 临场意外 |

## 9. 内容预算

每新增一个生态原型前，必须评估：

- 1 个球面移动差异
- 1 个独特生存压力
- 1 个生态恢复反馈
- 2 个 Observe 内容
- 1 个双人合作内容
- 1 个同行星友后续
- 1 套环境音与视觉识别

只换颜色、模型和材料而没有玩法差异，不算新星球原型。

## 10. 视觉方向

- URP，低饱和三渲二
- 大形体清楚，远处地标轮廓优先
- 材质层次依靠明暗分区、柔和渐变、细颗粒与风向动画
- 角色比例可爱但保持成年/全年龄中性表达，不做婴幼儿化
- UI 像旅行手帐和天文仪器的结合，不使用厚重战斗 HUD
- 球面地平线、昼夜线、云层和极光是品牌级视觉资产
