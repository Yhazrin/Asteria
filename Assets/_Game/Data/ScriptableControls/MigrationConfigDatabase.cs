using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Migration configuration database for the game.
    /// Contains all migration parameters.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Migration Config Database")]
    public sealed class MigrationConfigDatabase : ScriptableObject
    {
        [Header("Migration")]
        public int currentSchemaVersion = 1;
        public int maxSupportedVersion = 10;
        public bool requireBackup = true;
        public bool autoMigrate = true;

        [Header("Backup")]
        public int maxBackups = 3;
        public string backupSuffix = ".bak";
        public bool backupBeforeMigration = true;

        [Header("Validation")]
        public bool validateAfterMigration = true;
        public bool rollbackOnFailure = true;
    }
}
