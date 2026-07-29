using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Collision system with terrain flattening.
    /// </summary>
    public sealed class ProceduralPlanetCollision26 : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float flattenRadius = 10f;
        [SerializeField] float flattenStrength = 0.5f;

        [Header("References")]
        [SerializeField] PlanetBody planet;

        void Start()
        {
            if (planet == null)
                planet = FindFirstObjectByType<PlanetBody>();
        }

        /// <summary>
        /// Flatten terrain around a point.
        /// </summary>
        public float FlattenHeight(float originalHeight, float distanceFromCenter)
        {
            if (distanceFromCenter > flattenRadius) return originalHeight;

            float factor = 1f - (distanceFromCenter / flattenRadius);
            return Mathf.Lerp(originalHeight, 0f, factor * flattenStrength);
        }
    }
}
