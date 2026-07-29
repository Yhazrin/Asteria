using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Physics system with gravity scaling.
    /// </summary>
    public sealed class ProceduralPlanetPhysics20 : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float gravityStrength = 9.81f;
        [SerializeField] float gravityScale = 1f;

        [Header("References")]
        [SerializeField] PlanetBody planet;

        void Start()
        {
            if (planet == null)
                planet = FindFirstObjectByType<PlanetBody>();
        }

        void FixedUpdate()
        {
            ApplyScaledGravity();
        }

        void ApplyScaledGravity()
        {
            if (planet == null) return;

            var bodies = FindObjectsByType<Rigidbody>(FindObjectsSortMode.None);

            foreach (var body in bodies)
            {
                if (body.useGravity) continue;

                Vector3 direction = (planet.Center - body.position).normalized;
                float distance = Vector3.Distance(body.position, planet.Center);

                float gravity = gravityStrength * (planet.Radius / distance) * gravityScale;
                body.AddForce(direction * gravity, ForceMode.Acceleration);
            }
        }

        /// <summary>
        /// Set gravity scale.
        /// </summary>
        public void SetGravityScale(float scale)
        {
            gravityScale = Mathf.Clamp(scale, 0f, 2f);
        }
    }
}
