using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Collision system with mesh-based terrain detection.
    /// </summary>
    public sealed class ProceduralPlanetCollision3 : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float meshCollisionRadius = 1f;

        [Header("References")]
        [SerializeField] PlanetBody planet;
        [SerializeField] MeshFilter terrainMesh;

        void Start()
        {
            if (planet == null)
                planet = FindFirstObjectByType<PlanetBody>();
            if (terrainMesh == null)
                terrainMesh = planet?.GetComponent<MeshFilter>();
        }

        /// <summary>
        /// Check collision using mesh data.
        /// </summary>
        public bool IsCollidingWithMesh(Vector3 position, float radius)
        {
            if (terrainMesh == null || terrainMesh.mesh == null) return false;

            // Simplified mesh collision check
            Vector3 localPos = terrainMesh.transform.InverseTransformPoint(position);
            float distance = localPos.magnitude;

            return distance < planet.Radius + radius;
        }

        /// <summary>
        /// Get the closest point on mesh.
        /// </summary>
        public Vector3 GetClosestPointOnMesh(Vector3 position)
        {
            if (terrainMesh == null || terrainMesh.mesh == null) return position;

            Vector3 direction = (position - planet.Center).normalized;
            return planet.GetPointOnSurface(direction, 0f);
        }
    }
}
