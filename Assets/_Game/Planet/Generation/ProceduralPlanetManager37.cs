using System.Collections.Generic;
using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Manager with planet cloning.
    /// </summary>
    public sealed class ProceduralPlanetManager37 : MonoBehaviour
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
        /// Clone a planet.
        /// </summary>
        public ProceduralPlanetGenerator ClonePlanet(ProceduralPlanetGenerator source, string newName)
        {
            if (source == null) return null;

            var go = Instantiate(source.gameObject, transform);
            go.name = newName;

            var generator = go.GetComponent<ProceduralPlanetGenerator>();
            return generator;
        }
    }
}
