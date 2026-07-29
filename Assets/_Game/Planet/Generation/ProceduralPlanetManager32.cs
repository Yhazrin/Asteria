using System.Collections.Generic;
using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Manager with planet rotation.
    /// </summary>
    public sealed class ProceduralPlanetManager32 : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float rotationSpeed = 0.1f;

        [Header("References")]
        [SerializeField] Transform player;

        readonly Dictionary<string, float> _rotations = new();

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
            foreach (var kvp in _rotations.Keys)
            {
                _rotations[kvp] += rotationSpeed * Time.deltaTime;
            }
        }

        /// <summary>
        /// Set planet rotation.
        /// </summary>
        public void SetRotation(string planetName, float rotation)
        {
            _rotations[planetName] = rotation;
        }

        /// <summary>
        /// Get planet rotation.
        /// </summary>
        public float GetRotation(string planetName)
        {
            return _rotations.TryGetValue(planetName, out var rotation) ? rotation : 0f;
        }
    }
}
