using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Physics system with magnetic forces.
    /// </summary>
    public sealed class ProceduralPlanetPhysics14 : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float gravityStrength = 9.81f;
        [SerializeField] float magneticStrength = 2f;
        [SerializeField] float magneticRange = 20f;

        [Header("References")]
        [SerializeField] PlanetBody planet;
        [SerializeField] Transform[] magneticPoints;

        void Start()
        {
            if (planet == null)
                planet = FindFirstObjectByType<PlanetBody>();
        }

        void FixedUpdate()
        {
            ApplyMagneticForces();
        }

        void ApplyMagneticForces()
        {
            if (planet == null) return;

            var bodies = FindObjectsByType<Rigidbody>(FindObjectsSortMode.None);

            foreach (var body in bodies)
            {
                if (body.useGravity) continue;

                // Planet gravity
                Vector3 direction = (planet.Center - body.position).normalized;
                float distance = Vector3.Distance(body.position, planet.Center);
                float gravity = gravityStrength * (planet.Radius / distance);
                body.AddForce(direction * gravity, ForceMode.Acceleration);

                // Magnetic forces
                if (magneticPoints != null)
                {
                    foreach (var point in magneticPoints)
                    {
                        if (point == null) continue;

                        float magDist = Vector3.Distance(body.position, point.position);
                        if (magDist < magneticRange && magDist > 0.1f)
                        {
                            Vector3 magDir = (point.position - body.position).normalized;
                            float magForce = magneticStrength * (1f - magDist / magneticRange);
                            body.AddForce(magDir * magForce, ForceMode.Acceleration);
                        }
                    }
                }
            }
        }
    }
}
