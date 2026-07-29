using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Master configuration database.
    /// Contains all game configuration parameters.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Config Database")]
    public sealed class ConfigDatabase : ScriptableObject
    {
        [Header("Game")]
        public string gameName = "Asteria";
        public string gameVersion = "0.1.0-alpha";
        public string companyName = "Yhazrin";

        [Header("Paths")]
        public string savePath = "Saves";
        public string logPath = "Logs";
        public string screenshotPath = "Screenshots";

        [Header("Network")]
        public string serverAddress = "localhost";
        public int serverPort = 7777;
        public int maxPlayers = 4;

        [Header("Debug")]
        public bool enableDebugMode = false;
        public bool enableConsoleLog = true;
        public bool enableFileLog = false;
        public string logLevel = "Info"; // Debug, Info, Warning, Error
    }
}
