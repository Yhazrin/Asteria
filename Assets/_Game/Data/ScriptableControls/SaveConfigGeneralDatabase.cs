using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// General save configuration database for the game.
    /// Contains all save parameters.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Save Config General Database")]
    public sealed class SaveConfigGeneralDatabase : ScriptableObject
    {
        [Header("Save")]
        public string saveFileName = "save.json";
        public string saveDirectory = "Saves";
        public int maxSaveSlots = 3;
        public bool autoSave = true;
        public float autoSaveInterval = 300f;

        [Header("Backup")]
        public int maxBackups = 3;
        public string backupExtension = ".bak";
        public bool atomicWrite = true;

        [Header("Schema")]
        public int currentSchemaVersion = 1;
        public bool migrateOnLoad = true;
    }
}
