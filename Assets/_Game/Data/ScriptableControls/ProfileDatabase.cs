using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Player profile database for the game.
    /// Contains all player profile data.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Profile Database")]
    public sealed class ProfileDatabase : ScriptableObject
    {
        [Header("Profile")]
        public string profileId = "default";
        public string playerName = "Explorer";
        public string avatarId = "default";

        [Header("Progress")]
        public int totalDiscoveries = 0;
        public int totalExpeditions = 0;
        public int totalPlayTime = 0; // seconds

        [Header("Statistics")]
        public int totalResidents = 0;
        public int totalFacilities = 0;
        public int totalPhotos = 0;
        public int totalRescues = 0;

        [Header("Settings")]
        public string language = "zh-CN";
        public string inputPreset = "default";
        public string graphicsPreset = "medium";
        public string audioPreset = "default";
        public string gameplayPreset = "normal";
    }
}
