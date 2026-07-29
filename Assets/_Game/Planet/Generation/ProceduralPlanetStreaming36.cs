using System.Collections.Generic;
using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Streaming system with chunk caching.
    /// </summary>
    public sealed class ProceduralPlanetStreaming36 : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] int maxCacheSize = 100;

        [Header("References")]
        [SerializeField] ProceduralPlanetGenerator planetGenerator;

        readonly Dictionary<string, Mesh> _cache = new();

        void Start()
        {
            if (planetGenerator == null)
                planetGenerator = FindFirstObjectByType<ProceduralPlanetGenerator>();
        }

        /// <summary>
        /// Cache a mesh.
        /// </summary>
        public void CacheMesh(string key, Mesh mesh)
        {
            if (_cache.Count >= maxCacheSize)
            {
                // Remove oldest entry
                var enumerator = _cache.GetEnumerator();
                enumerator.MoveNext();
                _cache.Remove(enumerator.Current.Key);
            }

            _cache[key] = mesh;
        }

        /// <summary>
        /// Get cached mesh.
        /// </summary>
        public Mesh GetCachedMesh(string key)
        {
            return _cache.TryGetValue(key, out var mesh) ? mesh : null;
        }
    }
}
