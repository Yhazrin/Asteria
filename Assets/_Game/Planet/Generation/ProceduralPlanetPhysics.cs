using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Physics system for procedural planets.
    /// Handles gravity, collisions, and physics interactions.
    /// </summary>
    public sealed class ProceduralPlanetPhysics : MonoBehaviour
    {
        [Header("Gravity")]
        [SerializeField] float gravityStrength = 9.81f;
        [SerializeField] float gravityFalloff = 2f;

        [Header("Collisions")]
        [SerializeField] float collisionRadius = 0.5f;
        [SerializeField] LayerMask collisionMask = ~0;

        [Header("References")]
        [SerializeField] PlanetBody planet;

        void Start()
        {
            if (planet == null)
                planet = FindFirstObjectByType<PlanetBody>();
        }

        void FixedUpdate()
        {
            ApplyGravity();
        }

        void ApplyGravity()
        {
            if (planet == null) return;

            // Apply gravity to all rigidbodies
            var bodies = FindObjectsByType<Rigidbody>(FindObjectsSortMode.None);
            foreach (var body in bodies)
            {
                if (body.useGravity) continue;

                Vector3 direction = (planet.Center - body.position).normalized;
                float distance = Vector3.Distance(body.position, planet.Center);

                // Gravity falls off with distance
                float gravity = gravityStrength * (planet.Radius / distance);
                gravity = Mathf.Min(gravity, gravityStrength * 2f);

                body.AddForce(direction * gravity, ForceMode.Acceleration);
            }
        }

        /// <summary>
        /// Get the gravity direction at a position.
        /// </summary>
        public Vector3 GetGravityDirection(Vector3 position)
        {
            if (planet == null) return Vector3.down;
            return (planet.Center - position).normalized;
        }

        /// <summary>
        /// Get the gravity strength at a position.
        /// </summary>
        public float GetGravityStrength(Vector3 position)
        {
            if (planet == null) return gravityStrength;

            float distance = Vector3.Distance(position, planet.Center);
            return gravityStrength * (planet.Radius / distance);
        }

        /// <summary>
        /// Check if a position is on the planet surface.
        /// </summary>
        public bool IsOnSurface(Vector3 position, float threshold = 1f)
        {
            if (planet == null) return false;

            float distance = Vector3.Distance(position, planet.Center);
            return Mathf.Abs(distance - planet.Radius) < threshold;
        }

        /// <summary>
        /// Get the surface normal at a position.
        /// </summary>
        public Vector3 GetSurfaceNormal(Vector3 position)
        {
            if (planet == null) return Vector3.up;
            return (position - planet.Center).normalized;
        }
    }
}
