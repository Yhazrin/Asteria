using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Physics system with anti-gravity zones.
    /// </summary>
    public sealed class ProceduralPlanetPhysics13 : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float gravityStrength = 9.81f;
        [SerializeField] float antiGravityStrength = 5f;

        [Header("References")]
        [SerializeField] PlanetBody planet;
        [SerializeField] Transform[] antiGravityZones;

        void Start()
        {
            if (planet == null)
                planet = FindFirstObjectByType<PlanetBody>();
        }

        void FixedUpdate()
        {
            ApplyAntiGravity();
        }

        void ApplyAntiGravity()
        {
            if (planet == null) return;

            var bodies = FindObjectsByType<Rigidbody>(FindObjectsSortMode.None);

            foreach (var body in bodies)
            {
                if (body.useGravity) continue;

                Vector3 direction = (planet.Center - body.position).normalized;
                float distance = Vector3.Distance(body.position, planet.Center);

                // Normal gravity
                float gravity = gravityStrength * (planet.Radius / distance);
                body.AddForce(direction * gravity, ForceMode.Acceleration);

                // Anti-gravity zones
                if (antiGravityZones != null)
                {
                    foreach (var zone in antiGravityZones)
                    {
                        if (zone == null) continue;

                        float zoneDist = Vector3.Distance(body.position, zone.position);
                        if (zoneDist < 30f) // Zone radius
                        {
                            float factor = 1f - (zoneDist / 30f);
                            body.AddForce(-direction * (antiGravityStrength * factor), ForceMode.Acceleration);
                        }
                    }
                }
            }
        }
    }
}
