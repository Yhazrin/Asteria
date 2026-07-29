using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Shadow file database for the game.
    /// Contains all shadow parameters.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Shadow File Database")]
    public sealed class ShadowFileDatabase : ScriptableObject
    {
        [Header("Shadows")]
        public ShadowQuality shadowQuality = ShadowQuality.All;
        public ShadowResolution shadowResolution = ShadowResolution.Medium;
        public float shadowDistance = 200f;
        public float shadowStrength = 0.7f;
        public int shadowCascadeCount = 4;

        [Header("Shadow Bias")]
        public float depthBias = 0.05f;
        public float normalBias = 0.4f;
        public float shadowNearPlane = 0.2f;
    }
}
