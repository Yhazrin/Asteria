using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Collision system with ground detection.
    /// </summary>
    public sealed class ProceduralPlanetCollision15 : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float groundCheckDistance = 1f;
        [SerializeField] LayerMask groundLayer = ~0;

        [Header("References")]
        [SerializeField] PlanetBody planet;

        void Start()
        {
            if (planet == null)
                planet = FindFirstObjectByType<PlanetBody>();
        }

        /// <summary>
        /// Check if grounded.
        /// </summary>
        public bool IsGrounded(Vector3 position, Vector3 up)
        {
            Ray ray = new Ray(position + up * 0.1f, -up);
            return Physics.Raycast(ray, out _, groundCheckDistance, groundLayer);
        }

        /// <summary>
        /// Get ground normal.
        /// </summary>
        public Vector3 GetGroundNormal(Vector3 position, Vector3 up)
        {
            Ray ray = new Ray(position + up * 0.1f, -up);
            if (Physics.Raycast(ray, out RaycastHit hit, groundCheckDistance, groundLayer))
            {
                return hit.normal;
            }

            return up;
        }
    }
}
