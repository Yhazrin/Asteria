using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Collision system with surface type detection.
    /// </summary>
    public sealed class ProceduralPlanetCollision17 : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] PlanetBody planet;

        void Start()
        {
            if (planet == null)
                planet = FindFirstObjectByType<PlanetBody>();
        }

        /// <summary>
        /// Detect surface type at position.
        /// </summary>
        public string DetectSurfaceType(Vector3 position)
        {
            if (planet == null) return "unknown";

            Vector3 direction = (position - planet.Center).normalized;
            float latitude = Mathf.Abs(direction.y);

            if (latitude > 0.8f) return "snow";
            if (latitude > 0.5f) return "rock";
            return "grass";
        }
    }
}
