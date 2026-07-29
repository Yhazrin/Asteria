using System.Collections.Generic;
using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Manager with planet instancing.
    /// </summary>
    public sealed class ProceduralPlanetManager6 : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] int maxInstances = 10;

        [Header("References")]
        [SerializeField] Transform player;

        readonly Dictionary<string, PlanetInstance> _instances = new();

        void Start()
        {
            if (player == null)
            {
                var playerBody = FindFirstObjectByType<Player.SphericalGravityBody>();
                player = playerBody?.transform;
            }
        }

        /// <summary>
        /// Create a planet instance.
        /// </summary>
        public PlanetInstance CreateInstance(string planetName, Vector3 position, int seed)
        {
            if (_instances.Count >= maxInstances) return null;

            var go = new GameObject(planetName);
            go.transform.SetParent(transform, false);
            go.transform.position = position;

            var generator = go.AddComponent<ProceduralPlanetGenerator>();
            generator.Generate();

            var instance = new PlanetInstance
            {
                name = planetName,
                generator = generator,
                position = position,
                seed = seed
            };

            _instances[planetName] = instance;
            return instance;
        }

        /// <summary>
        /// Destroy a planet instance.
        /// </summary>
        public void DestroyInstance(string planetName)
        {
            if (_instances.TryGetValue(planetName, out var instance))
            {
                if (instance.generator != null)
                {
                    Destroy(instance.generator.gameObject);
                }
                _instances.Remove(planetName);
            }
        }

        /// <summary>
        /// Get all instances.
        /// </summary>
        public IReadOnlyDictionary<string, PlanetInstance> GetInstances()
        {
            return _instances;
        }

        public class PlanetInstance
        {
            public string name;
            public ProceduralPlanetGenerator generator;
            public Vector3 position;
            public int seed;
        }
    }
}
