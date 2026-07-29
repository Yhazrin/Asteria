using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Update configuration database for the game.
    /// Contains all update parameters.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Update Config Database")]
    public sealed class UpdateConfigDatabase : ScriptableObject
    {
        [Header("Update")]
        public bool checkForUpdates = true;
        public string updateEndpoint = "";
        public float checkInterval = 3600f; // 1 hour

        [Header("Auto Update")]
        public bool autoDownload = false;
        public bool autoInstall = false;
        public bool requireConfirmation = true;

        [Header("Channel")]
        public string updateChannel = "stable"; // stable, beta, alpha
    }
}
