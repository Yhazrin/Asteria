using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Game settings database.
    /// Contains all configurable game parameters.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Game Settings Database")]
    public sealed class GameSettingsDatabase : ScriptableObject
    {
        [Header("Player Settings")]
        public float walkSpeed = 8f;
        public float runSpeed = 14f;
        public float jumpSpeed = 7.5f;
        public float mouseSensitivity = 2.4f;
        public float interactionRadius = 3.5f;

        [Header("Planet Settings")]
        public float homePlanetRadius = 180f;
        public float expeditionPlanetRadius = 300f;
        public float gravityStrength = 9.81f;

        [Header("Time Settings")]
        public float secondsPerDay = 720f; // 12 minutes = 1 game day
        public float weatherChangeInterval = 60f;
        public float creatureSpawnInterval = 5f;

        [Header("Save Settings")]
        public int maxBackups = 3;
        public float autoSaveInterval = 300f; // 5 minutes
        public string saveFileName = "save.json";

        [Header("Audio Settings")]
        public float masterVolume = 1f;
        public float musicVolume = 0.7f;
        public float sfxVolume = 0.8f;
        public float ambientVolume = 0.6f;

        [Header("Graphics Settings")]
        public int targetFps = 60;
        public bool enableVSync = true;
        public int qualityLevel = 2;
        public bool enablePostProcessing = true;

        [Header("Debug Settings")]
        public bool enableDebugLog = true;
        public bool showFPS = false;
        public bool godMode = false;
    }
}
