using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Collision system with terrain normalization.
    /// </summary>
    public sealed class ProceduralPlanetCollision34 : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] PlanetBody planet;

        void Start()
        {
            if (planet == null)
                planet = FindFirstObjectByType<PlanetBody>();
        }

        /// <summary>
        /// Normalize terrain heights to 0-1 range.
        /// </summary>
        public float[] NormalizeHeights(float[] heights)
        {
            if (heights == null || heights.Length == 0) return heights;

            float min = float.MaxValue;
            float max = float.MinValue;

            foreach (float h in heights)
            {
                if (h < min) min = h;
                if (h > max) max = h;
            }

            float range = max - min;
            if (range < 0.001f) return heights;

            var normalized = new float[heights.Length];
            for (int i = 0; i < heights.Length; i++)
            {
                normalized[i] = (heights[i] - min) / range;
            }

            return normalized;
        }
    }
}
