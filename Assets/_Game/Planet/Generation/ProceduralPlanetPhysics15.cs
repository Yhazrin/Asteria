using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Physics system with portal mechanics.
    /// </summary>
    public sealed class ProceduralPlanetPhysics15 : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float gravityStrength = 9.81f;

        [Header("References")]
        [SerializeField] PlanetBody planet;
        [SerializeField] Transform[] portals;

        void Start()
        {
            if (planet == null)
                planet = FindFirstObjectByType<PlanetBody>();
        }

        void FixedUpdate()
        {
            ApplyPortalMechanics();
        }

        void ApplyPortalMechanics()
        {
            if (planet == null || portals == null || portals.Length < 2) return;

            var bodies = FindObjectsByType<Rigidbody>(FindObjectsSortMode.None);

            foreach (var body in bodies)
            {
                if (body.useGravity) continue;

                // Apply gravity
                Vector3 direction = (planet.Center - body.position).normalized;
                float distance = Vector3.Distance(body.position, planet.Center);
                float gravity = gravityStrength * (planet.Radius / distance);
                body.AddForce(direction * gravity, ForceMode.Acceleration);

                // Check portal proximity
                for (int i = 0; i < portals.Length - 1; i += 2)
                {
                    Transform entry = portals[i];
                    Transform exit = portals[i + 1];

                    if (entry == null || exit == null) continue;

                    float distToEntry = Vector3.Distance(body.position, entry.position);
                    if (distToEntry < 3f)
                    {
                        // Teleport to exit
                        body.position = exit.position + exit.forward * 3f;
                        body.linearVelocity = exit.forward * body.linearVelocity.magnitude;
                    }
                }
            }
        }
    }
}
