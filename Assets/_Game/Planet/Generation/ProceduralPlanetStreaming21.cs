using System.Collections.Generic;
using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Streaming system with chunk caching.
    /// </summary>
    public sealed class ProceduralPlanetStreaming21 : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] int maxCacheSize = 50;

        [Header("References")]
        [SerializeField] ProceduralPlanetGenerator planetGenerator;

        readonly Dictionary<string, Mesh> _cache = new();
        readonly Queue<string> _cacheOrder = new();

        void Start()
        {
            if (planetGenerator == null)
                planetGenerator = FindFirstObjectByType<ProceduralPlanetGenerator>();
        }

        /// <summary>
        /// Cache a mesh.
        /// </summary>
        public void CacheMesh(string chunkId, Mesh mesh)
        {
            if (_cache.Count >= maxCacheSize)
            {
                string oldest = _cacheOrder.Dequeue();
                _cache.Remove(oldest);
            }

            _cache[chunkId] = mesh;
            _cacheOrder.Enqueue(chunkId);
        }

        /// <summary>
        /// Get cached mesh.
        /// </summary>
        public Mesh GetCachedMesh(string chunkId)
        {
            return _cache.TryGetValue(chunkId, out var mesh) ? mesh : null;
        }

        /// <summary>
        /// Clear cache.
        /// </summary>
        public void ClearCache()
        {
            _cache.Clear();
            _cacheOrder.Clear();
        }
    }
}
