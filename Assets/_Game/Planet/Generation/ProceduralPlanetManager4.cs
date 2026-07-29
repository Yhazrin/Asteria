using System.Collections.Generic;
using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Manager with planet caching.
    /// </summary>
    public sealed class ProceduralPlanetManager4 : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] int maxCachedPlanets = 10;

        [Header("References")]
        [SerializeField] Transform player;

        readonly Dictionary<string, ProceduralPlanetGenerator> _cachedPlanets = new();
        readonly Queue<string> _cacheOrder = new();

        void Start()
        {
            if (player == null)
            {
                var playerBody = FindFirstObjectByType<Player.SphericalGravityBody>();
                player = playerBody?.transform;
            }
        }

        /// <summary>
        /// Get or create a planet.
        /// </summary>
        public ProceduralPlanetGenerator GetOrCreatePlanet(string planetName, int seed)
        {
            if (_cachedPlanets.TryGetValue(planetName, out var cached))
            {
                return cached;
            }

            // Create new planet
            var go = new GameObject(planetName);
            go.transform.SetParent(transform, false);

            var generator = go.AddComponent<ProceduralPlanetGenerator>();
            generator.Generate();

            // Add to cache
            _cachedPlanets[planetName] = generator;
            _cacheOrder.Enqueue(planetName);

            // Evict if cache is full
            while (_cacheOrder.Count > maxCachedPlanets)
            {
                string evictName = _cacheOrder.Dequeue();
                if (_cachedPlanets.TryGetValue(evictName, out var evictPlanet))
                {
                    Destroy(evictPlanet.gameObject);
                    _cachedPlanets.Remove(evictName);
                }
            }

            return generator;
        }

        /// <summary>
        /// Clear the cache.
        /// </summary>
        public void ClearCache()
        {
            foreach (var planet in _cachedPlanets.Values)
            {
                if (planet != null) Destroy(planet.gameObject);
            }

            _cachedPlanets.Clear();
            _cacheOrder.Clear();
        }
    }
}
