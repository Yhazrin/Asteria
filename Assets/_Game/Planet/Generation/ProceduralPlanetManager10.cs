using System.Collections.Generic;
using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Manager with planet batching.
    /// </summary>
    public sealed class ProceduralPlanetManager10 : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] int batchSize = 5;

        [Header("References")]
        [SerializeField] Transform player;

        readonly Dictionary<string, PlanetBatch> _batches = new();

        void Start()
        {
            if (player == null)
            {
                var playerBody = FindFirstObjectByType<Player.SphericalGravityBody>();
                player = playerBody?.transform;
            }
        }

        /// <summary>
        /// Add a planet to a batch.
        /// </summary>
        public void AddToBatch(string batchName, ProceduralPlanetGenerator planet)
        {
            if (!_batches.TryGetValue(batchName, out var batch))
            {
                batch = new PlanetBatch { name = batchName };
                _batches[batchName] = batch;
            }

            batch.planets.Add(planet);
        }

        /// <summary>
        /// Remove a planet from a batch.
        /// </summary>
        public void RemoveFromBatch(string batchName, ProceduralPlanetGenerator planet)
        {
            if (_batches.TryGetValue(batchName, out var batch))
            {
                batch.planets.Remove(planet);
            }
        }

        /// <summary>
        /// Get all planets in a batch.
        /// </summary>
        public List<ProceduralPlanetGenerator> GetBatch(string batchName)
        {
            return _batches.TryGetValue(batchName, out var batch) ? batch.planets : new List<ProceduralPlanetGenerator>();
        }

        class PlanetBatch
        {
            public string name;
            public List<ProceduralPlanetGenerator> planets = new();
        }
    }
}
