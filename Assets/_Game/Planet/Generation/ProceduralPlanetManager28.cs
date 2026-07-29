using System.Collections.Generic;
using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Manager with planet tagging.
    /// </summary>
    public sealed class ProceduralPlanetManager28 : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] Transform player;

        readonly Dictionary<string, HashSet<string>> _tags = new();

        void Start()
        {
            if (player == null)
            {
                var playerBody = FindFirstObjectByType<Player.SphericalGravityBody>();
                player = playerBody?.transform;
            }
        }

        /// <summary>
        /// Add a tag to a planet.
        /// </summary>
        public void AddTag(string planetName, string tag)
        {
            if (!_tags.TryGetValue(planetName, out var tagSet))
            {
                tagSet = new HashSet<string>();
                _tags[planetName] = tagSet;
            }

            tagSet.Add(tag);
        }

        /// <summary>
        /// Check if a planet has a tag.
        /// </summary>
        public bool HasTag(string planetName, string tag)
        {
            return _tags.TryGetValue(planetName, out var tagSet) && tagSet.Contains(tag);
        }

        /// <summary>
        /// Get all tags for a planet.
        /// </summary>
        public HashSet<string> GetTags(string planetName)
        {
            return _tags.TryGetValue(planetName, out var tagSet) ? tagSet : new HashSet<string>();
        }
    }
}
