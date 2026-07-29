using System.Collections.Generic;
using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Manager with planet metrics.
    /// </summary>
    public sealed class ProceduralPlanetManager13 : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] Transform player;

        readonly Dictionary<string, PlanetMetrics> _metrics = new();

        void Start()
        {
            if (player == null)
            {
                var playerBody = FindFirstObjectByType<Player.SphericalGravityBody>();
                player = playerBody?.transform;
            }
        }

        /// <summary>
        /// Record planet metrics.
        /// </summary>
        public void RecordMetrics(string planetName, int vertexCount, int triangleCount, float loadTime)
        {
            _metrics[planetName] = new PlanetMetrics
            {
                vertexCount = vertexCount,
                triangleCount = triangleCount,
                loadTime = loadTime,
                timestamp = Time.time
            };
        }

        /// <summary>
        /// Get planet metrics.
        /// </summary>
        public PlanetMetrics GetMetrics(string planetName)
        {
            return _metrics.TryGetValue(planetName, out var metrics) ? metrics : null;
        }

        public class PlanetMetrics
        {
            public int vertexCount;
            public int triangleCount;
            public float loadTime;
            public float timestamp;
        }
    }
}
