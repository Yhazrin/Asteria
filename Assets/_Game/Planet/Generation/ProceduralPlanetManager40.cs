using System.Collections.Generic;
using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Manager with planet pulsing.
    /// </summary>
    public sealed class ProceduralPlanetManager40 : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float pulseSpeed = 0.5f;

        [Header("References")]
        [SerializeField] Transform player;

        readonly Dictionary<string, bool> _pulsing = new();

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
            UpdatePulsing();
        }

        void UpdatePulsing()
        {
            foreach (var kvp in _pulsing)
            {
                if (!kvp.Value) continue;

                var planet = GameObject.Find(kvp.Key);
                if (planet == null) continue;

                float pulse = Mathf.Sin(Time.time * pulseSpeed) * 0.1f;
                planet.transform.localScale = Vector3.one * (1f + pulse);
            }
        }

        /// <summary>
        /// Enable pulsing for a planet.
        /// </summary>
        public void EnablePulsing(string planetName)
        {
            _pulsing[planetName] = true;
        }

        /// <summary>
        /// Disable pulsing for a planet.
        /// </summary>
        public void DisablePulsing(string planetName)
        {
            _pulsing[planetName] = false;
        }
    }
}
