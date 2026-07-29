using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Collision system with overlap sphere detection.
    /// </summary>
    public sealed class ProceduralPlanetCollision9 : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float overlapRadius = 1f;
        [SerializeField] LayerMask collisionLayer = ~0;

        [Header("References")]
        [SerializeField] PlanetBody planet;

        void Start()
        {
            if (planet == null)
                planet = FindFirstObjectByType<PlanetBody>();
        }

        /// <summary>
        /// Check for overlapping colliders.
        /// </summary>
        public Collider[] CheckOverlap(Vector3 position)
        {
            return Physics.OverlapSphere(position, overlapRadius, collisionLayer);
        }

        /// <summary>
        /// Check if position is clear.
        /// </summary>
        public bool IsPositionClear(Vector3 position)
        {
            Collider[] colliders = CheckOverlap(position);
            return colliders.Length == 0;
        }
    }
}
