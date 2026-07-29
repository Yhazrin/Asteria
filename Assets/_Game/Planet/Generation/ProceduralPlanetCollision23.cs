using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Collision system with terrain alignment.
    /// </summary>
    public sealed class ProceduralPlanetCollision23 : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float alignmentSpeed = 5f;

        [Header("References")]
        [SerializeField] PlanetBody planet;

        void Start()
        {
            if (planet == null)
                planet = FindFirstObjectByType<PlanetBody>();
        }

        /// <summary>
        /// Align rotation to terrain.
        /// </summary>
        public Quaternion AlignToTerrain(Quaternion currentRotation, Vector3 position)
        {
            if (planet == null) return currentRotation;

            Vector3 surfaceNormal = (position - planet.Center).normalized;
            Quaternion targetRotation = Quaternion.FromToRotation(Vector3.up, surfaceNormal) * currentRotation;

            return Quaternion.Slerp(currentRotation, targetRotation, alignmentSpeed * Time.deltaTime);
        }
    }
}
