using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Screenshot configuration file database for the game.
    /// Contains all screenshot parameters.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Screenshot Config File Database")]
    public sealed class ScreenshotConfigFileDatabase : ScriptableObject
    {
        [Header("Screenshot")]
        public int superSample = 2;
        public string screenshotFolder = "Screenshots";
        public string filePrefix = "Asteria_";

        [Header("Effects")]
        public bool applyVignette = true;
        public bool applyColorCorrection = false;
        public float saturation = 1f;
        public float contrast = 1f;
        public float brightness = 1f;
    }
}
