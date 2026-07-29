using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Graphics configuration file database for the game.
    /// Contains all graphics parameters.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Graphics Config File Database")]
    public sealed class GraphicsConfigFileDatabase : ScriptableObject
    {
        [Header("Quality")]
        public int qualityLevel = 2;
        public int targetFrameRate = 60;
        public bool enableVSync = true;

        [Header("Rendering")]
        public bool enablePostProcessing = true;
        public bool enableShadows = true;
        public bool enableLOD = true;

        [Header("Resolution")]
        public int defaultWidth = 1920;
        public int defaultHeight = 1080;
        public bool fullscreen = true;

        [Header("Effects")]
        public bool enableBloom = true;
        public bool enableVignette = true;
        public bool enableColorGrading = true;
    }
}
