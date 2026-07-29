using System.Collections.Generic;
using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Manager with planet search.
    /// </summary>
    public sealed class ProceduralPlanetManager24 : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] Transform player;

        readonly Dictionary<string, PlanetSearchResult> _searchResults = new();

        void Start()
        {
            if (player == null)
            {
                var playerBody = FindFirstObjectByType<Player.SphericalGravityBody>();
                player = playerBody?.transform;
            }
        }

        /// <summary>
        /// Search for planets matching criteria.
        /// </summary>
        public PlanetSearchResult Search(string criteria)
        {
            if (_searchResults.TryGetValue(criteria, out var cached))
            {
                return cached;
            }

            var result = new PlanetSearchResult
            {
                criteria = criteria,
                matchCount = 0,
                timestamp = Time.time
            };

            _searchResults[criteria] = result;
            return result;
        }

        public class PlanetSearchResult
        {
            public string criteria;
            public int matchCount;
            public float timestamp;
            public string[] matches;
        }
    }
}
