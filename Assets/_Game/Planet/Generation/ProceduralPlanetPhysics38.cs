using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Physics system with gravity inversion.
    /// </summary>
    public sealed class ProceduralPlanetPhysics38 : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float gravityStrength = 9.81f;
        [SerializeField] bool invertGravity = false;

        [Header("References")]
        [SerializeField] PlanetBody planet;

        void Start()
        {
            if (planet == null)
                planet = FindFirstObjectByType<PlanetBody>();
        }

        void FixedUpdate()
        {
            ApplyGravity();
        }

        void ApplyGravity()
        {
            if (planet == null) return;

            var bodies = FindObjectsByType<Rigidbody>(FindObjectsSortMode.None);

            foreach (var body in bodies)
            {
                if (body.useGravity) continue;

                Vector3 direction = (planet.Center - body.position).normalized;
                float distance = Vector3.Distance(body.position, planet.Center);

                float gravity = gravityStrength * (planet.Radius / distance);

                if (invertGravity)
                {
                    direction = -direction;
                }

                body.AddForce(direction * gravity, ForceMode.Acceleration);
            }
        }
    }
}
