using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Collision system with terrain scaling.
    /// </summary>
    public sealed class ProceduralPlanetCollision37 : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float scaleFactor = 1.5f;

        [Header("References")]
        [SerializeField] PlanetBody planet;

        void Start()
        {
            if (planet == null)
                planet = FindFirstObjectByType<PlanetBody>();
        }

        /// <summary>
        /// Scale terrain height.
        /// </summary>
        public float ScaleHeight(float height)
        {
            return height * scaleFactor;
        }
    }
}
