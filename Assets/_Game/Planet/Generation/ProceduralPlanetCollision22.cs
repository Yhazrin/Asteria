using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Collision system with terrain snapping.
    /// </summary>
    public sealed class ProceduralPlanetCollision22 : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float snapDistance = 2f;
        [SerializeField] float snapSpeed = 10f;

        [Header("References")]
        [SerializeField] PlanetBody planet;

        void Start()
        {
            if (planet == null)
                planet = FindFirstObjectByType<PlanetBody>();
        }

        /// <summary>
        /// Snap position to terrain surface.
        /// </summary>
        public Vector3 SnapToTerrain(Vector3 position)
        {
            if (planet == null) return position;

            Vector3 direction = (position - planet.Center).normalized;
            Vector3 surfacePoint = planet.GetPointOnSurface(direction, 1f);

            float distance = Vector3.Distance(position, surfacePoint);
            if (distance < snapDistance)
            {
                return Vector3.Lerp(position, surfacePoint, snapSpeed * Time.deltaTime);
            }

            return position;
        }
    }
}
