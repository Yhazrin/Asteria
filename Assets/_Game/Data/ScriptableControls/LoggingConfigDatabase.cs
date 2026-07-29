using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Logging configuration database for the game.
    /// Contains all logging parameters.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Logging Config Database")]
    public sealed class LoggingConfigDatabase : ScriptableObject
    {
        [Header("Log Level")]
        public bool enableDebug = true;
        public bool enableInfo = true;
        public bool enableWarning = true;
        public bool enableError = true;

        [Header("Output")]
        public bool logToConsole = true;
        public bool logToFile = false;
        public string logFilePath = "Logs/asteria.log";
        public int maxLogFileSize = 10; // MB

        [Header("Categories")]
        public bool logGameplay = true;
        public bool logNetwork = true;
        public bool logAudio = true;
        public bool logUI = true;
        public bool logPhysics = true;
    }
}
