using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Export configuration database for the game.
    /// Contains all export parameters.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Export Config Database")]
    public sealed class ExportConfigDatabase : ScriptableObject
    {
        [Header("Export")]
        public bool enableExport = true;
        public string[] exportFormats = { "json", "csv", "xml" };

        [Header("Data")]
        public bool exportSaveData = true;
        public bool exportStatistics = true;
        public bool exportScreenshots = true;

        [Header("Path")]
        public string exportPath = "Exports";
        public string exportFileNameTemplate = "asteria_export_{date}";
    }
}
