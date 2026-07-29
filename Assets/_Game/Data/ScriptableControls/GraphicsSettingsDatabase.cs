using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Graphics settings database for the game.
    /// Contains all Unity GraphicsSettings parameters.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Graphics Settings Database")]
    public sealed class GraphicsSettingsDatabase : ScriptableObject
    {
        [Header("Rendering")]
        public string defaultRenderPipeline = "UniversalRenderPipelineAsset";
        public string[] allConfiguredRenderPipelines = { "UniversalRenderPipelineAsset" };

        [Header("Transparency")]
        public bool transparencySortMode = false; // Default
        public float transparencySortAxisX = 0f;
        public float transparencySortAxisY = 0f;
        public float transparencySortAxisZ = 1f;

        [Header("Other")]
        public bool logWhenShaderIsCompiled = false;
        public bool allowEnlightenSupportForUpgradedProject = false;
    }
}
