using System.Collections.Generic;
using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Manager with planet rippling.
    /// </summary>
    public sealed class ProceduralPlanetManager41 : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float rippleSpeed = 0.5f;

        [Header("References")]
        [SerializeField] Transform player;

        readonly Dictionary<string, bool> _rippling = new();

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
            UpdateRippling();
        }

        void UpdateRippling()
        {
            foreach (var kvp in _rippling)
            {
                if (!kvp.Value) continue;

                var planet = GameObject.Find(kvp.Key);
                if (planet == null) continue;

                float ripple = Mathf.Sin(Time.time * rippleSpeed) * 0.05f;
                planet.transform.localScale = Vector3.one * (1f + ripple);
            }
        }

        /// <summary>
        /// Enable rippling for a planet.
        /// </summary>
        public void EnableRippling(string planetName)
        {
            _rippling[planetName] = true;
        }

        /// <summary>
        /// Disable rippling for a planet.
        /// </summary>
        public void DisableRippling(string planetName)
        {
            _rippling[planetName] = false;
        }
    }
}
