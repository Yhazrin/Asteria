using System.Collections.Generic;
using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Manager with planet statistics.
    /// </summary>
    public sealed class ProceduralPlanetManager16 : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] Transform player;

        readonly Dictionary<string, PlanetStats> _stats = new();

        void Start()
        {
            if (player == null)
            {
                var playerBody = FindFirstObjectByType<Player.SphericalGravityBody>();
                player = playerBody?.transform;
            }
        }

        /// <summary>
        /// Record planet statistics.
        /// </summary>
        public void RecordStats(string planetName, int vertexCount, int triangleCount)
        {
            _stats[planetName] = new PlanetStats
            {
                vertexCount = vertexCount,
                triangleCount = triangleCount,
                timestamp = Time.time
            };
        }

        /// <summary>
        /// Get planet statistics.
        /// </summary>
        public PlanetStats GetStats(string planetName)
        {
            return _stats.TryGetValue(planetName, out var stats) ? stats : null;
        }

        /// <summary>
        /// Get total vertex count across all planets.
        /// </summary>
        public int GetTotalVertexCount()
        {
            int total = 0;
            foreach (var stats in _stats.Values)
            {
                total += stats.vertexCount;
            }
            return total;
        }

        public class PlanetStats
        {
            public int vertexCount;
            public int triangleCount;
            public float timestamp;
        }
    }
}
