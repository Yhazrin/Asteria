using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Collision system with slope detection.
    /// </summary>
    public sealed class ProceduralPlanetCollision14 : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float maxSlopeAngle = 45f;

        [Header("References")]
        [SerializeField] PlanetBody planet;

        void Start()
        {
            if (planet == null)
                planet = FindFirstObjectByType<PlanetBody>();
        }

        /// <summary>
        /// Check if a slope is walkable.
        /// </summary>
        public bool IsWalkableSlope(Vector3 normal)
        {
            float angle = Vector3.Angle(normal, Vector3.up);
            return angle <= maxSlopeAngle;
        }

        /// <summary>
        /// Get slope angle at a position.
        /// </summary>
        public float GetSlopeAngle(Vector3 position)
        {
            if (planet == null) return 0f;

            Vector3 normal = (position - planet.Center).normalized;
            return Vector3.Angle(normal, Vector3.up);
        }
    }
}
