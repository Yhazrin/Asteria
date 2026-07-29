using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Collision system with sweep tests.
    /// </summary>
    public sealed class ProceduralPlanetCollision7 : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float sweepRadius = 0.5f;
        [SerializeField] float sweepDistance = 1f;

        [Header("References")]
        [SerializeField] PlanetBody planet;

        void Start()
        {
            if (planet == null)
                planet = FindFirstObjectByType<PlanetBody>();
        }

        /// <summary>
        /// Perform a sweep test.
        /// </summary>
        public bool SweepTest(Vector3 position, Vector3 direction, out RaycastHit hit)
        {
            hit = default;
            if (planet == null) return false;

            Ray ray = new Ray(position, direction);
            return Physics.SphereCast(ray, sweepRadius, out hit, sweepDistance);
        }

        /// <summary>
        /// Check if a position is valid (not colliding).
        /// </summary>
        public bool IsValidPosition(Vector3 position)
        {
            if (planet == null) return true;

            float distance = Vector3.Distance(position, planet.Center);
            return distance > planet.Radius + sweepRadius;
        }
    }
}
