using System.Collections.Generic;
using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Manager with planet orbiting.
    /// </summary>
    public sealed class ProceduralPlanetManager39 : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float orbitSpeed = 0.01f;

        [Header("References")]
        [SerializeField] Transform player;

        readonly Dictionary<string, OrbitData> _orbits = new();

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
            UpdateOrbits();
        }

        void UpdateOrbits()
        {
            foreach (var kvp in _orbits)
            {
                var planet = GameObject.Find(kvp.Key);
                if (planet == null) continue;

                kvp.Value.angle += kvp.Value.speed * Time.deltaTime;
                float x = Mathf.Cos(kvp.Value.angle) * kvp.Value.radius;
                float z = Mathf.Sin(kvp.Value.angle) * kvp.Value.radius;
                planet.transform.position = kvp.Value.center + new Vector3(x, 0, z);
            }
        }

        /// <summary>
        /// Set planet orbit.
        /// </summary>
        public void SetOrbit(string planetName, Vector3 center, float radius, float speed)
        {
            _orbits[planetName] = new OrbitData
            {
                center = center,
                radius = radius,
                speed = speed,
                angle = 0f
            };
        }

        class OrbitData
        {
            public Vector3 center;
            public float radius;
            public float speed;
            public float angle;
        }
    }
}
