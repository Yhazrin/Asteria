using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Physics system with gravity blending.
    /// </summary>
    public sealed class ProceduralPlanetPhysics22 : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float gravityStrength = 9.81f;
        [SerializeField] float blendSpeed = 2f;

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
            ApplyBlendedGravity();
        }

        void ApplyBlendedGravity()
        {
            if (planet == null) return;

            var bodies = FindObjectsByType<Rigidbody>(FindObjectsSortMode.None);

            foreach (var body in bodies)
            {
                if (body.useGravity) continue;

                Vector3 blendedDirection = Vector3.zero;
                float totalWeight = 0f;

                // Main planet
                Vector3 mainDir = (planet.Center - body.position).normalized;
                float mainDist = Vector3.Distance(body.position, planet.Center);
                float mainWeight = 1f / (mainDist * mainDist);

                blendedDirection += mainDir * mainWeight;
                totalWeight += mainWeight;

                // Other planets
                if (otherPlanets != null)
                {
                    foreach (var other in otherPlanets)
                    {
                        if (other == null || other == planet) continue;

                        Vector3 otherDir = (other.Center - body.position).normalized;
                        float otherDist = Vector3.Distance(body.position, other.Center);
                        float otherWeight = 1f / (otherDist * otherDist);

                        blendedDirection += otherDir * otherWeight;
                        totalWeight += otherWeight;
                    }
                }

                if (totalWeight > 0)
                {
                    blendedDirection /= totalWeight;
                    float gravity = gravityStrength * (planet.Radius / Vector3.Distance(body.position, planet.Center));
                    body.AddForce(blendedDirection.normalized * gravity, ForceMode.Acceleration);
                }
            }
        }
    }
}
