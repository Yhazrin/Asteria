using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Best practice database for the game.
    /// Contains all development best practices.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Best Practice Database")]
    public sealed class BestPracticeDatabase : ScriptableObject
    {
        [Header("Practices")]
        public BestPractice[] practices = new BestPractice[]
        {
            new BestPractice
            {
                practiceId = "bp_asmdef",
                category = "architecture",
                description = "使用 asmdef 分离模块",
                rationale = "避免循环依赖，加速编译",
                examples = new[] { "Asteria.Core", "Asteria.Data", "Asteria.Interaction" }
            },
            new BestPractice
            {
                practiceId = "bp_save_system",
                category = "persistence",
                description = "使用纯 C# DTO 存档",
                rationale = "避免序列化 Unity 对象，支持迁移",
                examples = new[] { "SaveRoot", "ResidentStateDTO" }
            },
            new BestPractice
            {
                practiceId = "bp_test_driven",
                category = "testing",
                description = "测试与功能同步编写",
                rationale = "确保功能正确性，支持回归",
                examples = new[] { "PlanetBodyTests", "SaveServiceTests" }
            },
            new BestPractice
            {
                practiceId = "bp_constants",
                category = "code",
                description = "使用常量类避免硬编码",
                rationale = "统一管理魔法数字和字符串",
                examples = new[] { "AsteriaConstants" }
            },
            new BestPractice
            {
                practiceId = "bp_material_helper",
                category = "art",
                description = "使用 MaterialHelper 创建材质",
                rationale = "避免重复代码，确保风格一致",
                examples = new[] { "MaterialHelper.CreateSimpleMaterial" }
            },
        };
    }

    [System.Serializable]
    public class BestPractice
    {
        public string practiceId;
        public string category;
        public string description;
        public string rationale;
        public string[] examples;
    }
}
