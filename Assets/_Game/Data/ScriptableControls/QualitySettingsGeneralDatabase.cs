using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// General quality settings database for the game.
    /// Contains all Unity QualitySettings parameters.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Quality Settings General Database")]
    public sealed class QualitySettingsGeneralDatabase : ScriptableObject
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
        public int textureQuality = 0;
        public int anisotropicFiltering = 2;

        [Header("Anti-Aliasing")]
        public int antiAliasing = 4;

        [Header("LOD")]
        public float lodBias = 1f;
        public int maximumLODLevel = 0;
    }
}
