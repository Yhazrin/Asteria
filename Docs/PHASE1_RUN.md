# Asteria — 如何运行（当前切片）

## 打开

```bash
unity open /Users/yanghaoze/Desktop/PROJECT/Asteria
```

或用 Unity Hub / Editor **6000.5.5f1** 打开工程。

场景：

```
Assets/_Game/Planet/Scenes/SphereMoveDemo.unity
```

按 **Play**。

## 操作

| 输入 | 作用 |
|------|------|
| WASD | 球面移动 |
| Shift | 奔跑 |
| Space | 跳跃 |
| 鼠标 | 视角 |
| **E** | **观察兴趣点（亮色风铃石）** |
| Esc | 释放鼠标 |
| 左键 | 重新锁定鼠标 |

## 你要看到什么

1. **更圆的星球**：96×64 高分段网格 + 地表纹理（苔绿 / 暖灰 / 薄雾色）。
2. **更密的世界**：约 140 个岩石/植被装饰物散布在球面。
3. **好找的目标**：出生点附近有一串黄色信标柱；全图约 **8 个** 发光「风铃石」观察点。
4. 走近亮色石头，按 **E** → 图鉴记录 +1。

## 用 Unity CLI 重建视觉 / 散布（已验证）

先关闭占用该工程的 Editor，再执行：

```bash
cd /Users/yanghaoze/Desktop/PROJECT/Asteria
unity --no-banner run . --timeout 600 -- \
  -nographics \
  -executeMethod Asteria.Editor.Phase1PlanetDressingUpgrade.RunFromBatch \
  -logFile Logs/phase1_planet_dressing.log \
  -accept-apiupdate
```

菜单等价项：`Asteria → Upgrade Planet Visuals And Scatter`

## 本阶段有 / 没有

**有：** 圆滑星球、纹理、散布物、多观察点、信标路径、球面移动、HUD  

**没有：** 联机、生存、建造、战斗
