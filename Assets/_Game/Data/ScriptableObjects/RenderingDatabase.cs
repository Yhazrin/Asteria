using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Rendering configuration database.
    /// Contains all rendering parameters for the game.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Rendering Database")]
    public sealed class RenderingDatabase : ScriptableObject
    {
        [Header("Post Processing")]
        public float saturation = 1.1f;
        public float contrast = 1.05f;
        public float brightness = 1f;
        public float bloomIntensity = 0.3f;
        public float vignetteIntensity = 0.3f;

        [Header("Atmosphere")]
        public float atmosphereDensity = 0.01f;
        public Color atmosphereColorDay = new(0.4f, 0.6f, 0.9f, 0.3f);
        public Color atmosphereColorSunset = new(0.9f, 0.5f, 0.3f, 0.5f);
        public Color atmosphereColorNight = new(0.1f, 0.1f, 0.3f, 0.2f);

        [Header("Fog")]
        public float fogDensityDay = 0.001f;
        public float fogDensityNight = 0.003f;
        public Color fogColorDay = new(0.55f, 0.68f, 0.82f);
        public Color fogColorNight = new(0.05f, 0.05f, 0.1f);

        [Header("LOD")]
        public int maxLODLevels = 4;
        public float[] lodDistances = { 100f, 200f, 400f, 800f };
        public float lodBias = 1f;
    }
}
