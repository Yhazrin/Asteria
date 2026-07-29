using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Collision system with terrain smoothing.
    /// </summary>
    public sealed class ProceduralPlanetCollision27 : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] int smoothIterations = 3;

        [Header("References")]
        [SerializeField] PlanetBody planet;

        void Start()
        {
            if (planet == null)
                planet = FindFirstObjectByType<PlanetBody>();
        }

        /// <summary>
        /// Smooth terrain heights.
        /// </summary>
        public float[] SmoothHeights(float[] heights, int width, int height)
        {
            var result = new float[heights.Length];
            System.Array.Copy(heights, result, heights.Length);

            for (int iter = 0; iter < smoothIterations; iter++)
            {
                for (int y = 1; y < height - 1; y++)
                {
                    for (int x = 1; x < width - 1; x++)
                    {
                        int idx = y * width + x;
                        float sum = 0;
                        int count = 0;

                        for (int dy = -1; dy <= 1; dy++)
                        {
                            for (int dx = -1; dx <= 1; dx++)
                            {
                                int neighborIdx = (y + dy) * width + (x + dx);
                                sum += result[neighborIdx];
                                count++;
                            }
                        }

                        result[idx] = sum / count;
                    }
                }
            }

            return result;
        }
    }
}
