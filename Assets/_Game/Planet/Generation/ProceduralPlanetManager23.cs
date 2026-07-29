using System.Collections.Generic;
using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Manager with planet history.
    /// </summary>
    public sealed class ProceduralPlanetManager23 : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] Transform player;

        readonly List<PlanetHistoryEntry> _history = new();

        void Start()
        {
            if (player == null)
            {
                var playerBody = FindFirstObjectByType<Player.SphericalGravityBody>();
                player = playerBody?.transform;
            }
        }

        /// <summary>
        /// Record a planet event.
        /// </summary>
        public void RecordEvent(string planetName, string eventType, string details)
        {
            _history.Add(new PlanetHistoryEntry
            {
                planetName = planetName,
                eventType = eventType,
                details = details,
                timestamp = Time.time
            });
        }

        /// <summary>
        /// Get planet history.
        /// </summary>
        public List<PlanetHistoryEntry> GetHistory(string planetName)
        {
            return _history.FindAll(e => e.planetName == planetName);
        }

        public class PlanetHistoryEntry
        {
            public string planetName;
            public string eventType;
            public string details;
            public float timestamp;
        }
    }
}
