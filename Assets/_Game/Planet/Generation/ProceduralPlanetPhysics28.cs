using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Physics system with gravity variation.
    /// </summary>
    public sealed class ProceduralPlanetPhysics28 : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float baseGravity = 9.81f;
        [SerializeField] float variationAmplitude = 0.5f;
        [SerializeField] float variationFrequency = 0.1f;

        [Header("References")]
        [SerializeField] PlanetBody planet;

        void Start()
        {
            if (planet == null)
                planet = FindFirstObjectByType<PlanetBody>();
        }

        void FixedUpdate()
        {
            ApplyVariedGravity();
        }

        void ApplyVariedGravity()
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
                float gravity = baseGravity * (planet.Radius / distance);

                // Add variation
                float variation = Mathf.Sin(time * variationFrequency + distance * 0.01f) * variationAmplitude;
                gravity += variation;

                body.AddForce(direction * gravity, ForceMode.Acceleration);
            }
        }
    }
}
