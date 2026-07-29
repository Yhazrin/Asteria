using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// General achievement configuration database for the game.
    /// Contains all achievement parameters.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Achievement Config General Database")]
    public sealed class AchievementConfigGeneralDatabase : ScriptableObject
    {
        [Header("Achievements")]
        public int totalAchievements = 15;

        [Header("Categories")]
        public string[] categories = {
            "discovery",
            "expedition",
            "social",
            "building",
            "survival",
            "photo",
            "codex",
            "cooperate",
            "residents",
            "tools",
            "time"
        };

        [Header("Notifications")]
        public float notificationDuration = 5f;
        public float notificationFadeSpeed = 2f;
    }
}
