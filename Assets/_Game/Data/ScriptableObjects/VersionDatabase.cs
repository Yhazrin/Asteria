using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Version tracking database for the game.
    /// Tracks build version, content version, and save schema version.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Version Database")]
    public sealed class VersionDatabase : ScriptableObject
    {
        [Header("Build Version")]
        public string buildVersion = "0.1.0-alpha";
        public string buildDate = "2026-07-28";
        public string buildNumber = "1";

        [Header("Content Version")]
        public string contentVersion = "0.1.0";
        public int contentRevision = 1;

        [Header("Save Schema")]
        public int saveSchemaVersion = 1;
        public string saveSchemaDescription = "Initial schema with discoveries, residents, and expedition history";

        [Header("API Version")]
        public int apiVersion = 1;
        public string apiDescription = "Initial API with ISessionAuthority and IDiscoveryRepository";

        /// <summary>
        /// Get full version string.
        /// </summary>
        public string GetFullVersion()
        {
            return $"{buildVersion} (build {buildNumber}, {buildDate})";
        }

        /// <summary>
        /// Check if save migration is needed.
        /// </summary>
        public bool NeedsSaveMigration(int currentSchemaVersion)
        {
            return currentSchemaVersion < saveSchemaVersion;
        }
    }
}
