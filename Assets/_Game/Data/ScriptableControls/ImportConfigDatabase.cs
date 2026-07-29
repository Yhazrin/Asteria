using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Import configuration database for the game.
    /// Contains all import parameters.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Import Config Database")]
    public sealed class ImportConfigDatabase : ScriptableObject
    {
        [Header("Import")]
        public bool enableImport = true;
        public string[] supportedFormats = { "json", "csv" };

        [Header("Validation")]
        public bool validateOnImport = true;
        public bool backupBeforeImport = true;
        public bool overwriteExisting = false;

        [Header("Path")]
        public string importPath = "Imports";
    }
}
