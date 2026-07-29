using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Compatibility rules database.
    /// Contains all compatibility checks for the game.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Compatibility Database")]
    public sealed class CompatibilityDatabase : ScriptableObject
    {
        [Header("Unity Version")]
        public string requiredUnityVersion = "6000.5.5f1";
        public string[] supportedUnityVersions = { "6000.5.5f1", "6000.5.0f1" };

        [Header("Packages")]
        public string requiredURPVersion = "17.5.0";
        public string requiredNetcodeVersion = "2.0.0";

        [Header("Platform")]
        public string[] supportedPlatforms = { "Windows", "macOS", "Linux" };
        public int minRamGB = 8;
        public int minVramGB = 2;

        [Header("Save Compatibility")]
        public int minSaveVersion = 1;
        public int maxSaveVersion = 10;
        public bool requireBackupBeforeMigration = true;
    }
}
