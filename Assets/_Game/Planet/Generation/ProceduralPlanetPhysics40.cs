using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Physics system with gravity oscillation.
    /// </summary>
    public sealed class ProceduralPlanetPhysics40 : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float gravityStrength = 9.81f;
        [SerializeField] float oscillationFrequency = 0.1f;
        [SerializeField] float oscillationAmplitude = 1f;

        [Header("References")]
        [SerializeField] PlanetBody planet;

        void Start()
        {
            if (planet == null)
                planet = FindFirstObjectByType<PlanetBody>();
        }

        void FixedUpdate()
        {
            ApplyOscillatingGravity();
        }

        void ApplyOscillatingGravity()
        {
            if (planet == null) return;

            var bodies = FindObjectsByType<Rigidbody>(FindObjectsSortMode.None);
            float oscillation = Mathf.Sin(Time.fixedTime * oscillationFrequency) * oscillationAmplitude;

            foreach (var body in bodies)
            {
                if (body.useGravity) continue;

                Vector3 direction = (planet.Center - body.position).normalized;
                float distance = Vector3.Distance(body.position, planet.Center);

                float gravity = (gravityStrength + oscillation) * (planet.Radius / distance);
                body.AddForce(direction * gravity, ForceMode.Acceleration);
            }
        }
    }
}
