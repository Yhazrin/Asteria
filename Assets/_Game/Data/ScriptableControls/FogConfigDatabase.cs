using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Fog configuration database for the game.
    /// Contains all fog parameters.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Fog Config Database")]
    public sealed class FogConfigDatabase : ScriptableObject
    {
        [Header("Fog")]
        public bool enableFog = true;
        public FogMode fogMode = FogMode.ExponentialSquared;
        public float fogDensityDay = 0.001f;
        public float fogDensityNight = 0.003f;

        [Header("Colors")]
        public Color fogColorDay = new(0.55f, 0.68f, 0.82f);
        public Color fogColorNight = new(0.05f, 0.05f, 0.1f);
        public Color fogColorSunset = new(0.8f, 0.5f, 0.3f);

        [Header("Atmosphere")]
        public float atmosphereDensity = 0.01f;
        public Color atmosphereColorDay = new(0.4f, 0.6f, 0.9f, 0.3f);
        public Color atmosphereColorSunset = new(0.9f, 0.5f, 0.3f, 0.5f);
        public Color atmosphereColorNight = new(0.1f, 0.1f, 0.3f, 0.2f);
    }
}
