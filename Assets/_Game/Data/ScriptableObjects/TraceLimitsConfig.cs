using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Placeholder limits for the future traces system (Phase 2+).
    /// </summary>
    [CreateAssetMenu(fileName = "TraceLimitsConfig", menuName = "Asteria/Config/Trace Limits")]
    public sealed class TraceLimitsConfig : ScriptableObject
    {
        public int maxCampLights = 5;
        public int maxWaymarks = 8;
        public int maxPhotos = 20;
        public float defaultDecaySeconds = 600f;
    }
}
