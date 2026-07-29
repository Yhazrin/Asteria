using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Quality settings database for the game.
    /// Contains all Unity QualitySettings parameters.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Quality Settings Database")]
    public sealed class QualitySettingsDatabase : ScriptableObject
    {
        [Header("Quality")]
        public int currentQualityLevel = 2;
        public string[] qualityLevelNames = { "Low", "Medium", "High", "Ultra" };

        [Header("Shadows")]
        public ShadowQuality shadowQuality = ShadowQuality.All;
        public ShadowResolution shadowResolution = ShadowResolution.Medium;
        public float shadowDistance = 200f;
        public float shadowStrength = 0.7f;

        [Header("Textures")]
        public int textureQuality = 0; // 0 = Full Res
        public int anisotropicFiltering = 2;

        [Header("Anti-Aliasing")]
        public int antiAliasing = 4; // 0, 2, 4, 8

        [Header("Other")]
        public float lodBias = 1f;
        public int maximumLODLevel = 0;
        public bool pixelLightCount = 4;
    }
}
