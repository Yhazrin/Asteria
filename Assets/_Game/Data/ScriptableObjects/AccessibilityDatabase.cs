using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Accessibility configuration database for the game.
    /// Contains all accessibility settings and options.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Accessibility Database")]
    public sealed class AccessibilityDatabase : ScriptableObject
    {
        [Header("Visual")]
        public bool enableColorBlindMode = false;
        public int colorBlindType = 0; // 0=none, 1=protanopia, 2=deuteranopia, 3=tritanopia
        public float uiScale = 1f;
        public bool enableHighContrast = false;
        public bool enableSubtitles = true;

        [Header("Audio")]
        public bool enableVisualCues = false;
        public float masterVolume = 1f;
        public float sfxVolume = 0.8f;
        public float musicVolume = 0.7f;

        [Header("Input")]
        public bool enableAutoAim = false;
        public float inputSensitivity = 1f;
        public bool enableVibration = true;
        public bool enableToggleSprint = false;
    }
}
