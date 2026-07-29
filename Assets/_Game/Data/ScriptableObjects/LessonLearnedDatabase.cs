using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Lessons learned database for the game.
    /// Contains all lessons learned during development.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Lesson Learned Database")]
    public sealed class LessonLearnedDatabase : ScriptableObject
    {
        [Header("Lessons")]
        public LessonEntry[] lessons = new LessonEntry[]
        {
            new LessonEntry
            {
                lessonId = "ll_asmdef",
                category = "architecture",
                description = "使用 asmdef 分离模块可以避免循环依赖",
                impact = "high",
                appliedIn = "Asteria.Core, Asteria.GameFlow 分离"
            },
            new LessonEntry
            {
                lessonId = "ll_procedural_assets",
                category = "art",
                description = "程序化资产可以快速填补视觉空白",
                impact = "medium",
                appliedIn = "ProceduralAssets, MaterialHelper"
            },
            new LessonEntry
            {
                lessonId = "ll_save_system",
                category = "persistence",
                description = "原子写入和多级备份是存档系统的基本要求",
                impact = "high",
                appliedIn = "SaveService"
            },
            new LessonEntry
            {
                lessonId = "ll_singleton_pattern",
                category = "architecture",
                description = "单例模式需要统一实现，避免不同变体",
                impact = "medium",
                appliedIn = "GameBootstrap, DiscoveryJournal, GameHud"
            },
            new LessonEntry
            {
                lessonId = "ll_test_driven",
                category = "testing",
                description = "测试应该与功能同步编写，不是事后补充",
                impact = "high",
                appliedIn = "All milestone tests"
            },
        };
    }

    [System.Serializable]
    public class LessonEntry
    {
        public string lessonId;
        public string category;
        public string description;
        public string impact; // "low", "medium", "high"
        public string appliedIn;
    }
}
