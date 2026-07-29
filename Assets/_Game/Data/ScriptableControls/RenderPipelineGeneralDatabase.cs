using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// General render pipeline database for the game.
    /// Contains all URP parameters.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Render Pipeline General Database")]
    public sealed class RenderPipelineGeneralDatabase : ScriptableObject
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
