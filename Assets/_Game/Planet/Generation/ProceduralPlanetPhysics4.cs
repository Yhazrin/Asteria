using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Physics system with orbital mechanics.
    /// </summary>
    public sealed class ProceduralPlanetPhysics4 : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float gravityStrength = 9.81f;
        [SerializeField] float orbitalSpeed = 0.1f;

        [Header("References")]
        [SerializeField] PlanetBody planet;

        void Start()
        {
            if (planet == null)
                planet = FindFirstObjectByType<PlanetBody>();
        }

        void FixedUpdate()
        {
            ApplyOrbitalMechanics();
        }

        void ApplyOrbitalMechanics()
        {
            if (planet == null) return;

            // Apply gravity
            var bodies = FindObjectsByType<Rigidbody>(FindObjectsSortMode.None);
            foreach (var body in bodies)
            {
                if (body.useGravity) continue;

                Vector3 direction = (planet.Center - body.position).normalized;
                float distance = Vector3.Distance(body.position, planet.Center);

                float gravity = gravityStrength * (planet.Radius / distance);
                body.AddForce(direction * gravity, ForceMode.Acceleration);
            }

            // Rotate planet
            planet.transform.Rotate(Vector3.up, orbitalSpeed * Time.fixedDeltaTime);
        }
    }
}
