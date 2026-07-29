using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Collision system with terrain rounding.
    /// </summary>
    public sealed class ProceduralPlanetCollision36 : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float roundFactor = 0.5f;

        [Header("References")]
        [SerializeField] PlanetBody planet;

        void Start()
        {
            if (planet == null)
                planet = FindFirstObjectByType<PlanetBody>();
        }

        /// <summary>
        /// Round terrain height.
        /// </summary>
        public float RoundHeight(float height)
        {
            return Mathf.Round(height / roundFactor) * roundFactor;
        }
    }
}
