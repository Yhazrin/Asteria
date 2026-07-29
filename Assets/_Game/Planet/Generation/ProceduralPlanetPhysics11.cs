using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Physics system with constraint-based movement.
    /// </summary>
    public sealed class ProceduralPlanetPhysics11 : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float gravityStrength = 9.81f;
        [SerializeField] float constraintStrength = 100f;

        [Header("References")]
        [SerializeField] PlanetBody planet;

        void Start()
        {
            if (planet == null)
                planet = FindFirstObjectByType<PlanetBody>();
        }

        void FixedUpdate()
        {
            ApplyConstraints();
        }

        void ApplyConstraints()
        {
            if (planet == null) return;

            var bodies = FindObjectsByType<Rigidbody>(FindObjectsSortMode.None);

            foreach (var body in bodies)
            {
                if (body.useGravity) continue;

                Vector3 direction = (planet.Center - body.position).normalized;
                float distance = Vector3.Distance(body.position, planet.Center);

                // Gravity
                float gravity = gravityStrength * (planet.Radius / distance);
                body.AddForce(direction * gravity, ForceMode.Acceleration);

                // Keep on surface constraint
                float altitude = distance - planet.Radius;
                if (altitude > 5f)
                {
                    // Pull back to surface
                    float constraint = constraintStrength * (altitude - 5f);
                    body.AddForce(direction * constraint, ForceMode.Acceleration);
                }
            }
        }
    }
}
