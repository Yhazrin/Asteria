using System.Collections.Generic;
using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Manager with planet merging.
    /// </summary>
    public sealed class ProceduralPlanetManager29 : MonoBehaviour
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
        /// Merge two planets into one.
        /// </summary>
        public ProceduralPlanetGenerator MergePlanets(ProceduralPlanetGenerator planet1, ProceduralPlanetGenerator planet2, string newName)
        {
            if (planet1 == null || planet2 == null) return null;

            var go = new GameObject(newName);
            go.transform.SetParent(transform, false);

            // Position between the two planets
            go.transform.position = (planet1.transform.position + planet2.transform.position) * 0.5f;

            var generator = go.AddComponent<ProceduralPlanetGenerator>();
            generator.Generate();

            return generator;
        }
    }
}
