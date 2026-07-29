using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Collision system with heightmap-based detection.
    /// </summary>
    public sealed class ProceduralPlanetCollision5 : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float heightmapResolution = 1f;

        [Header("References")]
        [SerializeField] PlanetBody planet;
        [SerializeField] float[,] heightmap;

        void Start()
        {
            if (planet == null)
                planet = FindFirstObjectByType<PlanetBody>();

            GenerateHeightmap();
        }

        void GenerateHeightmap()
        {
            int size = Mathf.CeilToInt(planet.Radius * 2 * Mathf.PI / heightmapResolution);
            heightmap = new float[size, size];

            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    float nx = (float)x / size * 8f;
                    float ny = (float)y / size * 8f;

                    heightmap[x, y] = Mathf.PerlinNoise(nx, ny) * 20f;
                }
            }
        }

        /// <summary>
        /// Get height at UV coordinates.
        /// </summary>
        public float GetHeight(float u, float v)
        {
            if (heightmap == null) return 0f;

            int x = Mathf.FloorToInt(u * (heightmap.GetLength(0) - 1));
            int y = Mathf.FloorToInt(v * (heightmap.GetLength(1) - 1));

            x = Mathf.Clamp(x, 0, heightmap.GetLength(0) - 1);
            y = Mathf.Clamp(y, 0, heightmap.GetLength(1) - 1);

            return heightmap[x, y];
        }
    }
}
