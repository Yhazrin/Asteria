using System.Collections.Generic;
using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Manager with streaming and LOD integration.
    /// </summary>
    public sealed class ProceduralPlanetManager3 : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] int maxActivePlanets = 3;
        [SerializeField] float streamingDistance = 1000f;

        [Header("References")]
        [SerializeField] Transform player;
        [SerializeField] ProceduralPlanetConfig[] planetConfigs;

        readonly Dictionary<string, ProceduralPlanetGenerator> _activePlanets = new();

        void Start()
        {
            if (player == null)
            {
                var playerBody = FindFirstObjectByType<Player.SphericalGravityBody>();
                player = playerBody?.transform;
            }
        }

        void Update()
        {
            UpdateStreaming();
        }

        void UpdateStreaming()
        {
            if (player == null) return;

            Vector3 playerPos = player.position;

            foreach (var kvp in _activePlanets)
            {
                float distance = Vector3.Distance(playerPos, kvp.Value.transform.position);

                if (distance > streamingDistance * 2f)
                {
                    // Unload
                    UnloadPlanet(kvp.Key);
                    break;
                }
            }
        }

        /// <summary>
        /// Load a planet by name.
        /// </summary>
        public void LoadPlanet(string planetName)
        {
            if (_activePlanets.ContainsKey(planetName)) return;

            var go = new GameObject(planetName);
            go.transform.SetParent(transform, false);

            var generator = go.AddComponent<ProceduralPlanetGenerator>();
            generator.Generate();

            _activePlanets[planetName] = generator;
        }

        /// <summary>
        /// Unload a planet by name.
        /// </summary>
        public void UnloadPlanet(string planetName)
        {
            if (!_activePlanets.TryGetValue(planetName, out var generator)) return;

            Destroy(generator.gameObject);
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
