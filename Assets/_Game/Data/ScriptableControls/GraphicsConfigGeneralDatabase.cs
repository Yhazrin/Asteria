using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// General graphics configuration database for the game.
    /// Contains all graphics parameters.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Graphics Config General Database")]
    public sealed class GraphicsConfigGeneralDatabase : ScriptableObject
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
