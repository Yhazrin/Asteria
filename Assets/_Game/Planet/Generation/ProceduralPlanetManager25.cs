using System.Collections.Generic;
using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Manager with planet duplication.
    /// </summary>
    public sealed class ProceduralPlanetManager25 : MonoBehaviour
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
        /// Duplicate a planet.
        /// </summary>
        public ProceduralPlanetGenerator DuplicatePlanet(ProceduralPlanetGenerator source, string newName)
        {
            if (source == null) return null;

            var go = new GameObject(newName);
            go.transform.SetParent(transform, false);
            go.transform.position = source.transform.position + Vector3.right * 1000f;

            var generator = go.AddComponent<ProceduralPlanetGenerator>();
            generator.Generate();

            return generator;
        }
    }
}
