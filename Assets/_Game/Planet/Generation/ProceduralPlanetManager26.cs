using System.Collections.Generic;
using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Manager with planet filtering.
    /// </summary>
    public sealed class ProceduralPlanetManager26 : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] Transform player;

        readonly Dictionary<string, PlanetFilter> _filters = new();

        void Start()
        {
            if (player == null)
            {
                var playerBody = FindFirstObjectByType<Player.SphericalGravityBody>();
                player = playerBody?.transform;
            }
        }

        /// <summary>
        /// Add a planet filter.
        /// </summary>
        public void AddFilter(string name, System.Func<ProceduralPlanetGenerator, bool> predicate)
        {
            _filters[name] = new PlanetFilter { predicate = predicate };
        }

        /// <summary>
        /// Apply filters to a list of planets.
        /// </summary>
        public List<ProceduralPlanetGenerator> ApplyFilters(List<ProceduralPlanetGenerator> planets)
        {
            var result = new List<ProceduralPlanetGenerator>(planets);

            foreach (var filter in _filters.Values)
            {
                result.RemoveAll(p => !filter.predicate(p));
            }

            return result;
        }

        class PlanetFilter
        {
            public System.Func<ProceduralPlanetGenerator, bool> predicate;
        }
    }
}
