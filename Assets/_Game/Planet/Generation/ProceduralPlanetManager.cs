using System.Collections.Generic;
using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Manages multiple procedural planets.
    /// Handles planet switching, streaming, and memory management.
    /// </summary>
    public sealed class ProceduralPlanetManager : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] int maxActivePlanets = 3;
        [SerializeField] float streamingDistance = 1000f;
        [SerializeField] float unloadDistance = 2000f;

        [Header("References")]
        [SerializeField] Transform player;
        [SerializeField] ProceduralPlanetConfig[] planetConfigs;

        readonly Dictionary<string, ProceduralPlanetGenerator> _activePlanets = new();
        readonly Dictionary<string, ProceduralPlanetConfig> _planetConfigs = new();
        readonly Queue<string> _loadQueue = new();
        readonly Queue<string> _unloadQueue = new();

        void Start()
        {
            if (player == null)
            {
                var playerBody = FindFirstObjectByType<Player.SphericalGravityBody>();
                player = playerBody?.transform;
            }

            // Register planet configs
            if (planetConfigs != null)
            {
                foreach (var config in planetConfigs)
                {
                    if (config != null)
                    {
                        _planetConfigs[config.planetName] = config;
                    }
                }
            }
        }

        void Update()
        {
            UpdateStreaming();
            ProcessQueues();
        }

        void UpdateStreaming()
        {
            if (player == null) return;

            Vector3 playerPos = player.position;

            // Check which planets should be active
            foreach (var kvp in _planetConfigs)
            {
                string planetName = kvp.Key;
                var config = kvp.Value;

                // Calculate distance to planet (simplified - using origin)
                float distance = Vector3.Distance(playerPos, Vector3.zero);

                bool isActive = _activePlanets.ContainsKey(planetName);

                if (!isActive && distance < streamingDistance)
                {
                    // Should load
                    if (!_loadQueue.Contains(planetName))
                    {
                        _loadQueue.Enqueue(planetName);
                    }
                }
                else if (isActive && distance > unloadDistance)
                {
                    // Should unload
                    if (!_unloadQueue.Contains(planetName))
                    {
                        _unloadQueue.Enqueue(planetName);
                    }
                }
            }
        }

        void ProcessQueues()
        {
            // Process load queue
            if (_loadQueue.Count > 0 && _activePlanets.Count < maxActivePlanets)
            {
                string planetName = _loadQueue.Dequeue();
                LoadPlanet(planetName);
            }

            // Process unload queue
            if (_unloadQueue.Count > 0)
            {
                string planetName = _unloadQueue.Dequeue();
                UnloadPlanet(planetName);
            }
        }

        void LoadPlanet(string planetName)
        {
            if (_activePlanets.ContainsKey(planetName)) return;
            if (!_planetConfigs.TryGetValue(planetName, out var config)) return;

            Debug.Log($"[ProceduralPlanetManager] Loading planet: {planetName}");

            var planetData = config.CreatePlanet();
            if (planetData != null)
            {
                _activePlanets[planetName] = planetData.planetBody.GetComponent<ProceduralPlanetGenerator>();
            }
        }

        void UnloadPlanet(string planetName)
        {
            if (!_activePlanets.TryGetValue(planetName, out var generator)) return;

            Debug.Log($"[ProceduralPlanetManager] Unloading planet: {planetName}");

            Destroy(generator.gameObject);
            _activePlanets.Remove(planetName);
        }

        /// <summary>
        /// Load a specific planet by name.
        /// </summary>
        public void LoadPlanetImmediate(string planetName)
        {
            if (!_loadQueue.Contains(planetName))
            {
                _loadQueue.Enqueue(planetName);
            }
        }

        /// <summary>
        /// Unload a specific planet by name.
        /// </summary>
        public void UnloadPlanetImmediate(string planetName)
        {
            if (!_unloadQueue.Contains(planetName))
            {
                _unloadQueue.Enqueue(planetName);
            }
        }

        /// <summary>
        /// Get all active planets.
        /// </summary>
        public IReadOnlyDictionary<string, ProceduralPlanetGenerator> GetActivePlanets()
        {
            return _activePlanets;
        }

        /// <summary>
        /// Get a planet by name.
        /// </summary>
        public ProceduralPlanetGenerator GetPlanet(string planetName)
        {
            return _activePlanets.TryGetValue(planetName, out var generator) ? generator : null;
        }

        /// <summary>
        /// Check if a planet is loaded.
        /// </summary>
        public bool IsPlanetLoaded(string planetName)
        {
            return _activePlanets.ContainsKey(planetName);
        }
    }
}
