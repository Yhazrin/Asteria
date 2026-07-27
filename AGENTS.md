# AGENTS.md — Asteria 开发执行规则

本文件适用于 Cursor、Codex、Claude Code 及其他自动化开发 Agent。

## 1. 每次任务的必读顺序

1. `Docs/README.md`
2. `Docs/PRODUCT_VISION_V2.md`
3. 与任务直接相关的系统文档（见下方文档矩阵）
4. `Docs/ROADMAP_V2.md`
5. 当前相关代码、Scene、Prefab、Package 与 Git 历史

不得只读用户的一段提示词就开始修改。

### 文档矩阵

| 任务类型 | 必读文档 |
|----------|----------|
| 数据结构/存档 | `DATA_CONTRACTS.md`、`SAVE_SCHEMA.md` |
| 事件/导演 | `EVENT_DIRECTOR.md`、`CONTENT_TAGGING.md` |
| 测试/回归 | `TEST_SPEC.md` |
| 编辑器工具 | `EDITOR_TOOLING.md` |
| 美术/视觉 | `ART_STYLE_GUIDE.md` |
| 音频/声音 | `AUDIO_DESIGN.md` |
| 术语/概念 | `GLOSSARY.md` |
| 新成员 | `ONBOARDING.md` |

## 2. 当前项目基线

- Unity 6 / URP
- 已有真实球面重力、移动、相机和可运行 `SphereMoveDemo`
- 已有球面散布、Observe POI、发现图鉴、HUD 和 Editor Upgrade 工具
- 尚无正式存档、居民模拟、建设、轻生存和运行时联机

默认结论：**在现有切片上演进，不重建项目。**

## 3. 绝对禁止

- 新建另一个 Unity 项目替换当前仓库
- 无证据重写 `PlanetBody`、`SphericalGravityBody`、`SphericalMotor` 或球面相机
- 手写或批量生成大段 `.unity`、`.prefab`、`.mat`、`.asset` YAML
- 手改 `.meta` GUID
- 一次提交同时做目录大迁移、输入替换、相机重写和联网接入
- 创建万能 `GameManager.cs`
- 擅自安装第三方包
- 把游戏改成体素挖掘、无限建造、持续饥饿口渴、PVP 或数值 MMO
- 复制参考游戏的具体 UI、文本、角色、美术、事件和资源

## 4. Unity 文件规则

- Scene / Prefab / Material / ScriptableObject 资产优先通过 Unity Editor 或已有 Editor 工具创建。
- 移动资源使用 Unity Editor 或保留 `.meta` 的 `git mv`。
- 修改序列化字段后，必须检查 Scene/Prefab 是否出现 Missing Script 或引用丢失。
- Package 和 ProjectSettings 变更必须单独说明原因、版本、替代方案和回滚方法。
- Editor Upgrade 工具必须尽可能幂等：重复运行不应重复生成内容。

## 5. 架构规则

- 小组件、小服务、接口、事件和 ScriptableObject 优先。
- 静态内容定义与运行时状态分离。
- 存档使用纯 C# DTO，不直接序列化 Scene 对象。
- 玩法逻辑不直接依赖具体网络 SDK。
- UI 只消费 ViewModel / 事件，不直接改关系、事件阶段和存档。
- 所有持久化内容使用稳定字符串 ID。
- 球面坐标、朝向和路径必须以星球中心/局部切面为依据。

## 6. 任务开始时必须输出

```text
目标:
当前基线:
涉及模块:
预计修改文件:
主要风险:
最小验收:
不做什么:
```

若发现文档与代码冲突，先报告冲突并以仓库真实代码 + 最新 Accepted 决策为准。

## 7. 实施方式

- 一次只交付一个垂直目标。
- 先补测试或验收入口，再修改高风险核心。
- 优先新增，不做无收益的大搬家。
- 每一步都保持项目可编译、可打开、可 Play。
- 不得用“后续再测试”替代当前可执行的最小验证。

## 8. 通用回归

每次运行时代码变更后至少检查：

1. Console 无 Error。
2. 打开 `Assets/_Game/Planet/Scenes/SphereMoveDemo.unity`。
3. WASD、Shift、Space、鼠标控制正常。
4. W 前进不原地自旋。
5. 玩家能经过极点、走到星球背面。
6. 相机不发生持续翻转或明显穿地。
7. Observe 提示和记录正常。
8. 无粉色材质、Missing Script、丢失引用。
9. 相关 Editor Upgrade 重复运行不制造重复对象。

## 9. 分系统验收

### 存档

- 重启后数据恢复
- 原子写入与备份可用
- schemaVersion 和迁移测试存在

### 居民

- 不操作时能完成日程
- 不拥堵、不穿地、不在极点抖动
- 关键记忆可保存

### 建设

- 只在合法锚点
- 设施影响居民行为
- 删除/替换不会留下悬空引用

### 轻生存

- 压力有清楚预告与恢复方式
- 不引入长期惩罚表
- 单人可完成，多人更有价值

### 联机

- 单机仍可独立运行
- 主机只结算一次
- 跨极点同步稳定
- 掉线重连恢复权威快照

## 10. 任务结束报告

```text
修改文件:
完成内容:
未完成内容:
验证方式与结果:
已知问题:
架构/产品影响:
回滚方式:
建议下一步:
```

不得声称“已验证”但没有实际运行或检查依据。无法运行 Unity 时，要明确写“仅静态审查”。

## 11. 当前推荐任务

除非用户另有明确指令，优先按 `Docs/ROADMAP_V2.md` 推进：

1. 固化 Observe 基线
2. 建立可保存的家园雏形
3. 两名会自主生活的星友
4. 家园愿望连接远征

在上述闭环完成前，不优先开发完整联机、更多星球、复杂角色创建或大规模美术内容。
