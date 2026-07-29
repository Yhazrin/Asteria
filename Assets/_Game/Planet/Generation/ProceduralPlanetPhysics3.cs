using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Physics system with N-body gravity simulation.
    /// </summary>
    public sealed class ProceduralPlanetPhysics3 : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float gravityStrength = 9.81f;
        [SerializeField] float gravityConstant = 6.674e-11f;

        [Header("References")]
        [SerializeField] PlanetBody[] planets;

        void Start()
        {
            if (planets == null || planets.Length == 0)
            {
                planets = FindObjectsByType<PlanetBody>(FindObjectsSortMode.None);
            }
        }

        void FixedUpdate()
        {
            ApplyNBodyGravity();
        }

        void ApplyNBodyGravity()
        {
            var bodies = FindObjectsByType<Rigidbody>(FindObjectsSortMode.None);

            foreach (var body in bodies)
            {
                if (body.useGravity) continue;

                Vector3 totalForce = Vector3.zero;

                foreach (var planet in planets)
                {
                    if (planet == null) continue;

                    Vector3 direction = (planet.Center - body.position).normalized;
                    float distance = Vector3.Distance(body.position, planet.Center);

                    if (distance < 1f) continue;

                    float force = gravityStrength * (planet.Radius * planet.Radius) / (distance * distance);
                    totalForce += direction * force;
                }

                body.AddForce(totalForce, ForceMode.Acceleration);
            }
        }
    }
}
