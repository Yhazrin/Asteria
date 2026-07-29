using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Collision system with terrain caching.
    /// </summary>
    public sealed class ProceduralPlanetCollision24 : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] int cacheSize = 100;

        [Header("References")]
        [SerializeField] PlanetBody planet;

        readonly System.Collections.Generic.Dictionary<Vector3Int, float> _heightCache = new();

        void Start()
        {
            if (planet == null)
                planet = FindFirstObjectByType<PlanetBody>();
        }

        /// <summary>
        /// Get cached height at grid position.
        /// </summary>
        public float GetCachedHeight(Vector3Int gridPos)
        {
            if (_heightCache.TryGetValue(gridPos, out float height))
            {
                return height;
            }

            // Calculate and cache
            height = CalculateHeight(gridPos);
            _heightCache[gridPos] = height;

            // Limit cache size
            if (_heightCache.Count > cacheSize)
            {
                var enumerator = _heightCache.GetEnumerator();
                enumerator.MoveNext();
                _heightCache.Remove(enumerator.Current.Key);
            }

            return height;
        }

        float CalculateHeight(Vector3Int gridPos)
        {
            // Simplified height calculation
            return Mathf.PerlinNoise(gridPos.x * 0.1f, gridPos.z * 0.1f) * 10f;
        }
    }
}
