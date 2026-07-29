using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Physics system with gravity interpolation.
    /// </summary>
    public sealed class ProceduralPlanetPhysics25 : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float gravityStrength = 9.81f;

        [Header("References")]
        [SerializeField] PlanetBody planet;

        void Start()
        {
            if (planet == null)
                planet = FindFirstObjectByType<PlanetBody>();
        }

        void FixedUpdate()
        {
            ApplyInterpolatedGravity();
        }

        void ApplyInterpolatedGravity()
        {
            if (planet == null) return;

            var bodies = FindObjectsByType<Rigidbody>(FindObjectsSortMode.None);

            foreach (var body in bodies)
            {
                if (body.useGravity) continue;

                Vector3 direction = (planet.Center - body.position).normalized;
                float distance = Vector3.Distance(body.position, planet.Center);

                // Interpolated gravity based on altitude
                float altitude = distance - planet.Radius;
                float gravityFactor = Mathf.Lerp(1f, 0.1f, Mathf.Clamp01(altitude / planet.Radius));
                float gravity = gravityStrength * gravityFactor;

                body.AddForce(direction * gravity, ForceMode.Acceleration);
            }
        }
    }
}
