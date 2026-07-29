using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Collision system with terrain projection.
    /// </summary>
    public sealed class ProceduralPlanetCollision21 : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float projectionDistance = 10f;

        [Header("References")]
        [SerializeField] PlanetBody planet;

        void Start()
        {
            if (planet == null)
                planet = FindFirstObjectByType<PlanetBody>();
        }

        /// <summary>
        /// Project position onto terrain.
        /// </summary>
        public Vector3 ProjectOntoTerrain(Vector3 position)
        {
            if (planet == null) return position;

            Vector3 direction = (position - planet.Center).normalized;
            return planet.GetPointOnSurface(direction, 1f);
        }

        /// <summary>
        /// Project direction onto terrain tangent.
        /// </summary>
        public Vector3 ProjectDirectionOntoTangent(Vector3 direction, Vector3 surfaceNormal)
        {
            return Vector3.ProjectOnPlane(direction, surfaceNormal).normalized;
        }
    }
}
