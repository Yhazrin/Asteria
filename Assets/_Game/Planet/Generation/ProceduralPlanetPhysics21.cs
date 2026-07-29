using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Physics system with gravity prediction.
    /// </summary>
    public sealed class ProceduralPlanetPhysics21 : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float gravityStrength = 9.81f;
        [SerializeField] float predictionTime = 1f;

        [Header("References")]
        [SerializeField] PlanetBody planet;

        void Start()
        {
            if (planet == null)
                planet = FindFirstObjectByType<PlanetBody>();
        }

        /// <summary>
        /// Predict future position under gravity.
        /// </summary>
        public Vector3 PredictPosition(Vector3 position, Vector3 velocity, float timeStep)
        {
            if (planet == null) return position;

            Vector3 direction = (planet.Center - position).normalized;
            float distance = Vector3.Distance(position, planet.Center);
            float gravity = gravityStrength * (planet.Radius / distance);

            Vector3 acceleration = direction * gravity;
            return position + velocity * timeStep + 0.5f * acceleration * timeStep * timeStep;
        }
    }
}
