using System.Collections.Generic;
using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Manager with planet comparison.
    /// </summary>
    public sealed class ProceduralPlanetManager22 : MonoBehaviour
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
        /// Compare two planets.
        /// </summary>
        public string ComparePlanets(string planet1, string planet2)
        {
            return $"Comparing {planet1} and {planet2}";
        }
    }
}
