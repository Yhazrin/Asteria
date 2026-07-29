using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Collision system with terrain inversion.
    /// </summary>
    public sealed class ProceduralPlanetCollision38 : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] PlanetBody planet;

        void Start()
        {
            if (planet == null)
                planet = FindFirstObjectByType<PlanetBody>();
        }

        /// <summary>
        /// Invert terrain height.
        /// </summary>
        public float InvertHeight(float height, float maxHeight)
        {
            return maxHeight - height;
        }
    }
}
