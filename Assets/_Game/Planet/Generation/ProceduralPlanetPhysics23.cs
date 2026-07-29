using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Physics system with gravity falloff.
    /// </summary>
    public sealed class ProceduralPlanetPhysics23 : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float surfaceGravity = 9.81f;
        [SerializeField] float falloffExponent = 2f;

        [Header("References")]
        [SerializeField] PlanetBody planet;

        void Start()
        {
            if (planet == null)
                planet = FindFirstObjectByType<PlanetBody>();
        }

        void FixedUpdate()
        {
            ApplyGravityWithFalloff();
        }

        void ApplyGravityWithFalloff()
        {
            if (planet == null) return;

            var bodies = FindObjectsByType<Rigidbody>(FindObjectsSortMode.None);

            foreach (var body in bodies)
            {
                if (body.useGravity) continue;

                Vector3 direction = (planet.Center - body.position).normalized;
                float distance = Vector3.Distance(body.position, planet.Center);
                float altitude = distance - planet.Radius;

                // Gravity with exponential falloff
                float gravity = surfaceGravity * Mathf.Exp(-altitude / (planet.Radius * 0.5f));
                gravity = Mathf.Max(gravity, surfaceGravity * 0.01f);

                body.AddForce(direction * gravity, ForceMode.Acceleration);
            }
        }
    }
}
