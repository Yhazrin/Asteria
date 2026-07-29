using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Collision system with terrain noise.
    /// </summary>
    public sealed class ProceduralPlanetCollision31 : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] int noiseSeed = 42;

        [Header("References")]
        [SerializeField] PlanetBody planet;

        void Start()
        {
            if (planet == null)
                planet = FindFirstObjectByType<PlanetBody>();
        }

        /// <summary>
        /// Generate noise-based terrain.
        /// </summary>
        public float GenerateNoiseTerrain(float x, float z)
        {
            float nx = x * 0.01f + noiseSeed;
            float nz = z * 0.01f + noiseSeed;

            float height = 0;
            height += Mathf.PerlinNoise(nx, nz) * 10f;
            height += Mathf.PerlinNoise(nx * 2f, nz * 2f) * 5f;
            height += Mathf.PerlinNoise(nx * 4f, nz * 4f) * 2.5f;

            return height;
        }
    }
}
