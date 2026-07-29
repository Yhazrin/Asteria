using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Physics system with tidal forces.
    /// </summary>
    public sealed class ProceduralPlanetPhysics5 : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float gravityStrength = 9.81f;
        [SerializeField] float tidalForce = 0.1f;

        [Header("References")]
        [SerializeField] PlanetBody planet;
        [SerializeField] PlanetBody[] otherPlanets;

        void Start()
        {
            if (planet == null)
                planet = FindFirstObjectByType<PlanetBody>();

            if (otherPlanets == null || otherPlanets.Length == 0)
            {
                otherPlanets = FindObjectsByType<PlanetBody>(FindObjectsSortMode.None);
            }
        }

        void FixedUpdate()
        {
            ApplyTidalForces();
        }

        void ApplyTidalForces()
        {
            if (planet == null) return;

            var bodies = FindObjectsByType<Rigidbody>(FindObjectsSortMode.None);

            foreach (var body in bodies)
            {
                if (body.useGravity) continue;

                // Apply gravity from main planet
                Vector3 direction = (planet.Center - body.position).normalized;
                float distance = Vector3.Distance(body.position, planet.Center);
                float gravity = gravityStrength * (planet.Radius / distance);
                body.AddForce(direction * gravity, ForceMode.Acceleration);

                // Apply tidal forces from other planets
                foreach (var otherPlanet in otherPlanets)
                {
                    if (otherPlanet == null || otherPlanet == planet) continue;

                    Vector3 tidalDirection = (otherPlanet.Center - body.position).normalized;
                    float tidalDistance = Vector3.Distance(body.position, otherPlanet.Center);
                    float tidal = tidalForce * (otherPlanet.Radius * otherPlanet.Radius) / (tidalDistance * tidalDistance);
                    body.AddForce(tidalDirection * tidal, ForceMode.Acceleration);
                }
            }
        }
    }
}
