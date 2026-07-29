using System.Collections.Generic;
using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Manager with planet versioning.
    /// </summary>
    public sealed class ProceduralPlanetManager17 : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] Transform player;

        readonly Dictionary<string, int> _versions = new();

        void Start()
        {
            if (player == null)
            {
                var playerBody = FindFirstObjectByType<Player.SphericalGravityBody>();
                player = playerBody?.transform;
            }
        }

        /// <summary>
        /// Get planet version.
        /// </summary>
        public int GetVersion(string planetName)
        {
            return _versions.TryGetValue(planetName, out var version) ? version : 0;
        }

        /// <summary>
        /// Increment planet version.
        /// </summary>
        public int IncrementVersion(string planetName)
        {
            int version = GetVersion(planetName) + 1;
            _versions[planetName] = version;
            return version;
        }

        /// <summary>
        /// Check if planet needs regeneration.
        /// </summary>
        public bool NeedsRegeneration(string planetName, int expectedVersion)
        {
            return GetVersion(planetName) < expectedVersion;
        }
    }
}
