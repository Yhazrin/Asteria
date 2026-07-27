# Asteria — Phase 0 项目审计

> 审计日期：2026-07-26（二次审计，覆盖过时结论）  
> 依据：`Docs/GAME_DESIGN.md`、`Docs/PHASE1_RUN.md`、仓库真实现状  
> 范围：**只读分析**；仅允许更新本文件与 `IMPLEMENTATION_PLAN.md`。未改玩法代码 / Scene / Prefab / Packages / ProjectSettings。

---

## 0. 结论（先看这里）

| 维度 | 状态 | 说明 |
|------|------|------|
| Unity 版本 | 合适 | `6000.5.5f1`（Unity 6） |
| 渲染管线 | **已就绪** | URP `17.5.0` + Linear + 自定义 URP Asset |
| Assets 内容 | **非空** | 已有 `_Game` 目录、球面 Demo、脚本、材质、Prefab |
| 球面玩法基础 | **已跑通（垂直切片）** | 向心重力、切面移动、第三人称轨道相机、绕球标记 |
| 已知 WS 原地转圈 | **代码侧已修复** | 相机轨道帧与角色 facing 解耦（见 §9） |
| Observe / Restore / 痕迹 | 未开始 | Interaction 目录为空 |
| 联机基础 | **未就绪** | 仅 Multiplayer Center，无 Netcode |
| 目录 vs 推荐结构 | **部分对齐** | 缺 World/Data/Art/Scenes/Tests；现用 Planet/Environment/Multiplayer |
| 总体判定 | **可在现有 Demo 上演进** | 禁止推倒重来；下一阶段补 Observe + 目录重整 + 配置 SO |

**一句话：** 这不是空仓库。Asteria 已具备可 Play 的球面移动 Demo（URP + 自定义重力 + 手写第三人称相机）。Phase 0 之后应**保留并演进**该底座，进入 Observe / HUD / 配置化 / 目录对齐；不要重装 URP、不要重写球面电机。

---

## 1. 当前项目具备什么能力

### 1.1 工程与渲染

- Unity Editor：`6000.5.5f1`（revision `d16e074b49fd`）
- `productName`: Asteria；`companyName`: DefaultCompany
- URP 已安装并接入 Graphics Settings（`Asteria_URP.asset`）
- Color Space：**Linear**（`m_ActiveColorSpace: 1`）
- 输入：旧 Input Manager（`activeInputHandler: 0`），Axes 含 Horizontal / Vertical / Jump / Mouse X/Y

### 1.2 球面世界与角色（可运行）

| 能力 | 实现位置 |
|------|----------|
| 星球中心、半径、重力强度 | `Assets/_Game/Planet/Scripts/PlanetBody.cs` |
| 禁用世界重力 + 向心加速度 | `SphericalGravityBody.cs` |
| WASD / 跑 / 跳（切平面，相机相对） | `SphericalMotor.cs` |
| 第三人称轨道相机 + SphereCast 遮挡 | `SphericalThirdPersonCamera.cs` |
| 运行时兜底生成 Demo | `SphereMoveDemoBuilder.cs` |
| 操作提示 HUD（OnGUI） | `SphereMoveDemoHud.cs` |
| Editor 一键 / batch 搭建 | `Phase1Bootstrap.cs`、`Phase1AutoSetup.cs` |

### 1.3 场景与资源

- 场景：`Assets/_Game/Planet/Scenes/SphereMoveDemo.unity`（已入 Build Settings）
- 场景内对象：Planet、Player、Main Camera、DemoHUD、Directional Light、南北极与赤道标记柱
- Prefab：`Assets/_Game/Player/Prefabs/Player.prefab`（含 Gravity + Motor）
- 材质：`M_PlanetSurface` / `M_EquatorBand` / `M_NorthPole` / `M_SouthPole` / `M_Player` / `M_Marker`（URP Lit）
- URP：`Asteria_URP.asset` + `Asteria_URP_Renderer.asset`
- 运行说明：`Docs/PHASE1_RUN.md`

### 1.4 尚不具备

- Observe / Restore / Cooperate 交互
- 玩法配置 ScriptableObject（星球参数、交互、痕迹上限等）
- 正式 UI（UGUI / UI Toolkit / TMP）
- asmdef、EditMode/PlayMode 测试
- 联机（Netcode / 同步）
- 三渲二风格化渲染、生物、天气、音频内容
- Cinemachine（曾尝试，因 Unity 6 编译冲突已移除，见 `PHASE1_RUN.md`）

---

## 2. 哪些文件可以保留

### 强烈保留（下一阶段直接复用）

| 路径 | 理由 |
|------|------|
| `Assets/_Game/Planet/Scripts/PlanetBody.cs` | 球面权威数据源 |
| `Assets/_Game/Player/Scripts/SphericalGravityBody.cs` | 向心重力 |
| `Assets/_Game/Player/Scripts/SphericalMotor.cs` | 移动核心（已解耦相机） |
| `Assets/_Game/Player/Scripts/SphericalThirdPersonCamera.cs` | 球面相机（已解耦 facing） |
| `Assets/_Game/Planet/Scenes/SphereMoveDemo.unity` | 当前唯一可验收场景 |
| `Assets/_Game/Player/Prefabs/Player.prefab` | 角色组装基线 |
| `Assets/_Game/Environment/Materials/*.mat` | Demo 分区色，便于绕球验收 |
| `Assets/_Game/Core/Settings/Asteria_URP*.asset` | URP 管线资产 |
| `Assets/_Game/Editor/Phase1Bootstrap.cs` | 可演进为「修复 / 重建 Demo」工具 |
| `Docs/GAME_DESIGN.md`、`PHASE1_RUN.md` | 产品边界与运行手册 |

### 可保留但需演进

| 路径 | 说明 |
|------|------|
| `SphereMoveDemoHud.cs` | OnGUI 可暂留；Phase 1 用正式 HUD 替换或并存 |
| `SphereMoveDemoBuilder.cs` | 运行时兜底有用；正式场景完善后可降为 Editor-only |
| `Phase1AutoSetup.cs` | 避免重复跑；目录重整后需更新路径常量 |
| `Assets/DefaultVolumeProfile.asset`、`UniversalRenderPipelineGlobalSettings.asset` | URP 全局配套，保留 |

### 已删除 / 无需恢复

- `Assets/Editor/HubForceResolve.cs`（git 显示已删）：Hub 强制解析临时脚本，与玩法无关，不必恢复。

---

## 3. 哪些结构需要新增（对照推荐 `Assets/_Game`）

### 推荐目标结构

```
Assets/_Game
├── Core/          # 启动、事件、通用工具、Settings
├── World/         # 星球 / 区域 / 地表（现 Planet 应对齐或迁入）
├── Player/        # 角色、相机、输入
├── Interaction/   # Observe / Restore / Cooperate
├── Networking/    # 联机（现 Multiplayer 应对齐）
├── UI/
├── Data/          # ScriptableObject 配置
├── Audio/
├── Art/           # 材质、模型、特效（现 Environment 可并入）
├── Scenes/        # 正式场景集中（或保留 World/Scenes）
├── Editor/
└── Tests/
```

### 现状对照

| 推荐 | 现状 | 动作建议（Phase 1，计划级） |
|------|------|------------------------------|
| Core | 有 Settings；Scripts 空 | 补 Bootstrap / 事件总线（小而专），不写万能 GameManager |
| World | 现为 `Planet/` | **优先保留 Planet 命名**，或在迁移时用 git mv + 更新 namespace；勿手改 .meta GUID |
| Player | 已有 Scripts + Prefabs | 保留；可拆 `PlayerInputReader` |
| Interaction | 空文件夹 | 新增 `IInteractable`、Observe 流程 |
| Networking | 现为 `Multiplayer/`（空） | Phase 3 前可改名或建别名目录；暂不装 Netcode |
| UI | Prefabs 空 | 交互提示 + 图鉴条 |
| Data | **缺失** | 新增 `PlanetConfig`、`ObserveConfig`、`TraceLimits` 等 SO |
| Audio | 空 | Phase 4 再填 |
| Art | 现为 `Environment/Materials` | 逐步迁入 Art 或把 Environment 视为 Art 子集 |
| Scenes | 在 `Planet/Scenes` | 可保留；或增加 `_Game/Scenes` 软链接式整理 |
| Editor | 已有 Bootstrap | 保留并扩展 |
| Tests | **缺失** | 新增 EditMode：重力方向、切面投影；PlayMode：可选 |

**原则：** 目录重整用 **Unity 内移动或 `git mv`**，禁止批量手写 `.meta` / 重生成 GUID 导致引用断裂。

---

## 4. 哪些包已安装

### 显式依赖（`Packages/manifest.json`）

| Package | 版本 | 评估 |
|---------|------|------|
| `com.unity.render-pipelines.universal` | 17.5.0 | **核心，保留** |
| `com.unity.multiplayer.center` | 1.0.1 | 仅向导；不是运行时联机 |
| `com.unity.ai.assistant` | 2.16.0-pre.1 | 编辑器 AI；与玩法无关，可留 |
| `com.unity.ai.inference` | 2.6.1 | 同上；Phase 1–3 不依赖 |
| 各 `com.unity.modules.*` | 内置 | 正常 |

### 传递依赖（节选，`packages-lock.json`）

- `com.unity.render-pipelines.core` 17.5.0
- `com.unity.shadergraph` 17.5.0
- `com.unity.burst`、`com.unity.collections`、`com.unity.mathematics`（随 URP / AI）
- `com.unity.test-framework`（随 collections 传递；**项目内尚无自有 Tests**）

### 明确未安装

| Package | 说明 |
|---------|------|
| Cinemachine | 曾因 CS0619 阻断编辑器编译而移除；现用手写相机 |
| Input System | 未装；旧 Input 足够跑 Demo |
| Netcode for GameObjects / Mirror / 等 | 未装 |
| TextMeshPro | 未在 manifest 显式声明（Unity 6 可能内置可选；正式 UI 时再确认） |

---

## 5. 第一阶段将修改哪些文件（仅计划，不执行）

> 此处「第一阶段」= 在**已有球面 Demo**上完成 Observe 垂直切片 + 目录/配置对齐（见 `IMPLEMENTATION_PLAN.md`）。  
> **不**重写重力/电机/相机核心逻辑，除非回归测试失败。

### 预计新增

- `Assets/_Game/Interaction/Scripts/IInteractable.cs`
- `Assets/_Game/Interaction/Scripts/ObserveInteractable.cs`（或等价命名）
- `Assets/_Game/Interaction/Scripts/InteractionDetector.cs`（靠近检测）
- `Assets/_Game/Data/*.asset` + 对应 SO 脚本（星球 / Observe / 痕迹上限）
- `Assets/_Game/UI/` 下提示与图鉴 UI（Prefab 由 Editor 生成，避免手写 YAML）
- `Assets/_Game/Tests/` 基础 EditMode 测试（可选但强烈建议）
- 场景内 POI 物体（在 `SphereMoveDemo` 或新切片场景中用 Editor 摆放）

### 预计修改（小步）

- `SphereMoveDemo.unity`：增加 1 个 POI + UI 引用（**在 Editor 内改**）
- `SphereMoveDemoHud.cs` → 迁到正式 UI 或改为读 SO 文案
- `Phase1Bootstrap.cs`：路径常量若目录重整则同步
- `Player.prefab`：挂检测器组件（Editor 内）
- 可能：`SphericalMotor` / Camera **仅**做输入隔离抽取，不改运动学公式

### 明确不动（除非验收必须）

- URP Asset / GraphicsSettings（已正确）
- Packages（除经确认的 TMP / Input System）
- 球面物理核心公式
- 推倒重建 `SphereMoveDemo`

---

## 6. 当前最大技术风险

| 排序 | 风险 | 等级 | 说明与缓解 |
|------|------|------|------------|
| 1 | **目录/GUID 重整破坏引用** | 高 | 移动 `Planet`→`World` 或批量改 meta 会导致 Scene/Prefab 丢脚本。缓解：少迁；必须迁时用 Unity 或 git mv，并立刻 Play 回归 |
| 2 | **极点附近相机/朝向稳定性** | 中高 | Demo 已处理投影与重初始化，但极点穿越仍需人工验收。缓解：固定验收路径（赤道→北极→背面→南极） |
| 3 | **产品漂移成沙盒** | 高（产品） | 代码底座干净，但内容膨胀易引入生存/建造。缓解：严格禁止项清单 |
| 4 | **过早上联机** | 中 | 仅有 Multiplayer Center。缓解：Phase 3 前不装 Netcode |
| 5 | **Cinemachine 再引入编译失败** | 中 | 历史已踩坑。缓解：Phase 1 继续用手写相机；若再引入需验证 Unity 6.0 兼容版本 |
| 6 | **未提交的大块工作区** | 中 | git 仅有 `Initial check-in`；`Assets/_Game`、Docs、URP 设置多为未提交。缓解：确认后尽快做一次干净提交（由人工发起） |
| 7 | **AI 包体积与干扰** | 低 | assistant/inference 与玩法无关。可留；勿让其驱动架构 |

**最大单点风险：** 在已可玩 Demo 上做「大搬家式重构」导致引用断裂与回归成本，高于缺少 Observe 本身。

---

## 7. 如何验证每一步没有破坏项目

每步变更后执行最小回归：

1. **编译**：Console 无 error（含 Editor 程序集）
2. **打开** `SphereMoveDemo.unity` → Play
3. **移动验收**
   - 站立不坠入太空
   - WASD 沿切面；**按 W 不应原地转圈**（相机解耦回归）
   - Shift 跑、Space 跳
   - 鼠标旋转；Esc / 左键光标锁
4. **绕球验收**：经过赤道标记与南北极，走到背面仍可见标记
5. **URP**：材质粉红/洋红 = shader 丢失，立即停
6. **引用完整性**：Player / Camera / PlanetBody 序列化字段非 Missing
7. **可选**：EditMode 测试 `GetSurfaceUp` / 重力方向单位向量

回滚：对该步 `git checkout -- <files>` 或恢复 Unity 场景备份；不混多个大改在一次提交。

---

## 8. 专项评估

### 8.1 Unity 版本

`6000.5.5f1` — 符合设计；无需换版。

### 8.2 URP

已安装、已指定、Linear 已开、自有 URP Asset GUID 与 GraphicsSettings 一致。  
美术仍是纯色 Lit，**尚未**三渲二低饱和风格化 — 属 Phase 4，非阻断。

### 8.3 场景

唯一正式 Demo 场景已入 Build Settings；Play 即可验球面移动。  
无多场景流、无主菜单。

### 8.4 联机就绪度

| 项 | 状态 |
|----|------|
| Multiplayer Center | 有（工具） |
| Netcode / 传输层 | 无 |
| 网络对象 / 同步代码 | 无 |
| 目标 2–4 人局域网 | **未就绪**（计划 Phase 3） |

### 8.5 与产品定位偏差风险

| 定位要求 | 现状偏差 |
|----------|----------|
| 低压力球形探索 | Demo 符合「探索底座」 |
| Observe / Restore / Cooperate | 未实现 → 下一阶段优先 Observe |
| 痕迹有上限 | 无痕迹系统 |
| 非沙盒 / 非竞技 | 代码无生存/战斗，**风险在未来内容决策** |
| URP 三渲二低饱和 | 管线对，风格未做 |
| 2–8 人（首版 2–4） | 联机未开始 |

**偏差结论：** 技术底座与「球形探索」对齐；与「完整体验循环」仍差交互与联机。最大产品风险是后续把 Demo 做成沙盒，而非当前代码已沙盒化。

---

## 9. 已知移动 Bug 修复状态

**问题：** `SphericalThirdPersonCamera` 与角色 facing 形成反馈环 → 按 W/S 原地转圈。

**现状（代码审查）：已按正确架构修复。**

证据：

- 相机维护独立 `_planarForward` 轨道帧，注释明确：*Orbit yaw is independent of the player facing*
- `SphericalMotor.GetTangentMoveDirection` 使用 `_orbitCamera.PlanarForward / PlanarRight`，**不用** `transform.forward` 作为移动基
- 角色旋转只影响视觉朝向，不再驱动相机 yaw

**验证状态：** 静态代码已正确；本 Phase 0 **未**在 Editor 中实机复测。进入下一阶段前建议人工 Play 确认 W 前进无自旋。

---

## 10. Git 状态（只读）

| 项 | 值 |
|----|-----|
| 分支 | `main` @ `5c470f5`（Initial check-in） |
| 远程跟踪 | 未见活跃 ahead/behind 信息（本地为主） |
| 已修改 | `Packages/manifest.json`、`packages-lock.json`、多项 ProjectSettings |
| 已删除 | `Assets/Editor/HubForceResolve.cs` |
| 未跟踪 | 整个 `Assets/_Game/`、`Docs/`、URP 全局资产、部分 ProjectSettings |

**含义：** 球面 Demo 与文档几乎全在工作区，**尚未形成可回滚的提交历史**。确认 Phase 0 后建议先提交基线再改 Phase 1。

---

## 11. Docs 现有内容

| 文档 | 状态 |
|------|------|
| `GAME_DESIGN.md` | 产品边界完整；Phase 定义仍以「球面 Demo = Phase 1」为主 |
| `PROJECT_AUDIT.md` | **本文件已覆盖更新**（旧版误判为空项目 / 无 URP） |
| `IMPLEMENTATION_PLAN.md` | 同步重写：从现有 Demo 演进 |
| `PHASE1_RUN.md` | 有效；描述如何 Play / CLI Bootstrap；记录 Cinemachine 移除原因 |

---

## 12. Phase 0 产出对照

| 要求 | 结果 |
|------|------|
| Unity / URP / Package / Assets / Scene | 已基于真实现状记录 |
| MonoBehaviour / Prefab / 材质 | 已列出 |
| Input / Camera / Networking | 旧 Input；手写相机；无 Netcode |
| asmdef / Tests / Editor 工具 | 无 asmdef；无自有 Tests；有 Phase1 Editor 工具 |
| 移动 bug | 代码侧已修，待 Play 确认 |
| 可保留 / 需新增 / 风险 / 验证 | 见 §2–7 |
| 不写玩法代码 | 遵守 |
| 完成后等待确认 | **停止于此** |

---

## 13. 等待确认

请确认后再进入下一阶段（不要自动开始实现）：

1. 是否同意 **保留现有球面 Demo**，在其上做 Observe + HUD + SO + 温和目录对齐？  
2. 目录是否 **暂缓重命名** `Planet`→`World` / `Multiplayer`→`Networking`（推荐暂缓，先加 Data/Interaction）？  
3. Phase 1 相机是否 **继续手写** `SphericalThirdPersonCamera`（推荐是）？  
4. 是否先 **git commit 当前基线** 再动 Phase 1？  
5. 输入是否继续旧 Input Manager 到 Observe 切片完成？
