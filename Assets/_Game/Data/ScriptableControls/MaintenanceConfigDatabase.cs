using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Maintenance configuration database for the game.
    /// Contains all maintenance parameters.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Maintenance Config Database")]
    public sealed class MaintenanceConfigDatabase : ScriptableObject
    {
        [Header("Maintenance")]
        public bool enableMaintenanceMode = false;
        public string maintenanceMessage = "服务器维护中，请稍后再试。";
        public string maintenanceEndTime = "";

        [Header("Cleanup")]
        public bool autoCleanup = true;
        public float cleanupInterval = 3600f; // 1 hour
        public bool cleanupOldLogs = true;
        public bool cleanupOldBackups = true;
        public int maxLogAge = 7; // days
        public int maxBackupAge = 30; // days
    }
}
