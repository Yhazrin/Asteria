using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Alternative physics system with different approach.
    /// Uses custom gravity calculations.
    /// </summary>
    public sealed class ProceduralPlanetPhysics2 : MonoBehaviour
    {
        [Header("Gravity")]
        [SerializeField] float gravityStrength = 9.81f;
        [SerializeField] float gravityFalloff = 2f;

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
                gravity = Mathf.Min(gravity, gravityStrength * 2f);

                body.AddForce(direction * gravity, ForceMode.Acceleration);
            }
        }

        public Vector3 GetGravityDirection(Vector3 position)
        {
            if (planet == null) return Vector3.down;
            return (planet.Center - position).normalized;
        }

        public float GetGravityStrength(Vector3 position)
        {
            if (planet == null) return gravityStrength;

            float distance = Vector3.Distance(position, planet.Center);
            return gravityStrength * (planet.Radius / distance);
        }

        public bool IsOnSurface(Vector3 position, float threshold = 1f)
        {
            if (planet == null) return false;

            float distance = Vector3.Distance(position, planet.Center);
            return Mathf.Abs(distance - planet.Radius) < threshold;
        }
    }
}
