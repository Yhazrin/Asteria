using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Physics system with gravity warping.
    /// </summary>
    public sealed class ProceduralPlanetPhysics39 : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float gravityStrength = 9.81f;
        [SerializeField] float warpFrequency = 0.01f;
        [SerializeField] float warpAmplitude = 1f;

        [Header("References")]
        [SerializeField] PlanetBody planet;

        void Start()
        {
            if (planet == null)
                planet = FindFirstObjectByType<PlanetBody>();
        }

        void FixedUpdate()
        {
            ApplyWarpedGravity();
        }

        void ApplyWarpedGravity()
        {
            if (planet == null) return;

            var bodies = FindObjectsByType<Rigidbody>(FindObjectsSortMode.None);

            foreach (var body in bodies)
            {
                if (body.useGravity) continue;

                Vector3 direction = (planet.Center - body.position).normalized;
                float distance = Vector3.Distance(body.position, planet.Center);

                float gravity = gravityStrength * (planet.Radius / distance);

                // Add warp
                float warp = Mathf.PerlinNoise(
                    body.position.x * warpFrequency,
                    body.position.z * warpFrequency) * warpAmplitude;

                gravity += warp;

                body.AddForce(direction * gravity, ForceMode.Acceleration);
            }
        }
    }
}
