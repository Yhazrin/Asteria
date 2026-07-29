using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Session database for the game.
    /// Contains all session data.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Session Database")]
    public sealed class SessionDatabase : ScriptableObject
    {
        [Header("Session")]
        public string sessionId = "";
        public float sessionStartTime = 0f;
        public float sessionDuration = 0f;
        public int sessionDiscoveries = 0;
        public int sessionExpeditions = 0;

        [Header("Current State")]
        public string currentScene = "";
        public string currentBiome = "";
        public string currentWeather = "clear";
        public int currentDay = 1;
        public float currentTimeOfDay = 0.5f;

        [Header("Multiplayer")]
        public bool isHosting = false;
        public bool isConnected = false;
        public int playerCount = 1;
        public string hostId = "";
    }
}
