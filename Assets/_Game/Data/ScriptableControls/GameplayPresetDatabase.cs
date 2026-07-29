using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Gameplay preset database for the game.
    /// Contains all gameplay presets (difficulty, etc.).
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Gameplay Preset Database")]
    public sealed class GameplayPresetDatabase : ScriptableObject
    {
        [Header("Presets")]
        public GameplayPreset[] presets = new GameplayPreset[]
        {
            new GameplayPreset
            {
                presetId = "easy",
                displayName = "简单",
                description = "适合休闲玩家",
                pressureMultiplier = 0.5f,
                damageMultiplier = 0.5f,
                resourceMultiplier = 1.5f,
                hintFrequency = "高"
            },
            new GameplayPreset
            {
                presetId = "normal",
                displayName = "普通",
                description = "标准游戏体验",
                pressureMultiplier = 1f,
                damageMultiplier = 1f,
                resourceMultiplier = 1f,
                hintFrequency = "中"
            },
            new GameplayPreset
            {
                presetId = "hard",
                displayName = "困难",
                description = "适合挑战型玩家",
                pressureMultiplier = 1.5f,
                damageMultiplier = 1.5f,
                resourceMultiplier = 0.7f,
                hintFrequency = "低"
            },
        };
    }

    [System.Serializable]
    public class GameplayPreset
    {
        public string presetId;
        public string displayName;
        public string description;
        public float pressureMultiplier;
        public float damageMultiplier;
        public float resourceMultiplier;
        public string hintFrequency;
    }
}
