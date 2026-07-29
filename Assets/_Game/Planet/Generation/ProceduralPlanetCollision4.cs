using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Collision system with sphere-based detection.
    /// </summary>
    public sealed class ProceduralPlanetCollision4 : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float collisionRadius = 0.5f;

        [Header("References")]
        [SerializeField] PlanetBody planet;

        void Start()
        {
            if (planet == null)
                planet = FindFirstObjectByType<PlanetBody>();
        }

        /// <summary>
        /// Check collision with sphere cast.
        /// </summary>
        public bool CheckCollision(Vector3 position, Vector3 direction, float distance)
        {
            if (planet == null) return false;

            Ray ray = new Ray(position, direction);
            return Physics.SphereCast(ray, collisionRadius, out _, distance);
        }

        /// <summary>
        /// Get collision point.
        /// </summary>
        public Vector3 GetCollisionPoint(Vector3 position, Vector3 direction, float distance)
        {
            if (planet == null) return position;

            Ray ray = new Ray(position, direction);
            if (Physics.SphereCast(ray, collisionRadius, out RaycastHit hit, distance))
            {
                return hit.point;
            }

            return position + direction * distance;
        }
    }
}
