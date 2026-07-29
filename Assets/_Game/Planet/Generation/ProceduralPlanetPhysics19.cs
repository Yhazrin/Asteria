using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Physics system with gravity zones.
    /// </summary>
    public sealed class ProceduralPlanetPhysics19 : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float gravityStrength = 9.81f;

        [Header("References")]
        [SerializeField] PlanetBody planet;
        [SerializeField] GravityZone[] gravityZones;

        void Start()
        {
            if (planet == null)
                planet = FindFirstObjectByType<PlanetBody>();
        }

        void FixedUpdate()
        {
            ApplyGravityWithZones();
        }

        void ApplyGravityWithZones()
        {
            if (planet == null) return;

            var bodies = FindObjectsByType<Rigidbody>(FindObjectsSortMode.None);

            foreach (var body in bodies)
            {
                if (body.useGravity) continue;

                Vector3 totalGravity = Vector3.zero;

                // Planet gravity
                Vector3 planetDir = (planet.Center - body.position).normalized;
                float planetDist = Vector3.Distance(body.position, planet.Center);
                float planetGravity = gravityStrength * (planet.Radius / planetDist);
                totalGravity += planetDir * planetGravity;

                // Zone modifications
                if (gravityZones != null)
                {
                    foreach (var zone in gravityZones)
                    {
                        if (zone == null) continue;

                        float zoneDist = Vector3.Distance(body.position, zone.center);
                        if (zoneDist < zone.radius)
                        {
                            float factor = 1f - (zoneDist / zone.radius);
                            totalGravity += zone.direction * (zone.strength * factor);
                        }
                    }
                }

                body.AddForce(totalGravity, ForceMode.Acceleration);
            }
        }

        [System.Serializable]
        public class GravityZone
        {
            public Vector3 center;
            public float radius = 20f;
            public Vector3 direction;
            public float strength = 5f;
        }
    }
}
