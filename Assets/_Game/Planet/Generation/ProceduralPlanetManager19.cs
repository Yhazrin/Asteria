using System.Collections.Generic;
using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Manager with planet snapshots.
    /// </summary>
    public sealed class ProceduralPlanetManager19 : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] Transform player;

        readonly Dictionary<string, PlanetSnapshot> _snapshots = new();

        void Start()
        {
            if (player == null)
            {
                var playerBody = FindFirstObjectByType<Player.SphericalGravityBody>();
                player = playerBody?.transform;
            }
        }

        /// <summary>
        /// Take a snapshot of planet state.
        /// </summary>
        public void TakeSnapshot(string planetName, string state)
        {
            _snapshots[planetName] = new PlanetSnapshot
            {
                state = state,
                timestamp = Time.time
            };
        }

        /// <summary>
        /// Get planet snapshot.
        /// </summary>
        public PlanetSnapshot GetSnapshot(string planetName)
        {
            return _snapshots.TryGetValue(planetName, out var snapshot) ? snapshot : null;
        }

        public class PlanetSnapshot
        {
            public string state;
            public float timestamp;
        }
    }
}
