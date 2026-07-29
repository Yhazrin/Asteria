using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Day/night cycle configuration database for the game.
    /// Contains all day/night parameters.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Day Night Config Database")]
    public sealed class DayNightConfigDatabase : ScriptableObject
    {
        [Header("Day/Night Cycle")]
        public float secondsPerDay = 720f; // 12 minutes = 1 game day
        public float sunRotationSpeed = 0.1f;

        [Header("Sun")]
        public float sunIntensityDay = 1.2f;
        public float sunIntensityNight = 0.1f;
        public Color sunColorDay = new(1f, 0.95f, 0.85f);
        public Color sunColorSunset = new(1f, 0.6f, 0.3f);
        public Color sunColorNight = new(0.2f, 0.2f, 0.4f);

        [Header("Ambient")]
        public Color ambientDay = new(0.55f, 0.7f, 0.9f);
        public Color ambientSunset = new(0.8f, 0.5f, 0.3f);
        public Color ambientNight = new(0.1f, 0.1f, 0.2f);

        [Header("Stars")]
        public float starsAlphaDay = 0f;
        public float starsAlphaNight = 0.8f;
    }
}
