using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Alternative collision system with different approach.
    /// Uses raycasting for terrain detection.
    /// </summary>
    public sealed class ProceduralPlanetCollision2 : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float raycastDistance = 10f;
        [SerializeField] LayerMask terrainLayer = ~0;

        [Header("References")]
        [SerializeField] PlanetBody planet;

        void Start()
        {
            if (planet == null)
                planet = FindFirstObjectByType<PlanetBody>();
        }

        /// <summary>
        /// Check if a position is colliding with terrain.
        /// </summary>
        public bool IsCollidingWithTerrain(Vector3 position, float radius)
        {
            if (planet == null) return false;

            Vector3 direction = (planet.Center - position).normalized;
            Ray ray = new Ray(position, direction);

            if (Physics.Raycast(ray, out RaycastHit hit, raycastDistance, terrainLayer))
            {
                return hit.distance < radius;
            }

            return false;
        }

        /// <summary>
        /// Get the closest point on terrain.
        /// </summary>
        public Vector3 GetClosestTerrainPoint(Vector3 position)
        {
            if (planet == null) return position;

            Vector3 direction = (planet.Center - position).normalized;
            Ray ray = new Ray(position, direction);

            if (Physics.Raycast(ray, out RaycastHit hit, raycastDistance, terrainLayer))
            {
                return hit.point;
            }

            return planet.GetPointOnSurface(direction, 0f);
        }

        /// <summary>
        /// Get the terrain height at a position.
        /// </summary>
        public float GetTerrainHeight(Vector3 position)
        {
            if (planet == null) return 0f;

            Vector3 direction = (planet.Center - position).normalized;
            Ray ray = new Ray(position, direction);

            if (Physics.Raycast(ray, out RaycastHit hit, raycastDistance, terrainLayer))
            {
                return hit.distance;
            }

            return 0f;
        }

        /// <summary>
        /// Get the surface normal at a position.
        /// </summary>
        public Vector3 GetSurfaceNormal(Vector3 position)
        {
            if (planet == null) return Vector3.up;

            Vector3 direction = (planet.Center - position).normalized;
            Ray ray = new Ray(position, direction);

            if (Physics.Raycast(ray, out RaycastHit hit, raycastDistance, terrainLayer))
            {
                return hit.normal;
            }

            return direction;
        }
    }
}
