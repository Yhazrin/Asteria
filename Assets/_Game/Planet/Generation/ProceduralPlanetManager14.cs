using System.Collections.Generic;
using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Manager with planet serialization.
    /// </summary>
    public sealed class ProceduralPlanetManager14 : MonoBehaviour
    {
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

        /// <summary>
        /// Serialize planet data.
        /// </summary>
        public string SerializePlanet(string planetName)
        {
            if (!_planets.TryGetValue(planetName, out var data))
                return null;

            return JsonUtility.ToJson(data);
        }

        /// <summary>
        /// Deserialize planet data.
        /// </summary>
        public PlanetData DeserializePlanet(string json)
        {
            return JsonUtility.FromJson<PlanetData>(json);
        }

        /// <summary>
        /// Register a planet.
        /// </summary>
        public void RegisterPlanet(string name, PlanetData data)
        {
            _planets[name] = data;
        }

        [System.Serializable]
        public class PlanetData
        {
            public string name;
            public float radius;
            public int seed;
            public string[] biomeNames;
        }
    }
}
