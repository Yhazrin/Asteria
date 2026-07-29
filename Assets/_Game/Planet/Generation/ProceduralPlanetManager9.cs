using System.Collections.Generic;
using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Manager with planet visibility culling.
    /// </summary>
    public sealed class ProceduralPlanetManager9 : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float cullingDistance = 2000f;
        [SerializeField] float frustumPadding = 1.2f;

        [Header("References")]
        [SerializeField] Transform player;
        [SerializeField] Camera mainCamera;

        readonly Dictionary<string, PlanetVisibility> _planets = new();

        void Start()
        {
            if (player == null)
            {
                var playerBody = FindFirstObjectByType<Player.SphericalGravityBody>();
                player = playerBody?.transform;
            }
            if (mainCamera == null)
                mainCamera = Camera.main;
        }

        void Update()
        {
            UpdateVisibility();
        }

        void UpdateVisibility()
        {
            if (mainCamera == null) return;

            Plane[] frustumPlanes = GeometryUtility.CalculateFrustumPlanes(mainCamera);

            foreach (var kvp in _planets)
            {
                var visibility = kvp.Value;

                // Distance culling
                float distance = player != null
                    ? Vector3.Distance(player.position, visibility.position)
                    : Vector3.Distance(mainCamera.transform.position, visibility.position);

                bool distanceVisible = distance < cullingDistance;

                // Frustum culling
                Bounds bounds = new Bounds(visibility.position, Vector3.one * visibility.radius * 2f);
                bool frustumVisible = GeometryUtility.TestPlanesAABB(frustumPlanes, bounds);

                visibility.isVisible = distanceVisible && frustumVisible;
            }
        }

        /// <summary>
        /// Register a planet for visibility culling.
        /// </summary>
        public void RegisterPlanet(string name, Vector3 position, float radius)
        {
            _planets[name] = new PlanetVisibility
            {
                position = position,
                radius = radius,
                isVisible = true
            };
        }

        /// <summary>
        /// Unregister a planet.
        /// </summary>
        public void UnregisterPlanet(string name)
        {
            _planets.Remove(name);
        }

        /// <summary>
        /// Check if a planet is visible.
        /// </summary>
        public bool IsVisible(string name)
        {
            return _planets.TryGetValue(name, out var visibility) && visibility.isVisible;
        }

        class PlanetVisibility
        {
            public Vector3 position;
            public float radius;
            public bool isVisible;
        }
    }
}
