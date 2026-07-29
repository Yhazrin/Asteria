using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Save configuration database for the game.
    /// Contains all save parameters.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Save Config Database")]
    public sealed class SaveConfigDatabase : ScriptableObject
    {
        [Header("Save")]
        public string saveFileName = "save.json";
        public string saveDirectory = "Saves";
        public int maxSaveSlots = 3;
        public bool autoSave = true;
        public float autoSaveInterval = 300f; // 5 minutes

        [Header("Backup")]
        public int maxBackups = 3;
        public string backupExtension = ".bak";
        public bool atomicWrite = true;

        [Header("Schema")]
        public int currentSchemaVersion = 1;
        public bool migrateOnLoad = true;
        public bool backupBeforeMigration = true;
    }
}
