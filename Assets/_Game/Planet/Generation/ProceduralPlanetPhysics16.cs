using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Physics system with water buoyancy.
    /// </summary>
    public sealed class ProceduralPlanetPhysics16 : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float gravityStrength = 9.81f;
        [SerializeField] float waterDensity = 1000f;
        [SerializeField] float objectDensity = 800f;

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

                // Gravity
                float gravity = gravityStrength * (planet.Radius / distance);
                body.AddForce(direction * gravity, ForceMode.Acceleration);

                // Buoyancy in water
                float waterLevel = planet.Radius * 0.4f;
                if (distance < waterLevel)
                {
                    float submergedVolume = (waterLevel - distance) / waterLevel;
                    float buoyancyForce = submergedVolume * waterDensity * gravityStrength;
                    float weight = objectDensity * gravityStrength;

                    if (buoyancyForce > weight)
                    {
                        body.AddForce(-direction * (buoyancyForce - weight), ForceMode.Acceleration);
                    }
                }
            }
        }
    }
}
