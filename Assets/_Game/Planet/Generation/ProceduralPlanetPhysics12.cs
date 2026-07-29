using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Physics system with gravity zones.
    /// </summary>
    public sealed class ProceduralPlanetPhysics12 : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float gravityStrength = 9.81f;
        [SerializeField] float zoneRadius = 50f;

        [Header("References")]
        [SerializeField] PlanetBody planet;
        [SerializeField] Transform[] gravityZones;

        void Start()
        {
            if (planet == null)
                planet = FindFirstObjectByType<PlanetBody>();
        }

        void FixedUpdate()
        {
            ApplyGravityZones();
        }

        void ApplyGravityZones()
        {
            if (planet == null) return;

            var bodies = FindObjectsByType<Rigidbody>(FindObjectsSortMode.None);

            foreach (var body in bodies)
            {
                if (body.useGravity) continue;

                Vector3 totalGravity = Vector3.zero;

                // Planet gravity
                Vector3 planetDir = (planet.Center - body.position).normalized;
                float planetDist = Vector3.Distance(body.position, planet.Center);
                float planetGravity = gravityStrength * (planet.Radius / planetDist);
                totalGravity += planetDir * planetGravity;

                // Zone gravity modifiers
                if (gravityZones != null)
                {
                    foreach (var zone in gravityZones)
                    {
                        if (zone == null) continue;

                        float zoneDist = Vector3.Distance(body.position, zone.position);
                        if (zoneDist < zoneRadius)
                        {
                            float factor = 1f - (zoneDist / zoneRadius);
                            Vector3 zoneDir = (zone.position - body.position).normalized;
                            totalGravity += zoneDir * (gravityStrength * factor * 0.5f);
                        }
                    }
                }

                body.AddForce(totalGravity, ForceMode.Acceleration);
            }
        }
    }
}
