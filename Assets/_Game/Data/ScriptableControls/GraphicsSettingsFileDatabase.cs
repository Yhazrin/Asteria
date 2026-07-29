using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Graphics settings file database for the game.
    /// Contains all Unity GraphicsSettings parameters.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Graphics Settings File Database")]
    public sealed class GraphicsSettingsFileDatabase : ScriptableObject
    {
        [Header("Rendering")]
        public string defaultRenderPipeline = "UniversalRenderPipelineAsset";

        [Header("Settings")]
        public bool logWhenShaderIsCompiled = false;
        public bool allowEnlightenSupportForUpgradedProject = false;
    }
}
