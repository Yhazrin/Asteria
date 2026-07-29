using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Physics system with gravity attraction.
    /// </summary>
    public sealed class ProceduralPlanetPhysics32 : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float gravityStrength = 9.81f;
        [SerializeField] float attractionRadius = 50f;

        [Header("References")]
        [SerializeField] PlanetBody planet;
        [SerializeField] Transform[] attractors;

        void Start()
        {
            if (planet == null)
                planet = FindFirstObjectByType<PlanetBody>();
        }

        void FixedUpdate()
        {
            ApplyAttraction();
        }

        void ApplyAttraction()
        {
            if (planet == null || attractors == null) return;

            var bodies = FindObjectsByType<Rigidbody>(FindObjectsSortMode.None);

            foreach (var body in bodies)
            {
                if (body.useGravity) continue;

                // Planet gravity
                Vector3 direction = (planet.Center - body.position).normalized;
                float distance = Vector3.Distance(body.position, planet.Center);
                float gravity = gravityStrength * (planet.Radius / distance);
                body.AddForce(direction * gravity, ForceMode.Acceleration);

                // Attractor forces
                foreach (var attractor in attractors)
                {
                    if (attractor == null) continue;

                    float attractorDist = Vector3.Distance(body.position, attractor.position);
                    if (attractorDist < attractionRadius && attractorDist > 0.1f)
                    {
                        Vector3 attractorDir = (attractor.position - body.position).normalized;
                        float attractorForce = gravityStrength * 0.5f * (1f - attractorDist / attractionRadius);
                        body.AddForce(attractorDir * attractorForce, ForceMode.Acceleration);
                    }
                }
            }
        }
    }
}
