using System.Collections.Generic;
using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Manager with planet scaling.
    /// </summary>
    public sealed class ProceduralPlanetManager34 : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] Transform player;

        readonly Dictionary<string, float> _scales = new();

        void Start()
        {
            if (player == null)
            {
                var playerBody = FindFirstObjectByType<Player.SphericalGravityBody>();
                player = playerBody?.transform;
            }
        }

        /// <summary>
        /// Set planet scale.
        /// </summary>
        public void SetScale(string planetName, float scale)
        {
            _scales[planetName] = Mathf.Clamp(scale, 0.1f, 10f);
        }

        /// <summary>
        /// Get planet scale.
        /// </summary>
        public float GetScale(string planetName)
        {
            return _scales.TryGetValue(planetName, out var scale) ? scale : 1f;
        }
    }
}
