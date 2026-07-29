using System.Collections.Generic;
using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Manager with planet backup.
    /// </summary>
    public sealed class ProceduralPlanetManager18 : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] Transform player;

        readonly Dictionary<string, PlanetBackup> _backups = new();

        void Start()
        {
            if (player == null)
            {
                var playerBody = FindFirstObjectByType<Player.SphericalGravityBody>();
                player = playerBody?.transform;
            }
        }

        /// <summary>
        /// Create a backup of planet data.
        /// </summary>
        public void CreateBackup(string planetName, string data)
        {
            _backups[planetName] = new PlanetBackup
            {
                data = data,
                timestamp = Time.time
            };
        }

        /// <summary>
        /// Restore planet from backup.
        /// </summary>
        public string RestoreBackup(string planetName)
        {
            return _backups.TryGetValue(planetName, out var backup) ? backup.data : null;
        }

        /// <summary>
        /// Check if backup exists.
        /// </summary>
        public bool HasBackup(string planetName)
        {
            return _backups.ContainsKey(planetName);
        }

        class PlanetBackup
        {
            public string data;
            public float timestamp;
        }
    }
}
