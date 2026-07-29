using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Shadow configuration database for the game.
    /// Contains all shadow parameters.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Shadow Database")]
    public sealed class ShadowDatabase : ScriptableObject
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

        [Header("Contact Shadows")]
        public bool enableContactShadows = false;
        public float contactShadowLength = 0.1f;
    }
}
