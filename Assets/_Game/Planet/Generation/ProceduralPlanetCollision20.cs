using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Collision system with terrain smoothing.
    /// </summary>
    public sealed class ProceduralPlanetCollision20 : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float smoothFactor = 0.5f;

        [Header("References")]
        [SerializeField] PlanetBody planet;

        void Start()
        {
            if (planet == null)
                planet = FindFirstObjectByType<PlanetBody>();
        }

        /// <summary>
        /// Smooth terrain normal at position.
        /// </summary>
        public Vector3 GetSmoothedNormal(Vector3 position, Vector3 rawNormal)
        {
            if (planet == null) return rawNormal;

            // Average with planet surface normal
            Vector3 surfaceNormal = (position - planet.Center).normalized;
            return Vector3.Lerp(rawNormal, surfaceNormal, smoothFactor).normalized;
        }
    }
}
