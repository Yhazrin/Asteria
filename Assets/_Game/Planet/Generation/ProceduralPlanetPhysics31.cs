using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Physics system with gravity pulses.
    /// </summary>
    public sealed class ProceduralPlanetPhysics31 : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float gravityStrength = 9.81f;
        [SerializeField] float pulseAmplitude = 2f;
        [SerializeField] float pulseFrequency = 0.05f;

        [Header("References")]
        [SerializeField] PlanetBody planet;

        void Start()
        {
            if (planet == null)
                planet = FindFirstObjectByType<PlanetBody>();
        }

        void FixedUpdate()
        {
            ApplyPulsedGravity();
        }

        void ApplyPulsedGravity()
        {
            if (planet == null) return;

            var bodies = FindObjectsByType<Rigidbody>(FindObjectsSortMode.None);
            float pulse = Mathf.Sin(Time.fixedTime * pulseFrequency) * pulseAmplitude;

            foreach (var body in bodies)
            {
                if (body.useGravity) continue;

                Vector3 direction = (planet.Center - body.position).normalized;
                float distance = Vector3.Distance(body.position, planet.Center);

                float gravity = (gravityStrength + pulse) * (planet.Radius / distance);
                body.AddForce(direction * gravity, ForceMode.Acceleration);
            }
        }
    }
}
