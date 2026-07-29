using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Physics system with gravity smoothing.
    /// </summary>
    public sealed class ProceduralPlanetPhysics27 : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float gravityStrength = 9.81f;
        [SerializeField] float smoothFactor = 0.1f;

        [Header("References")]
        [SerializeField] PlanetBody planet;

        void Start()
        {
            if (planet == null)
                planet = FindFirstObjectByType<PlanetBody>();
        }

        void FixedUpdate()
        {
            ApplySmoothedGravity();
        }

        void ApplySmoothedGravity()
        {
            if (planet == null) return;

            var bodies = FindObjectsByType<Rigidbody>(FindObjectsSortMode.None);

            foreach (var body in bodies)
            {
                if (body.useGravity) continue;

                Vector3 direction = (planet.Center - body.position).normalized;
                float distance = Vector3.Distance(body.position, planet.Center);

                float gravity = gravityStrength * (planet.Radius / distance);

                // Smooth gravity application
                Vector3 gravityForce = direction * gravity;
                Vector3 currentForce = body.linearVelocity.normalized * body.linearVelocity.magnitude;
                Vector3 smoothed = Vector3.Lerp(currentForce, gravityForce, smoothFactor);

                body.AddForce(smoothed, ForceMode.Acceleration);
            }
        }
    }
}
