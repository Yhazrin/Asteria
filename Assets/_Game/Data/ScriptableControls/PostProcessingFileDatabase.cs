using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Post-processing file database for the game.
    /// Contains all post-processing parameters.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Post Processing File Database")]
    public sealed class PostProcessingFileDatabase : ScriptableObject
    {
        [Header("Bloom")]
        public bool enableBloom = true;
        public float bloomIntensity = 0.3f;
        public float bloomThreshold = 0.8f;
        public float bloomSoftness = 0.5f;

        [Header("Vignette")]
        public bool enableVignette = true;
        public float vignetteIntensity = 0.3f;
        public float vignetteSmoothness = 0.3f;

        [Header("Color Grading")]
        public bool enableColorGrading = true;
        public float saturation = 1.1f;
        public float contrast = 1.05f;
        public float brightness = 1f;
    }
}
