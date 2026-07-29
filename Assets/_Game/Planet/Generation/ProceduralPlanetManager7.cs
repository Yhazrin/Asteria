using System.Collections.Generic;
using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Manager with planet recycling.
    /// </summary>
    public sealed class ProceduralPlanetManager7 : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] int maxRecycledPlanets = 5;

        [Header("References")]
        [SerializeField] Transform player;

        readonly Queue<ProceduralPlanetGenerator> _recycled = new();
        readonly Dictionary<string, ProceduralPlanetGenerator> _active = new();

        void Start()
        {
            if (player == null)
            {
                var playerBody = FindFirstObjectByType<Player.SphericalGravityBody>();
                player = playerBody?.transform;
            }
        }

        /// <summary>
        /// Get a recycled planet or create new.
        /// </summary>
        public ProceduralPlanetGenerator GetPlanet(string planetName)
        {
            if (_recycled.Count > 0)
            {
                var planet = _recycled.Dequeue();
                planet.gameObject.SetActive(true);
                planet.name = planetName;
                _active[planetName] = planet;
                return planet;
            }

            var go = new GameObject(planetName);
            go.transform.SetParent(transform, false);
            var generator = go.AddComponent<ProceduralPlanetGenerator>();
            _active[planetName] = generator;
            return generator;
        }

        /// <summary>
        /// Recycle a planet.
        /// </summary>
        public void RecyclePlanet(string planetName)
        {
            if (!_active.TryGetValue(planetName, out var planet)) return;

            planet.gameObject.SetActive(false);
            _active.Remove(planetName);

            if (_recycled.Count < maxRecycledPlanets)
            {
                _recycled.Enqueue(planet);
            }
            else
            {
                Destroy(planet.gameObject);
            }
        }

        /// <summary>
        /// Get active planet count.
        /// </summary>
        public int GetActiveCount()
        {
            return _active.Count;
        }

        /// <summary>
        /// Get recycled planet count.
        /// </summary>
        public int GetRecycledCount()
        {
            return _recycled.Count;
        }
    }
}
