using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Physics system with buoyancy.
    /// </summary>
    public sealed class ProceduralPlanetPhysics7 : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float gravityStrength = 9.81f;
        [SerializeField] float buoyancyFactor = 0.5f;

        [Header("References")]
        [SerializeField] PlanetBody planet;

        void Start()
        {
            if (planet == null)
                planet = FindFirstObjectByType<PlanetBody>();
        }

        void FixedUpdate()
        {
            ApplyBuoyancy();
        }

        void ApplyBuoyancy()
        {
            if (planet == null) return;

            var bodies = FindObjectsByType<Rigidbody>(FindObjectsSortMode.None);

            foreach (var body in bodies)
            {
                if (body.useGravity) continue;

                Vector3 direction = (planet.Center - body.position).normalized;
                float distance = Vector3.Distance(body.position, planet.Center);

                // Apply gravity
                float gravity = gravityStrength * (planet.Radius / distance);
                body.AddForce(direction * gravity, ForceMode.Acceleration);

                // Apply buoyancy if submerged
                float waterLevel = planet.Radius * 0.4f;
                if (distance < waterLevel)
                {
                    float submersion = (waterLevel - distance) / waterLevel;
                    float buoyancy = submersion * buoyancyFactor * gravityStrength;
                    body.AddForce(-direction * buoyancy, ForceMode.Acceleration);
                }
            }
        }
    }
}
