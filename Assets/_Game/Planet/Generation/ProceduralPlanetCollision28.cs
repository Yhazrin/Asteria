using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Collision system with terrain deformation.
    /// </summary>
    public sealed class ProceduralPlanetCollision28 : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float deformRadius = 5f;
        [SerializeField] float deformStrength = 1f;

        [Header("References")]
        [SerializeField] PlanetBody planet;

        void Start()
        {
            if (planet == null)
                planet = FindFirstObjectByType<PlanetBody>();
        }

        /// <summary>
        /// Deform terrain at position.
        /// </summary>
        public float DeformHeight(float originalHeight, float distanceFromImpact, float impactStrength)
        {
            if (distanceFromImpact > deformRadius) return originalHeight;

            float factor = 1f - (distanceFromImpact / deformRadius);
            float deformation = impactStrength * deformStrength * factor;

            return originalHeight - deformation;
        }
    }
}
