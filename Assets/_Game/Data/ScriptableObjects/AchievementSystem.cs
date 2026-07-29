using System;
using System.Collections.Generic;
using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Achievement system tracking player accomplishments.
    /// Provides goals and rewards for exploration and social activities.
    /// </summary>
    public sealed class AchievementSystem : MonoBehaviour
    {
        static AchievementSystem _instance;

        [Header("Achievements")]
        [SerializeField] AchievementDefinition[] allAchievements;

        readonly Dictionary<string, AchievementDefinition> _achievements = new();
        readonly Dictionary<string, AchievementProgress> _progress = new();
        readonly HashSet<string> _unlocked = new();

        public static AchievementSystem Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindFirstObjectByType<AchievementSystem>();
                    if (_instance == null)
                    {
                        var go = new GameObject("AchievementSystem");
                        _instance = go.AddComponent<AchievementSystem>();
                    }
                }
                return _instance;
            }
        }

        // Events
        public event Action<AchievementDefinition> OnAchievementUnlocked;
        public event Action<string, float> OnProgressUpdated;

        void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }
            _instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeAchievements();
        }

        void InitializeAchievements()
        {
            if (allAchievements != null)
            {
                foreach (var achievement in allAchievements)
                {
                    if (achievement != null)
                    {
                        _achievements[achievement.achievementId] = achievement;
                        _progress[achievement.achievementId] = new AchievementProgress
                        {
                            achievementId = achievement.achievementId,
                            current = 0,
                            target = achievement.targetValue
                        };
                    }
                }
            }

            // Create default achievements if none configured
            if (_achievements.Count == 0)
            {
                CreateDefaultAchievements();
            }
        }

        void CreateDefaultAchievements()
        {
            var defaults = new[]
            {
                CreateAchievement("first_discovery", "初次发现", "完成第一次观察", 1, "observe"),
                CreateAchievement("explorer_10", "探索者", "观察10个兴趣点", 10, "observe"),
                CreateAchievement("explorer_50", "资深探索者", "观察50个兴趣点", 50, "observe"),
                CreateAchievement("first_expedition", "远征者", "完成第一次远征", 1, "expedition"),
                CreateAchievement("expedition_10", "远征专家", "完成10次远征", 10, "expedition"),
                CreateAchievement("social_butterfly", "社交蝴蝶", "与5个居民互动", 5, "social"),
                CreateAchievement("best_friend", "最好的朋友", "与一个居民达到最大亲密度", 1, "relationship"),
                CreateAchievement("home_builder", "建造者", "在家园建造3个设施", 3, "building"),
                CreateAchievement("weather_survivor", "天气幸存者", "在暴风中存活", 1, "survival"),
                CreateAchievement("photographer", "摄影师", "拍摄10张照片", 10, "photo"),
                CreateAchievement("codex_complete", "图鉴大师", "收集所有星球类型", 7, "codex"),
                CreateAchievement("cooperator", "合作者", "完成一次合作交互", 1, "cooperate"),
                CreateAchievement("resident_6", "社区领袖", "家园有6个居民", 6, "residents"),
                CreateAchievement("tool_master", "工具大师", "使用所有工具类型", 6, "tools"),
                CreateAchievement("night_owl", "夜猫子", "在夜晚探索", 1, "time"),
            };

            foreach (var achievement in defaults)
            {
                _achievements[achievement.achievementId] = achievement;
                _progress[achievement.achievementId] = new AchievementProgress
                {
                    achievementId = achievement.achievementId,
                    current = 0,
                    target = achievement.targetValue
                };
            }
        }

        AchievementDefinition CreateAchievement(string id, string name, string desc, int target, string category)
        {
            var achievement = ScriptableObject.CreateInstance<AchievementDefinition>();
            achievement.achievementId = id;
            achievement.displayName = name;
            achievement.description = desc;
            achievement.targetValue = target;
            achievement.category = category;
            achievement.rewardType = "none";
            achievement.rewardValue = 0;
            return achievement;
        }

        /// <summary>
        /// Report progress toward an achievement.
        /// </summary>
        public void ReportProgress(string category, int amount = 1)
        {
            foreach (var kvp in _progress)
            {
                if (_achievements.TryGetValue(kvp.Key, out var achievement))
                {
                    if (achievement.category == category && !_unlocked.Contains(kvp.Key))
                    {
                        kvp.Value.current += amount;
                        OnProgressUpdated?.Invoke(kvp.Key, (float)kvp.Value.current / kvp.Value.target);

                        if (kvp.Value.current >= kvp.Value.target)
                        {
                            Unlock(achievement);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Unlock an achievement directly.
        /// </summary>
        public void Unlock(AchievementDefinition achievement)
        {
            if (achievement == null) return;
            if (_unlocked.Contains(achievement.achievementId)) return;

            _unlocked.Add(achievement.achievementId);
            OnAchievementUnlocked?.Invoke(achievement);

            Debug.Log($"[Achievement] Unlocked: {achievement.displayName} - {achievement.description}");
        }

        /// <summary>
        /// Check if an achievement is unlocked.
        /// </summary>
        public bool IsUnlocked(string achievementId)
        {
            return _unlocked.Contains(achievementId);
        }

        /// <summary>
        /// Get progress for an achievement.
        /// </summary>
        public float GetProgress(string achievementId)
        {
            if (!_progress.TryGetValue(achievementId, out var progress)) return 0f;
            return (float)progress.current / progress.target;
        }

        /// <summary>
        /// Get all achievements.
        /// </summary>
        public IReadOnlyList<AchievementDefinition> GetAllAchievements()
        {
            return new List<AchievementDefinition>(_achievements.Values);
        }

        /// <summary>
        /// Get unlocked achievements.
        /// </summary>
        public List<AchievementDefinition> GetUnlocked()
        {
            var result = new List<AchievementDefinition>();
            foreach (var id in _unlocked)
            {
                if (_achievements.TryGetValue(id, out var achievement))
                    result.Add(achievement);
            }
            return result;
        }

        public struct AchievementProgress
        {
            public string achievementId;
            public int current;
            public int target;
        }
    }

    [CreateAssetMenu(menuName = "Asteria/Achievement Definition")]
    public sealed class AchievementDefinition : ScriptableObject
    {
        public string achievementId;
        public string displayName;
        [TextArea(1, 3)] public string description;
        public int targetValue = 1;
        public string category;
        public string rewardType;
        public int rewardValue;
        public Sprite icon;
    }
}
