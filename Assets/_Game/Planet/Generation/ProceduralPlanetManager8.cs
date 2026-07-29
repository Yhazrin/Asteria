using System.Collections.Generic;
using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Manager with planet streaming and LOD.
    /// </summary>
    public sealed class ProceduralPlanetManager8 : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float streamingDistance = 1000f;
        [SerializeField] float lodDistance = 500f;

        [Header("References")]
        [SerializeField] Transform player;

        readonly Dictionary<string, PlanetData> _planets = new();

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
            UpdatePlanets();
        }

        void UpdatePlanets()
        {
            if (player == null) return;

            foreach (var kvp in _planets)
            {
                float distance = Vector3.Distance(player.position, kvp.Value.position);

                // Update LOD based on distance
                if (distance < lodDistance * 0.5f)
                    kvp.Value.currentLOD = 0;
                else if (distance < lodDistance)
                    kvp.Value.currentLOD = 1;
                else if (distance < streamingDistance)
                    kvp.Value.currentLOD = 2;
                else
                    kvp.Value.currentLOD = 3;
            }
        }

        /// <summary>
        /// Register a planet.
        /// </summary>
        public void RegisterPlanet(string name, Vector3 position)
        {
            _planets[name] = new PlanetData
            {
                name = name,
                position = position,
                currentLOD = 0
            };
        }

        /// <summary>
        /// Unregister a planet.
        /// </summary>
        public void UnregisterPlanet(string name)
        {
            _planets.Remove(name);
        }

        /// <summary>
        /// Get planet data.
        /// </summary>
        public PlanetData GetPlanet(string name)
        {
            return _planets.TryGetValue(name, out var data) ? data : null;
        }

        public class PlanetData
        {
            public string name;
            public Vector3 position;
            public int currentLOD;
        }
    }
}
