using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Backup configuration database for the game.
    /// Contains all backup parameters.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Backup Config Database")]
    public sealed class BackupConfigDatabase : ScriptableObject
    {
        [Header("Backup")]
        public bool enableBackup = true;
        public int maxBackups = 5;
        public string backupSuffix = ".bak";
        public string backupDirectory = "Backups";

        [Header("Auto Backup")]
        public bool autoBackup = true;
        public float autoBackupInterval = 3600f; // 1 hour
        public bool backupOnExit = true;
        public bool backupBeforeMigration = true;

        [Header("Cleanup")]
        public bool autoCleanup = true;
        public int maxBackupAge = 30; // days
    }
}
