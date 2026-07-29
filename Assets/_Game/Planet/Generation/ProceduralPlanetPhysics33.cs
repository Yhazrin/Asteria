using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Physics system with gravity falloff curves.
    /// </summary>
    public sealed class ProceduralPlanetPhysics33 : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float gravityStrength = 9.81f;
        [SerializeField] AnimationCurve falloffCurve = AnimationCurve.Linear(0, 1, 1, 0.1f);

        [Header("References")]
        [SerializeField] PlanetBody planet;

        void Start()
        {
            if (planet == null)
                planet = FindFirstObjectByType<PlanetBody>();
        }

        void FixedUpdate()
        {
            ApplyCurvedGravity();
        }

        void ApplyCurvedGravity()
        {
            if (planet == null) return;

            var bodies = FindObjectsByType<Rigidbody>(FindObjectsSortMode.None);

            foreach (var body in bodies)
            {
                if (body.useGravity) continue;

                Vector3 direction = (planet.Center - body.position).normalized;
                float distance = Vector3.Distance(body.position, planet.Center);
                float altitude = distance - planet.Radius;

                // Use animation curve for gravity falloff
                float normalizedAlt = Mathf.Clamp01(altitude / planet.Radius);
                float falloff = falloffCurve.Evaluate(normalizedAlt);
                float gravity = gravityStrength * falloff;

                body.AddForce(direction * gravity, ForceMode.Acceleration);
            }
        }
    }
}
