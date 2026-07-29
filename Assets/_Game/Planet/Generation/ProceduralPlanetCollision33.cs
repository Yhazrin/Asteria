using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Collision system with terrain detail.
    /// </summary>
    public sealed class ProceduralPlanetCollision33 : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float detailScale = 0.05f;
        [SerializeField] float detailAmplitude = 2f;

        [Header("References")]
        [SerializeField] PlanetBody planet;

        void Start()
        {
            if (planet == null)
                planet = FindFirstObjectByType<PlanetBody>();
        }

        /// <summary>
        /// Add detail to terrain height.
        /// </summary>
        public float AddDetail(float baseHeight, float x, float z)
        {
            float detail = Mathf.PerlinNoise(x * detailScale, z * detailScale) * detailAmplitude;
            return baseHeight + detail;
        }
    }
}
