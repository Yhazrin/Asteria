using System.Collections.Generic;
using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Alternative planet manager with different approach.
    /// Uses object pooling for planets.
    /// </summary>
    public sealed class ProceduralPlanetManager2 : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] int maxActivePlanets = 3;
        [SerializeField] float streamingDistance = 1000f;

        [Header("References")]
        [SerializeField] Transform player;
        [SerializeField] ProceduralPlanetConfig[] planetConfigs;

        readonly Dictionary<string, ProceduralPlanetGenerator> _activePlanets = new();
        readonly Queue<ProceduralPlanetGenerator> _planetPool = new();

        void Start()
        {
            if (player == null)
            {
                var playerBody = FindFirstObjectByType<Player.SphericalGravityBody>();
                player = playerBody?.transform;
            }

            // Initialize pool
            for (int i = 0; i < maxActivePlanets; i++)
            {
                var go = new GameObject($"PooledPlanet_{i}");
                go.transform.SetParent(transform, false);
                go.SetActive(false);

                var generator = go.AddComponent<ProceduralPlanetGenerator>();
                _planetPool.Enqueue(generator);
            }
        }

        /// <summary>
        /// Get a planet from the pool.
        /// </summary>
        public ProceduralPlanetGenerator GetPlanetFromPool()
        {
            if (_planetPool.Count > 0)
            {
                var planet = _planetPool.Dequeue();
                planet.gameObject.SetActive(true);
                return planet;
            }

            return null;
        }

        /// <summary>
        /// Return a planet to the pool.
        /// </summary>
        public void ReturnPlanetToPool(ProceduralPlanetGenerator planet)
        {
            if (planet == null) return;

            planet.gameObject.SetActive(false);
            _planetPool.Enqueue(planet);
        }

        /// <summary>
        /// Load a planet by name.
        /// </summary>
        public void LoadPlanet(string planetName)
        {
            if (_activePlanets.ContainsKey(planetName)) return;

            var planet = GetPlanetFromPool();
            if (planet == null) return;

            planet.name = planetName;
            planet.Generate();
            _activePlanets[planetName] = planet;
        }

        /// <summary>
        /// Unload a planet by name.
        /// </summary>
        public void UnloadPlanet(string planetName)
        {
            if (!_activePlanets.TryGetValue(planetName, out var planet)) return;

            ReturnPlanetToPool(planet);
            _activePlanets.Remove(planetName);
        }

        /// <summary>
        /// Get all active planets.
        /// </summary>
        public IReadOnlyDictionary<string, ProceduralPlanetGenerator> GetActivePlanets()
        {
            return _activePlanets;
        }
    }
}
