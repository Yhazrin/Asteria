using System.Collections.Generic;
using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Manager with planet locking.
    /// </summary>
    public sealed class ProceduralPlanetManager35 : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] Transform player;

        readonly HashSet<string> _lockedPlanets = new();

        void Start()
        {
            if (player == null)
            {
                var playerBody = FindFirstObjectByType<Player.SphericalGravityBody>();
                player = playerBody?.transform;
            }
        }

        /// <summary>
        /// Lock a planet (prevent modifications).
        /// </summary>
        public void LockPlanet(string planetName)
        {
            _lockedPlanets.Add(planetName);
        }

        /// <summary>
        /// Unlock a planet.
        /// </summary>
        public void UnlockPlanet(string planetName)
        {
            _lockedPlanets.Remove(planetName);
        }

        /// <summary>
        /// Check if planet is locked.
        /// </summary>
        public bool IsLocked(string planetName)
        {
            return _lockedPlanets.Contains(planetName);
        }
    }
}
