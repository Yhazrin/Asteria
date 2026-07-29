using System.Collections.Generic;
using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Manager with planet merging.
    /// </summary>
    public sealed class ProceduralPlanetManager11 : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float mergeDistance = 100f;

        [Header("References")]
        [SerializeField] Transform player;

        readonly List<PlanetGroup> _groups = new();

        void Start()
        {
            if (player == null)
            {
                var playerBody = FindFirstObjectByType<Player.SphericalGravityBody>();
                player = playerBody?.transform;
            }
        }

        /// <summary>
        /// Merge nearby planets into groups.
        /// </summary>
        public void MergeNearby()
        {
            // Simple grouping based on distance
            for (int i = 0; i < _groups.Count; i++)
            {
                for (int j = i + 1; j < _groups.Count; j++)
                {
                    float distance = Vector3.Distance(
                        _groups[i].center,
                        _groups[j].center);

                    if (distance < mergeDistance)
                    {
                        // Merge groups
                        _groups[i].planets.AddRange(_groups[j].planets);
                        _groups.RemoveAt(j);
                        j--;
                    }
                }
            }
        }

        /// <summary>
        /// Add a planet to a group.
        /// </summary>
        public void AddPlanet(string groupName, ProceduralPlanetGenerator planet)
        {
            var group = _groups.Find(g => g.name == groupName);
            if (group == null)
            {
                group = new PlanetGroup { name = groupName, center = planet.transform.position };
                _groups.Add(group);
            }

            group.planets.Add(planet);
        }

        class PlanetGroup
        {
            public string name;
            public Vector3 center;
            public List<ProceduralPlanetGenerator> planets = new();
        }
    }
}
