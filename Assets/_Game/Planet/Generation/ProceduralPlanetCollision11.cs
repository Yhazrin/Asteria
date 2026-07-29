using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Collision system with contact modification.
    /// </summary>
    public sealed class ProceduralPlanetCollision11 : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float contactOffset = 0.01f;

        [Header("References")]
        [SerializeField] PlanetBody planet;

        void Start()
        {
            if (planet == null)
                planet = FindFirstObjectByType<PlanetBody>();
        }

        /// <summary>
        /// Modify contact point.
        /// </summary>
        public Vector3 ModifyContactPoint(Vector3 contactPoint, Vector3 contactNormal)
        {
            return contactPoint + contactNormal * contactOffset;
        }
    }
}
