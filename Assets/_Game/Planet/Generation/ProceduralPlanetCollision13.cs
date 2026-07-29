using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Collision system with step detection.
    /// </summary>
    public sealed class ProceduralPlanetCollision13 : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float stepHeight = 0.3f;
        [SerializeField] float stepSmooth = 0.1f;

        [Header("References")]
        [SerializeField] PlanetBody planet;

        void Start()
        {
            if (planet == null)
                planet = FindFirstObjectByType<PlanetBody>();
        }

        /// <summary>
        /// Check if a step can be climbed.
        /// </summary>
        public bool CanClimbStep(Vector3 position, Vector3 direction)
        {
            if (planet == null) return false;

            // Cast ray forward
            Ray ray = new Ray(position + Vector3.up * 0.1f, direction);
            if (Physics.Raycast(ray, out RaycastHit hit, 1f))
            {
                // Check if step height is climbable
                float stepDiff = hit.point.y - position.y;
                return stepDiff > 0 && stepDiff <= stepHeight;
            }

            return false;
        }

        /// <summary>
        /// Smooth step climbing.
        /// </summary>
        public Vector3 SmoothStep(Vector3 current, Vector3 target)
        {
            return Vector3.Lerp(current, target, stepSmooth);
        }
    }
}
