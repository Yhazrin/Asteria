# Asteria — 编辑器工具规范

> 状态：Active
> 目标：定义编辑器工具的设计原则、命名规范和使用方式，确保工具可重复运行且不破坏项目。

## 1. 设计原则

- 所有 Editor 工具必须尽可能幂等：重复运行不应重复生成内容。
- 工具必须在菜单中可访问，路径统一在 `Asteria/` 下。
- 工具必须支持 batch mode 运行（`-executeMethod`）。
- 工具不得修改运行时代码逻辑。
- 工具生成的资产必须有唯一标识，避免重复。
- 工具必须在日志中输出操作摘要。

## 2. 现有工具

### 2.1 Phase1Bootstrap

**菜单路径：** `Asteria/Setup/Phase 1 Bootstrap`

**功能：**
- 确保 SphereMoveDemo 场景存在且结构正确
- 创建 Planet、Player、Camera、HUD 等基础对象
- 设置 URP 渲染管线

**幂等性：**
- 检查对象是否已存在，存在则跳过
- 使用 `FindOrCreate` 模式

**Batch Mode：**
```bash
unity -nographics -executeMethod Asteria.Editor.Phase1Bootstrap.RunFromBatch
```

### 2.2 Phase1PlanetDressingUpgrade

**菜单路径：** `Asteria/Upgrade Planet Visuals And Scatter`

**功能：**
- 重建球面装饰散布
- 更新地表材质
- 生成信标路径

**幂等性：**
- 先清除旧散布对象
- 使用种子控制随机，相同种子产生相同结果

**Batch Mode：**
```bash
unity -nographics -executeMethod Asteria.Editor.Phase1PlanetDressingUpgrade.RunFromBatch
```

## 3. 工具命名规范

### 3.1 菜单路径

```text
Asteria/
├── Setup/              # 初始化和场景搭建
│   ├── Phase 1 Bootstrap
│   ├── Phase 2 Bootstrap
│   └── ...
├── Upgrade/            # 升级和重建
│   ├── Planet Visuals And Scatter
│   ├── Upgrade Observe POIs
│   └── ...
├── Validate/           # 验证和检查
│   ├── Validate Save Schema
│   ├── Validate IDs
│   └── ...
├── Generate/           # 生成内容
│   ├── Generate Test Data
│   ├── Generate Resident
│   └── ...
└── Debug/              # 调试工具
    ├── Debug Resident State
    ├── Debug Event Director
    └── ...
```

### 3.2 类命名

```text
{功能描述}EditorTool    // 如 Phase1BootstrapEditorTool
{功能描述}Upgrade       // 如 PlanetDressingUpgrade
{功能描述}Validator     // 如 SaveSchemaValidator
{功能描述}Generator     // 如 TestDataGenerator
```

### 3.3 文件位置

```text
Assets/_Game/Editor/
├── Setup/
│   ├── Phase1Bootstrap.cs
│   └── Phase1AutoSetup.cs
├── Upgrade/
│   └── Phase1PlanetDressingUpgrade.cs
├── Validators/
│   ├── SaveSchemaValidator.cs
│   └── IdFormatValidator.cs
├── Generators/
│   ├── TestDataGenerator.cs
│   └── ResidentGenerator.cs
└── Debuggers/
    ├── ResidentStateDebugger.cs
    └── EventDirectorDebugger.cs
```

## 4. 工具模板

### 4.1 基础工具模板

```csharp
#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Asteria.Editor
{
    public static class ExampleTool
    {
        [MenuItem("Asteria/Setup/Example Tool")]
        public static void RunFromMenu()
        {
            if (!EditorUtility.DisplayDialog(
                "Example Tool",
                "This will do X. Continue?",
                "Yes", "Cancel"))
                return;

            Execute();
        }

        /// <summary>
        /// Batch mode entry point.
        /// Usage: unity -nographics -executeMethod Asteria.Editor.ExampleTool.RunFromBatch
        /// </summary>
        public static void RunFromBatch()
        {
            Execute();
        }

        private static void Execute()
        {
            Debug.Log("[ExampleTool] Starting...");

            // Check if already done (idempotency)
            if (IsAlreadyDone())
            {
                Debug.Log("[ExampleTool] Already done, skipping.");
                return;
            }

            // Do work
            int created = DoWork();

            Debug.Log($"[ExampleTool] Complete. Created {created} objects.");
        }

        private static bool IsAlreadyDone()
        {
            // Check for existing objects
            return false;
        }

        private static int DoWork()
        {
            // Create objects
            return 0;
        }
    }
}
#endif
```

### 4.2 带进度条的工具模板

```csharp
#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Asteria.Editor
{
    public static class ExampleProgressTool
    {
        [MenuItem("Asteria/Generate/Example Progress Tool")]
        public static void RunFromMenu()
        {
            try
            {
                EditorUtility.DisplayProgressBar("Example Tool", "Starting...", 0f);
                Execute(progress =>
                {
                    EditorUtility.DisplayProgressBar("Example Tool", progress.message, progress.percent);
                });
            }
            finally
            {
                EditorUtility.ClearProgressBar();
            }
        }

        public static void RunFromBatch()
        {
            Execute(_ => { });
        }

        private static void Execute(System.Action<(string message, float percent)> onProgress)
        {
            onProgress(("Step 1...", 0.25f));
            // Do step 1

            onProgress(("Step 2...", 0.50f));
            // Do step 2

            onProgress(("Step 3...", 0.75f));
            // Do step 3

            onProgress(("Complete!", 1f));
        }
    }
}
#endif
```

## 5. 验证工具

### 5.1 SaveSchemaValidator

**菜单路径：** `Asteria/Validate/Save Schema`

**功能：**
- 验证当前存档的 schemaVersion
- 检查所有 ID 格式
- 检查引用完整性
- 报告警告和错误

### 5.2 IdFormatValidator

**菜单路径：** `Asteria/Validate/ID Format`

**功能：**
- 扫描所有 ScriptableObject 资产
- 验证 ID 符合 `{namespace}.{type}.{slug}` 格式
- 检查 ID 唯一性
- 报告缺失或重复的 ID

## 6. 生成工具

### 6.1 TestDataGenerator

**菜单路径：** `Asteria/Generate/Test Data`

**功能：**
- 生成测试用居民定义
- 生成测试用事件定义
- 生成测试用存档样本

**幂等性：**
- 使用固定种子
- 输出到 `Assets/_Game/Tests/Fixtures/`

### 6.2 ResidentGenerator

**菜单路径：** `Asteria/Generate/Resident`

**功能：**
- 交互式创建新居民定义
- 自动分配稳定 ID
- 创建对应 ScriptableObject 资产

## 7. 调试工具

### 7.1 ResidentStateDebugger

**菜单路径：** `Asteria/Debug/Resident State`

**功能：**
- 运行时显示选中居民的完整状态
- 性格、需求、关系、日程、记忆
- 支持修改运行时值（仅调试用）

### 7.2 EventDirectorDebugger

**菜单路径：** `Asteria/Debug/Event Director`

**功能：**
- 显示当前事件导演状态
- 候选事件列表及评分
- 冷却状态
- 阶段转换条件
- 手动触发事件（仅调试用）

## 8. Batch Mode 规范

### 8.1 命令行参数

```bash
# 基础运行
unity -nographics -executeMethod {Namespace}.{ClassName}.{MethodName}

# 带日志
unity -nographics -executeMethod {Namespace}.{ClassName}.{MethodName} -logFile Logs/{tool_name}.log

# 带超时
unity -noGraphics -executeMethod {Namespace}.{ClassName}.{MethodName} -timeout 600
```

### 8.2 退出码

- 0：成功
- 1：有警告但完成
- 2：有错误
- 3：超时

### 8.3 日志格式

```text
[{ToolName}] Starting...
[{ToolName}] Step 1: ...
[{ToolName}] WARNING: ...
[{ToolName}] ERROR: ...
[{ToolName}] Complete. Created X objects, Y warnings, Z errors.
```

## 9. 工具测试

每个 Editor 工具应有对应的 EditMode 测试：

- 验证幂等性：运行两次结果相同
- 验证 batch mode 入口可调用
- 验证输出资产符合预期

## 10. 工具文档

每个工具的脚本文件顶部必须包含：

```csharp
/// <summary>
/// {一句话描述}
/// </summary>
/// <remarks>
/// Menu: Asteria/{Category}/{Name}
/// Batch: unity -nographics -executeMethod {FullName}.RunFromBatch
/// Idempotent: {Yes/No}
/// </remarks>
```
