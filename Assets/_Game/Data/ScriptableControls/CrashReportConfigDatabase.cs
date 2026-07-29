using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Crash report configuration database for the game.
    /// Contains all crash report parameters.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Crash Report Config Database")]
    public sealed class CrashReportConfigDatabase : ScriptableObject
    {
        [Header("Crash Report")]
        public bool enableCrashReport = true;
        public string crashReportEndpoint = "";
        public bool includeSaveData = true;
        public bool includeSystemInfo = true;

        [Header("Logs")]
        public int maxLogLines = 1000;
        public bool includeStackTrace = true;
        public bool includeUnityVersion = true;
    }
}
