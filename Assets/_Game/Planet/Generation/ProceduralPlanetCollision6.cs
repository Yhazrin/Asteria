using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Collision system with predictive collision detection.
    /// </summary>
    public sealed class ProceduralPlanetCollision6 : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float predictionTime = 0.1f;

        [Header("References")]
        [SerializeField] PlanetBody planet;
        [SerializeField] Rigidbody targetBody;

        void Start()
        {
            if (planet == null)
                planet = FindFirstObjectByType<PlanetBody>();
            if (targetBody == null)
                targetBody = FindFirstObjectByType<Rigidbody>();
        }

        /// <summary>
        /// Predict collision in the future.
        /// </summary>
        public bool PredictCollision(float timeStep, out Vector3 collisionPoint)
        {
            collisionPoint = Vector3.zero;
            if (planet == null || targetBody == null) return false;

            Vector3 futurePosition = targetBody.position + targetBody.linearVelocity * timeStep;
            float distance = Vector3.Distance(futurePosition, planet.Center);

            if (distance < planet.Radius)
            {
                collisionPoint = (futurePosition - planet.Center).normalized * planet.Radius;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Get time until collision.
        /// </summary>
        public float GetTimeUntilCollision()
        {
            if (planet == null || targetBody == null) return float.MaxValue;

            Vector3 direction = (planet.Center - targetBody.position).normalized;
            float closingSpeed = Vector3.Dot(targetBody.linearVelocity, direction);

            if (closingSpeed <= 0) return float.MaxValue;

            float distance = Vector3.Distance(targetBody.position, planet.Center) - planet.Radius;
            return distance / closingSpeed;
        }
    }
}
