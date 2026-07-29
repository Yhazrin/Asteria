using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Physics system with spring forces.
    /// </summary>
    public sealed class ProceduralPlanetPhysics10 : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float gravityStrength = 9.81f;
        [SerializeField] float springStrength = 10f;
        [SerializeField] float springDamping = 2f;

        [Header("References")]
        [SerializeField] PlanetBody planet;

        void Start()
        {
            if (planet == null)
                planet = FindFirstObjectByType<PlanetBody>();
        }

        void FixedUpdate()
        {
            ApplySpringForces();
        }

        void ApplySpringForces()
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
                body.AddForce(direction * gravity, ForceMode.Acceleration);

                // Spring force (pushes away from surface when too close)
                if (altitude < 2f)
                {
                    float spring = springStrength * (2f - altitude);
                    float damping = springDamping * Vector3.Dot(body.linearVelocity, direction);
                    body.AddForce(-direction * (spring + damping), ForceMode.Acceleration);
                }
            }
        }
    }
}
