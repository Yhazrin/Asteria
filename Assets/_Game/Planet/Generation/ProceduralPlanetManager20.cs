using System.Collections.Generic;
using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Manager with planet export.
    /// </summary>
    public sealed class ProceduralPlanetManager20 : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] Transform player;

        void Start()
        {
            if (player == null)
            {
                var playerBody = FindFirstObjectByType<Player.SphericalGravityBody>();
                player = playerBody?.transform;
            }
        }

        /// <summary>
        /// Export planet data to JSON.
        /// </summary>
        public string ExportToJson(string planetName, float radius, int seed)
        {
            var data = new PlanetExport
            {
                name = planetName,
                radius = radius,
                seed = seed,
                exportTime = System.DateTime.UtcNow.ToString("o")
            };

            return JsonUtility.ToJson(data, true);
        }

        /// <summary>
        /// Import planet data from JSON.
        /// </summary>
        public PlanetExport ImportFromJson(string json)
        {
            return JsonUtility.FromJson<PlanetExport>(json);
        }

        [System.Serializable]
        public class PlanetExport
        {
            public string name;
            public float radius;
            public int seed;
            public string exportTime;
        }
    }
}
