using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Render pipeline file database for the game.
    /// Contains all URP parameters.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Render Pipeline File Database")]
    public sealed class RenderPipelineFileDatabase : ScriptableObject
    {
        [Header("URP")]
        public string urpAssetPath = "Assets/_Game/Core/Settings/Asteria_URP.asset";
        public string urpRendererPath = "Assets/_Game/Core/Settings/Asteria_URP_Renderer.asset";

        [Header("Rendering")]
        public bool enableSRPBatcher = true;
        public bool enableDynamicBatching = false;
        public bool enableGPUInstancing = true;

        [Header("Shadows")]
        public int mainLightShadowResolution = 2048;
        public int mainLightShadowCascadeCount = 4;
        public float shadowDistance = 200f;

        [Header("Post Processing")]
        public bool enablePostProcessing = true;
    }
}
