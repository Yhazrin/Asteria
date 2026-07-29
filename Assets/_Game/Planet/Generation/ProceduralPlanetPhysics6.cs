using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Physics system with variable gravity based on altitude.
    /// </summary>
    public sealed class ProceduralPlanetPhysics6 : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float surfaceGravity = 9.81f;
        [SerializeField] float gravityFalloff = 2f;

        [Header("References")]
        [SerializeField] PlanetBody planet;

        void Start()
        {
            if (planet == null)
                planet = FindFirstObjectByType<PlanetBody>();
        }

        void FixedUpdate()
        {
            ApplyVariableGravity();
        }

        void ApplyVariableGravity()
        {
            if (planet == null) return;

            var bodies = FindObjectsByType<Rigidbody>(FindObjectsSortMode.None);

            foreach (var body in bodies)
            {
                if (body.useGravity) continue;

                Vector3 direction = (planet.Center - body.position).normalized;
                float distance = Vector3.Distance(body.position, planet.Center);
                float altitude = distance - planet.Radius;

                // Gravity decreases with altitude
                float gravity = surfaceGravity * Mathf.Pow(planet.Radius / distance, gravityFalloff);
                gravity = Mathf.Max(gravity, surfaceGravity * 0.1f); // Minimum gravity

                body.AddForce(direction * gravity, ForceMode.Acceleration);
            }
        }

        /// <summary>
        /// Get gravity at a specific altitude.
        /// </summary>
        public float GetGravityAtAltitude(float altitude)
        {
            if (planet == null) return surfaceGravity;

            float distance = planet.Radius + altitude;
            return surfaceGravity * Mathf.Pow(planet.Radius / distance, gravityFalloff);
        }

        /// <summary>
        /// Get escape velocity at a specific altitude.
        /// </summary>
        public float GetEscapeVelocity(float altitude)
        {
            if (planet == null) return 0f;

            float distance = planet.Radius + altitude;
            return Mathf.Sqrt(2f * surfaceGravity * planet.Radius * planet.Radius / distance);
        }
    }
}
