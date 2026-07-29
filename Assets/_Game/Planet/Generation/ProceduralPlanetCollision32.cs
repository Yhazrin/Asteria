using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Collision system with terrain blending.
    /// </summary>
    public sealed class ProceduralPlanetCollision32 : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float blendRadius = 10f;

        [Header("References")]
        [SerializeField] PlanetBody planet;

        void Start()
        {
            if (planet == null)
                planet = FindFirstObjectByType<PlanetBody>();
        }

        /// <summary>
        /// Blend terrain heights.
        /// </summary>
        public float BlendHeight(float height1, float height2, float distance, float blendRadius)
        {
            if (distance > blendRadius) return height1;

            float t = distance / blendRadius;
            return Mathf.Lerp(height1, height2, t);
        }
    }
}
