using System.Collections.Generic;
using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Manager with planet rotation animation.
    /// </summary>
    public sealed class ProceduralPlanetManager38 : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float rotationSpeed = 0.1f;

        [Header("References")]
        [SerializeField] Transform player;

        readonly Dictionary<string, bool> _rotating = new();

        void Start()
        {
            if (player == null)
            {
                var playerBody = FindFirstObjectByType<Player.SphericalGravityBody>();
                player = playerBody?.transform;
            }
        }

        void Update()
        {
            UpdateRotations();
        }

        void UpdateRotations()
        {
            // Rotate all marked planets
            foreach (var kvp in _rotating)
            {
                if (!kvp.Value) continue;

                var planet = GameObject.Find(kvp.Key);
                if (planet != null)
                {
                    planet.transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
                }
            }
        }

        /// <summary>
        /// Enable rotation for a planet.
        /// </summary>
        public void EnableRotation(string planetName)
        {
            _rotating[planetName] = true;
        }

        /// <summary>
        /// Disable rotation for a planet.
        /// </summary>
        public void DisableRotation(string planetName)
        {
            _rotating[planetName] = false;
        }
    }
}
