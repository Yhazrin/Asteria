using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Physics system with gravity waves.
    /// </summary>
    public sealed class ProceduralPlanetPhysics18 : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float gravityStrength = 9.81f;
        [SerializeField] float waveAmplitude = 0.5f;
        [SerializeField] float waveFrequency = 0.1f;

        [Header("References")]
        [SerializeField] PlanetBody planet;

        void Start()
        {
            if (planet == null)
                planet = FindFirstObjectByType<PlanetBody>();
        }

        void FixedUpdate()
        {
            ApplyGravityWaves();
        }

        void ApplyGravityWaves()
        {
            if (planet == null) return;

            var bodies = FindObjectsByType<Rigidbody>(FindObjectsSortMode.None);
            float time = Time.fixedTime;

            foreach (var body in bodies)
            {
                if (body.useGravity) continue;

                Vector3 direction = (planet.Center - body.position).normalized;
                float distance = Vector3.Distance(body.position, planet.Center);

                // Base gravity
                float gravity = gravityStrength * (planet.Radius / distance);

                // Add gravity wave
                float wave = Mathf.Sin(time * waveFrequency + distance * 0.01f) * waveAmplitude;
                gravity += wave;

                body.AddForce(direction * gravity, ForceMode.Acceleration);
            }
        }
    }
}
