using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Physics system with gravity breathing.
    /// </summary>
    public sealed class ProceduralPlanetPhysics41 : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float gravityStrength = 9.81f;
        [SerializeField] float breatheAmount = 0.5f;
        [SerializeField] float breatheSpeed = 0.2f;

        [Header("References")]
        [SerializeField] PlanetBody planet;

        void Start()
        {
            if (planet == null)
                planet = FindFirstObjectByType<PlanetBody>();
        }

        void FixedUpdate()
        {
            ApplyBreathingGravity();
        }

        void ApplyBreathingGravity()
        {
            if (planet == null) return;

            var bodies = FindObjectsByType<Rigidbody>(FindObjectsSortMode.None);
            float breathe = Mathf.Sin(Time.fixedTime * breatheSpeed) * breatheAmount;

            foreach (var body in bodies)
            {
                if (body.useGravity) continue;

                Vector3 direction = (planet.Center - body.position).normalized;
                float distance = Vector3.Distance(body.position, planet.Center);

                float gravity = (gravityStrength + breathe) * (planet.Radius / distance);
                body.AddForce(direction * gravity, ForceMode.Acceleration);
            }
        }
    }
}
