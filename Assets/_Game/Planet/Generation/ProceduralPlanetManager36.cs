using System.Collections.Generic;
using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Manager with planet validation.
    /// </summary>
    public sealed class ProceduralPlanetManager36 : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] Transform player;

        void Start()
        {
            if (player == null)
            {
                var playerBody = FindFirstObjectByType<Player.SphericalGravityBody>();
                player = playerBody?.transform;
            }
        }

        /// <summary>
        /// Validate planet data.
        /// </summary>
        public bool ValidatePlanet(string planetName, float radius, int seed)
        {
            if (string.IsNullOrEmpty(planetName)) return false;
            if (radius <= 0) return false;
            if (seed < 0) return false;

            return true;
        }
    }
}
