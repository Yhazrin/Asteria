using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Collision system with continuous detection.
    /// </summary>
    public sealed class ProceduralPlanetCollision8 : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float continuousRadius = 0.5f;

        [Header("References")]
        [SerializeField] PlanetBody planet;

        void Start()
        {
            if (planet == null)
                planet = FindFirstObjectByType<PlanetBody>();
        }

        /// <summary>
        /// Check for continuous collision along a path.
        /// </summary>
        public bool CheckContinuousCollision(Vector3 start, Vector3 end, out RaycastHit hit)
        {
            hit = default;
            if (planet == null) return false;

            Vector3 direction = end - start;
            float distance = direction.magnitude;

            if (distance < 0.001f) return false;

            Ray ray = new Ray(start, direction.normalized);
            return Physics.SphereCast(ray, continuousRadius, out hit, distance);
        }

        /// <summary>
        /// Get the safe position along a path (before collision).
        /// </summary>
        public Vector3 GetSafePosition(Vector3 start, Vector3 end)
        {
            if (CheckContinuousCollision(start, end, out RaycastHit hit))
            {
                return hit.point - (end - start).normalized * continuousRadius;
            }

            return end;
        }
    }
}
