using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// General graphics settings database for the game.
    /// Contains all Unity GraphicsSettings parameters.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Graphics Settings General Database")]
    public sealed class GraphicsSettingsGeneralDatabase : ScriptableObject
    {
        [Header("Rendering")]
        public string defaultRenderPipeline = "UniversalRenderPipelineAsset";

        [Header("Settings")]
        public bool logWhenShaderIsCompiled = false;
        public bool allowEnlightenSupportForUpgradedProject = false;
    }
}
