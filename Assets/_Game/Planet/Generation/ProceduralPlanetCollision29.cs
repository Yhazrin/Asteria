using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Collision system with terrain erosion.
    /// </summary>
    public sealed class ProceduralPlanetCollision29 : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float erosionRate = 0.1f;
        [SerializeField] float erosionThreshold = 0.5f;

        [Header("References")]
        [SerializeField] PlanetBody planet;

        void Start()
        {
            if (planet == null)
                planet = FindFirstObjectByType<PlanetBody>();
        }

        /// <summary>
        /// Apply erosion to terrain height.
        /// </summary>
        public float ApplyErosion(float height, float slope, float deltaTime)
        {
            if (slope < erosionThreshold) return height;

            float erosion = erosionRate * slope * deltaTime;
            return height - erosion;
        }
    }
}
