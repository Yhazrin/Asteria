# Editor 工具幂等性规范

> 所有 Asteria Editor 工具必须尽可能幂等：重复运行不应重复生成内容。

## 幂等性规则

### 1. Phase1Bootstrap

**菜单路径：** `Asteria/Setup Phase 1 Demo`

**幂等性保证：**
- 使用 `EnsureFolder()` 检查目录是否存在，存在则跳过
- 使用 `AssetDatabase.LoadAssetAtPath()` 检查资产是否存在
- 已存在的 Material 只更新 shader 和颜色，不重新创建
- 场景使用 `EditorSceneManager.SaveScene()` 覆盖，不创建重复场景
- Prefab 使用 `PrefabUtility.SaveAsPrefabAsset()` 覆盖

**重复运行行为：**
- 第一次：创建所有目录、材质、场景、Prefab
- 后续运行：更新现有材质的 shader，重建场景内容

### 2. Phase1AutoSetup

**菜单路径：** 自动执行（首次导入时）

**幂等性保证：**
- 使用 `EditorPrefs` 标记 `Asteria.Phase1Setup.Completed.v2`
- 标记为 true 后不再执行
- 可通过 `Asteria/Reset Phase 1 Auto Setup Flag` 重置

**重复运行行为：**
- 第一次：运行 Phase1Bootstrap
- 后续运行：检查标记，跳过

### 3. Phase1PlanetDressingUpgrade

**菜单路径：** `Asteria/Upgrade Planet Visuals And Scatter`

**幂等性保证：**
- 先清除旧散布对象
- 使用种子控制随机，相同种子产生相同结果
- 检查场景中是否已有目标对象

**重复运行行为：**
- 每次运行：清除旧对象，重建新对象（相同种子 = 相同布局）

### 4. Phase1ObserveUpgrade / Phase1ObserveAutoUpgrade

**幂等性保证：**
- 检查场景中是否已有 ObserveInteractable
- 使用固定位置和方向
- 已存在的 POI 不重复创建

## 验证方法

每个工具应通过以下方式验证幂等性：

1. 运行工具一次，记录场景状态
2. 运行工具第二次，比较场景状态
3. 确认没有重复对象、重复组件或重复资产
4. 确认 Console 无 Error 和重复警告

## batch mode 验证

```bash
# 第一次运行
unity -nographics -executeMethod Asteria.Editor.Phase1Bootstrap.RunFromBatch -logFile Logs/first.log

# 第二次运行（应幂等）
unity -nographics -executeMethod Asteria.Editor.Phase1Bootstrap.RunFromBatch -logFile Logs/second.log

# 比较日志确认无重复创建
```
