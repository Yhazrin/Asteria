using System.Collections.Generic;
using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Manager with planet pooling.
    /// </summary>
    public sealed class ProceduralPlanetManager5 : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] int poolSize = 5;

        [Header("References")]
        [SerializeField] Transform player;

        readonly Queue<ProceduralPlanetGenerator> _pool = new();
        readonly Dictionary<string, ProceduralPlanetGenerator> _active = new();

        void Start()
        {
            if (player == null)
            {
                var playerBody = FindFirstObjectByType<Player.SphericalGravityBody>();
                player = playerBody?.transform;
            }

            // Initialize pool
            for (int i = 0; i < poolSize; i++)
            {
                var go = new GameObject($"PlanetPool_{i}");
                go.transform.SetParent(transform, false);
                go.SetActive(false);

                var generator = go.AddComponent<ProceduralPlanetGenerator>();
                _pool.Enqueue(generator);
            }
        }

        /// <summary>
        /// Get a planet from the pool.
        /// </summary>
        public ProceduralPlanetGenerator GetPlanet()
        {
            if (_pool.Count > 0)
            {
                var planet = _pool.Dequeue();
                planet.gameObject.SetActive(true);
                return planet;
            }

            return null;
        }

        /// <summary>
        /// Return a planet to the pool.
        /// </summary>
        public void ReturnPlanet(ProceduralPlanetGenerator planet)
        {
            if (planet == null) return;

            planet.gameObject.SetActive(false);
            _pool.Enqueue(planet);
        }

        /// <summary>
        /// Get pool size.
        /// </summary>
        public int GetPoolSize()
        {
            return _pool.Count;
        }
    }
}
