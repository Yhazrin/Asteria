using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Physics system with wind forces.
    /// </summary>
    public sealed class ProceduralPlanetPhysics9 : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float gravityStrength = 9.81f;
        [SerializeField] float windStrength = 5f;
        [SerializeField] float windDirection = 0f;

        [Header("References")]
        [SerializeField] PlanetBody planet;

        void Start()
        {
            if (planet == null)
                planet = FindFirstObjectByType<PlanetBody>();
        }

        void FixedUpdate()
        {
            ApplyWind();
        }

        void ApplyWind()
        {
            if (planet == null) return;

            var bodies = FindObjectsByType<Rigidbody>(FindObjectsSortMode.None);

            // Wind direction changes slowly
            windDirection += Time.fixedDeltaTime * 10f;
            Vector3 windDir = new Vector3(
                Mathf.Cos(windDirection * Mathf.Deg2Rad),
                0,
                Mathf.Sin(windDirection * Mathf.Deg2Rad));

            foreach (var body in bodies)
            {
                if (body.useGravity) continue;

                // Apply gravity
                Vector3 direction = (planet.Center - body.position).normalized;
                float distance = Vector3.Distance(body.position, planet.Center);
                float gravity = gravityStrength * (planet.Radius / distance);
                body.AddForce(direction * gravity, ForceMode.Acceleration);

                // Apply wind
                body.AddForce(windDir * windStrength, ForceMode.Acceleration);
            }
        }
    }
}
