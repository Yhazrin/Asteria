using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Physics system with atmospheric drag.
    /// </summary>
    public sealed class ProceduralPlanetPhysics8 : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float gravityStrength = 9.81f;
        [SerializeField] float dragCoefficient = 0.1f;
        [SerializeField] float atmosphericDensity = 1.225f;

        [Header("References")]
        [SerializeField] PlanetBody planet;

        void Start()
        {
            if (planet == null)
                planet = FindFirstObjectByType<PlanetBody>();
        }

        void FixedUpdate()
        {
            ApplyPhysics();
        }

        void ApplyPhysics()
        {
            if (planet == null) return;

            var bodies = FindObjectsByType<Rigidbody>(FindObjectsSortMode.None);

            foreach (var body in bodies)
            {
                if (body.useGravity) continue;

                Vector3 direction = (planet.Center - body.position).normalized;
                float distance = Vector3.Distance(body.position, planet.Center);
                float altitude = distance - planet.Radius;

                // Gravity
                float gravity = gravityStrength * (planet.Radius / distance);
                body.AddForce(direction * gravity, ForceMode.Accentrication);

                // Atmospheric drag (decreases with altitude)
                if (altitude < 100f)
                {
                    float density = atmosphericDensity * Mathf.Exp(-altitude / 10f);
                    Vector3 velocity = body.linearVelocity;
                    float speed = velocity.magnitude;

                    if (speed > 0.1f)
                    {
                        Vector3 dragForce = -velocity.normalized * (0.5f * density * speed * speed * dragCoefficient);
                        body.AddForce(dragForce, ForceMode.Acceleration);
                    }
                }
            }
        }
    }
}
