using System.Collections.Generic;
using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Manager with planet naming.
    /// </summary>
    public sealed class ProceduralPlanetManager31 : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] Transform player;

        readonly Dictionary<string, string> _names = new();

        void Start()
        {
            if (player == null)
            {
                var playerBody = FindFirstObjectByType<Player.SphericalGravityBody>();
                player = playerBody?.transform;
            }
        }

        /// <summary>
        /// Set planet name.
        /// </summary>
        public void SetName(string planetId, string name)
        {
            _names[planetId] = name;
        }

        /// <summary>
        /// Get planet name.
        /// </summary>
        public string GetName(string planetId)
        {
            return _names.TryGetValue(planetId, out var name) ? name : planetId;
        }
    }
}
