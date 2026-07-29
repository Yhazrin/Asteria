using System.Collections.Generic;
using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Manager with planet deletion.
    /// </summary>
    public sealed class ProceduralPlanetManager33 : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] Transform player;

        readonly Dictionary<string, ProceduralPlanetGenerator> _planets = new();

        void Start()
        {
            if (player == null)
            {
                var playerBody = FindFirstObjectByType<Player.SphericalGravityBody>();
                player = playerBody?.transform;
            }
        }

        /// <summary>
        /// Delete a planet.
        /// </summary>
        public bool DeletePlanet(string planetName)
        {
            if (!_planets.TryGetValue(planetName, out var planet)) return false;

            Destroy(planet.gameObject);
            _planets.Remove(planetName);

            Debug.Log($"[ProceduralPlanetManager33] Deleted planet: {planetName}");
            return true;
        }

        /// <summary>
        /// Register a planet.
        /// </summary>
        public void RegisterPlanet(string name, ProceduralPlanetGenerator planet)
        {
            _planets[name] = planet;
        }
    }
}
