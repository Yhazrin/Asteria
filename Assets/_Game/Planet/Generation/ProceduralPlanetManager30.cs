using System.Collections.Generic;
using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Manager with planet visualization.
    /// </summary>
    public sealed class ProceduralPlanetManager30 : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] Transform player;

        readonly Dictionary<string, PlanetVisualization> _visualizations = new();

        void Start()
        {
            if (player == null)
            {
                var playerBody = FindFirstObjectByType<Player.SphericalGravityBody>();
                player = playerBody?.transform;
            }
        }

        /// <summary>
        /// Set planet visualization.
        /// </summary>
        public void SetVisualization(string planetName, Color color, float opacity)
        {
            _visualizations[planetName] = new PlanetVisualization
            {
                color = color,
                opacity = opacity
            };
        }

        /// <summary>
        /// Get planet visualization.
        /// </summary>
        public PlanetVisualization GetVisualization(string planetName)
        {
            return _visualizations.TryGetValue(planetName, out var viz) ? viz : null;
        }

        public class PlanetVisualization
        {
            public Color color;
            public float opacity;
        }
    }
}
