using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Physics system with wind resistance.
    /// </summary>
    public sealed class ProceduralPlanetPhysics17 : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float gravityStrength = 9.81f;
        [SerializeField] float windResistance = 0.1f;

        [Header("References")]
        [SerializeField] PlanetBody planet;

        void Start()
        {
            if (planet == null)
                planet = FindFirstObjectByType<PlanetBody>();
        }

        void FixedUpdate()
        {
            ApplyWindResistance();
        }

        void ApplyWindResistance()
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

                // Wind resistance (opposes horizontal movement)
                Vector3 velocity = body.linearVelocity;
                Vector3 horizontalVelocity = velocity - Vector3.Dot(velocity, direction) * direction;

                if (horizontalVelocity.magnitude > 0.1f)
                {
                    Vector3 resistance = -horizontalVelocity.normalized * (horizontalVelocity.magnitude * windResistance);
                    body.AddForce(resistance, ForceMode.Acceleration);
                }
            }
        }
    }
}
