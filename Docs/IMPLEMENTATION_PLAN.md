# Asteria — 实施计划

> 依据：`Docs/GAME_DESIGN.md` + `Docs/PROJECT_AUDIT.md`（2026-07-26 二次审计）  
> 原则：在**已有球面 Demo**上演进；先垂直切片；禁止沙盒化  
> 状态：**Phase 0 完成；Phase 1（Observe 切片）推进中 / 以代码落地为准**

---

## 0. 产品定位摘要

| 项 | 定义 |
|----|------|
| 游戏名 | **Asteria** |
| 类型 | 2–8 人低压力**球形探索**（第一版联机目标 **2–4** 人局域网/好友） |
| 核心 | 真实球面重力；漫游、相遇、发现、记录、陪伴 |
| 互动三柱 | **Observe** / **Restore** / **Cooperate** |
| 痕迹 | 可留灯/路标/记录等，**必须有数量上限** |
| 美术 | URP；三渲二；柔和光照；**低饱和** |
| 不是 | 生存沙盒、挖矿建造、MMO 数值、PVP 竞技 |

**当前工程事实：** Phase「球面移动 Demo」资源与脚本**已经落地**（`SphereMoveDemo`）。后续阶段**禁止推倒重来**。

---

## 1. 绝对禁止项

### 玩法 / 产品

- 方块建造、挖矿、合成、大型基地
- 饥饿 / 饮水 / 生存值驱动
- 等级排行、装备数值碾压、金币经济、日常任务刷刷刷
- PVP、排位、击杀、竞争资源
- 无限堆放痕迹 / 垃圾物体淹没星球
- 为「内容量」破坏探索节奏与低压力氛围

### 工程 / AI 工作流

- 一次性大重构或删除核心球面脚本
- 手写大量 `.unity` / `.prefab` / `.meta` YAML
- 擅自引入未确认第三方插件
- 创建万能 `GameManager.cs` 承担全部逻辑
- Phase 3 前强行上联机框架
- 在 Built-in 或错误 Color Space 下新增正式材质（当前已是 URP + Linear，保持）
- **未经确认擅自大搬家或推倒球面核心脚本**

---

## 2. 阶段总览（Phase 0–4）

```
Phase 0  仓库审计 ✅
    ↓
Phase 1  球面 Demo + Observe POI + HUD + 配置 SO  ← 当前
    ↓
Phase 2  区域占位 + Restore + 痕迹上限
    ↓
Phase 3  局域网 2–4 人 + Cooperate
    ↓
Phase 4  视觉与内容```

> 与旧文档差异说明：旧计划把「装 URP + 写球面移动」当作 Phase 0.5/1。  
> **这些已完成。** 新 Phase 1 = 体验循环的第一刀（Observe），不是重做电机。

---

## 3. Phase 0 — 审计（本阶段）

### 做什么

- 只读检查版本、包、Assets、场景、脚本、Git、文档
- 更新 `PROJECT_AUDIT.md`、`IMPLEMENTATION_PLAN.md`
- **不写玩法代码**

### 验收

- [x] 审计反映真实现状（非空项目、URP 已通、Demo 已有）
- [x] 记录可保留文件与最大风险
- [x] 明确停止并等待确认

### 回滚

- 仅文档变更；不满意可还原两份 Docs

---

## 4. Phase 1 — 从现有 Demo 演进到 Observe 切片

### 4.1 目标验收（完成才算过）

1. 打开现有 `SphereMoveDemo`（或由其另存的切片场景），球面移动**回归通过**（含 W 不自旋）
2. 球上至少 **1 个 POI**，靠近可 **Observe**
3. Observe 触发：提示 UI + 至少一条「图鉴/记录」反馈（可极简）
4. 关键配置用 **ScriptableObject**（星球或交互参数可读 SO，避免魔法数散落）
5. 目录向推荐结构**温和对齐**（优先新增 `Data/`、填满 `Interaction/`；重命名可延后）
6. 仍无生存、无建造、无联机、无战斗

### 4.2 演进策略（不要推倒重来）

| 保留 | 演进 |
|------|------|
| `PlanetBody` / Gravity / Motor / Camera | 抽取输入只读包装；参数可迁 SO |
| `SphereMoveDemo.unity` + 标记柱 | 场景内加 POI；HUD 升级 |
| `Player.prefab` | 加 InteractionDetector |
| URP 设置与材质 | 可加一张 POI 材质；不做风格大改 |
| `Phase1Bootstrap` | 扩展「确保 POI / UI 存在」或新菜单，避免毁掉旧场景 |

**禁止：** 新建空项目、重装 URP、重写相机轨道数学（除非回归失败）、删除南北极标记。

### 4.3 建议组件（小而专）

```
Interaction/
  IInteractable.cs
  ObserveInteractable.cs      # 观察点：镜头轻微推移 / 解锁记录
  InteractionDetector.cs      # 玩家附近检测 + 按键确认（如 E）

Data/
  PlanetConfig.asset          # 半径、重力等（可选从 PlanetBody 读）
  ObserveEntry.asset          # 单条图鉴数据
  TraceLimitsConfig.asset     # 先占位：上限数字，Phase 2 再用

UI/
  InteractPrompt (Prefab)
  CodexToast / simple panel

Player/
  （可选）PlayerInputReader.cs  # 隔离旧 Input，便于以后换 Input System
```

### 4.4 目录重整规则（Phase 1）

**推荐默认：少迁多增。**

1. 新增 `Assets/_Game/Data/`
2. 在现有 `Interaction/Scripts/` 落代码
3. `Planet` / `Multiplayer` / `Environment` **暂不强制改名**（降低 GUID 风险）
4. 若确认改名：`World`←`Planet`，`Networking`←`Multiplayer`，`Art`←`Environment`  
   - 必须用 Unity 移动或 `git mv`  
   - 立刻 Play + 检查 Missing Script  
5. 新增 `Tests/`（EditMode：重力方向、切面投影单位长度）

### 4.5 第一阶段预计改动文件（计划）

见 `PROJECT_AUDIT.md` §5。实施时每次 PR/提交只覆盖其中一个垂直目标。

### 4.6 Phase 1 验收清单

- [ ] Play 无红字
- [ ] WASD / 跑 / 跳 / 相机回归（W 不转圈；极点可过）
- [ ] 靠近 POI 出现提示
- [ ] 触发 Observe 后有记录反馈
- [ ] 至少 1 个玩法 SO 被运行时引用
- [ ] 无沙盒系统被引入
- [ ] `PHASE1_RUN.md` 或新 `PHASE1_OBSERVE_RUN.md` 更新操作说明

### 4.7 回滚

- 按目录回滚 Interaction/Data/UI 新增文件
- 场景用 git 还原 `SphereMoveDemo.unity`
- 核心四脚本（PlanetBody / Gravity / Motor / Camera）无故不改；若改了优先还原它们

---

## 5. Phase 2 — 区域 + Restore + 痕迹上限

### 范围

- 在球上划出 **一个** 简化区域意向（建议「风之草原」开阔感，仍可用色块/简单道具）
- **Restore**：1 个可修复装置（修后改变灯光或打开短路径）
- 痕迹系统原型：放置小灯/路标，**读取 SO 上限**，超出则拒绝或替换最旧

### 不做

- 完整三区域美术、背包、经济、任务系统、联机

### 验收

- Observe 仍可用；Restore 一次状态变化可复现；痕迹不超过上限

### 回滚

- 独立 Prefab/脚本提交；可卸掉 Restore 组件回退到 Phase 1 场景

---

## 6. Phase 3 — 局域网 2–4 人

### 包引入（届时再执行，需再次确认）

见 §8。优先评估 **Netcode for GameObjects**。

### 范围

- 同步：位置、旋转、简单动作
- 无攻击；打招呼 / 标记
- **最小 Cooperate**：两人同时激活

### 验收

- 2 客户端同场景可见彼此；Cooperate 机关可触发一次全局反馈（如变色/粒子）

### 回滚

- 移除 Networking 程序集引用与 Network 组件；单机 Demo 仍可 Play

---

## 7. Phase 4 — 氛围与内容

- 三区域视觉分化；生物；天气；声音
- 正式三渲二材质与低饱和光照
- 痕迹与观察内容扩容（仍守上限与禁止项）

### 验收

- 视觉方向可辨认「Asteria」而非通用 URP 灰盒；性能在目标平台可接受

---

## 8. 包引入策略

**规则：新增任何包前，必须在本表补一行并得到确认。**

| 包名 | 用途 | 替代方案 | 风险 | 建议阶段 |
|------|------|----------|------|----------|
| （已有）URP 17.5.0 | 渲染 | 无 | 低 | 已完成 |
| （已有）Multiplayer Center | 联机向导 | 无 | 低 | 保留 |
| （已有）AI Assistant / Inference | 编辑器辅助 | 不用即可 | 体积/干扰 | 保留但不依赖 |
| Cinemachine | 轨道相机 | **现有手写相机** | Unity 6 曾 CS0619 编译失败 | **默认不引入**；除非验证兼容 |
| Input System | 新输入 | 旧 Input Manager | 双系统设置、迁移成本 | Phase 1 后可选 |
| TextMeshPro / UI Toolkit | 正式 UI | OnGUI（仅 Demo） | 低 | Phase 1 |
| Netcode for GameObjects | 2–4 人同步 | 自制 UDP（不推荐） | 学习成本、场景改造 | Phase 3 |
| Unity Services Relay/Lobby | 互联网联机 | 仅局域网 | 账号与复杂度 | Phase 3 后可选 |

---

## 9. Unity 文件安全规则

1. **Scene / Prefab / Material / URP Asset**：在 Unity Editor 中创建与修改；AI 不手写大段 YAML。
2. **脚本**：可正常编辑 `.cs`；改序列化字段后让 Unity 刷新，再在 Inspector 赋值。
3. **.meta**：不手工改 GUID；移动资源用 Unity 或 `git mv`。
4. **ProjectSettings / Packages**：只改必要项；改前说明用途；能用 Editor API / 菜单完成的优先。
5. **Bootstrap**：继续用 `Asteria/Setup…` 菜单生成内容，避免「半成品场景」。
6. **一次一小步**：电机、交互、UI、目录迁移不要捆在同一提交。

---

## 10. 每阶段验收与回滚（总表）

| 阶段 | 验收锚点 | 回滚方式 |
|------|----------|----------|
| 0 | 两份文档与现状一致 | 还原 Docs |
| 1 | Observe + HUD + SO + 移动回归 | 删新增脚本/UI；还原场景 |
| 2 | Restore + 痕迹上限 | 卸组件 / 还原场景 |
| 3 | 2–4 人同步 + Cooperate | 去网络组件；单机可玩 |
| 4 | 风格与氛围达标 | 保留灰盒分支；美术资源可卸载 |

**通用回归：** 每次合并前执行 `PROJECT_AUDIT.md` §7 的 Play 清单。

---

## 11. Cursor / AI 工作流约定

每次任务开始：

1. 阅读相关代码与 `GAME_DESIGN.md` / 本计划  
2. 说明影响范围与修改计划（短列表）  
3. 小步实现；优先垂直切片  

每次任务结束报告：

```
修改文件:
新增功能:
测试方式:
已知问题:
下一步:
```

禁止：

- 大规模重构、擅自删文件、引入未确认插件  
- 修改大量 Unity 序列化文件  
- 把项目做成沙盒  
- **Phase 0 未确认就写 Phase 1 代码**

---

## 12. 建议的下一步（待你确认后才执行）

1. （推荐）提交当前工作区为「球面 Demo 基线」commit  
2. Play 回归：确认 W 不自旋、绕球一圈  
3. Phase 1：新增 `Data` + `Interaction` Observe POI + 简单 HUD  
4. 再暂停，给你看切片  

### 待确认决策

| # | 问题 | 推荐默认 |
|---|------|----------|
| 1 | 保留现有 Demo 并进入 Observe Phase 1？ | **是** |
| 2 | 是否立即重命名 Planet→World 等？ | **否，先加目录** |
| 3 | 相机继续手写？ | **是** |
| 4 | Phase 1 输入 | **旧 Input Manager** |
| 5 | 先 git commit 基线？ | **是（需你明确要求 commit）** |

---

## 13. 文档索引

| 文档 | 作用 |
|------|------|
| `Docs/GAME_DESIGN.md` | 产品边界与体验原则 |
| `Docs/PROJECT_AUDIT.md` | 工程事实与风险 |
| `Docs/IMPLEMENTATION_PLAN.md` | 本文件：分阶段怎么做 |
| `Docs/PHASE1_RUN.md` | 现有球面 Demo 如何运行 |

---

## 14. 状态声明

**Phase 0（仓库审计）已完成。**

已更新：

- `Docs/PROJECT_AUDIT.md`
- `Docs/IMPLEMENTATION_PLAN.md`

**现在停止实现。等待你确认后，才进入 Phase 1。**
