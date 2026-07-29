using System.Collections.Generic;
using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Collision system for procedural planets.
    /// Handles terrain collisions and object interactions.
    /// </summary>
    public sealed class ProceduralPlanetCollision : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float collisionRadius = 0.5f;
        [SerializeField] float bounceForce = 5f;
        [SerializeField] float friction = 0.8f;

        [Header("References")]
        [SerializeField] PlanetBody planet;
        [SerializeField] SphericalTerrainGenerator terrainGenerator;

        readonly Dictionary<Collider, CollisionData> _collisionData = new();

        void Start()
        {
            if (planet == null)
                planet = FindFirstObjectByType<PlanetBody>();
            if (terrainGenerator == null)
                terrainGenerator = FindFirstObjectByType<SphericalTerrainGenerator>();
        }

        void OnCollisionEnter(Collision collision)
        {
            HandleCollision(collision);
        }

        void OnCollisionStay(Collision collision)
        {
            HandleCollision(collision);
        }

        void HandleCollision(Collision collision)
        {
            if (planet == null) return;

            ContactPoint contact = collision.GetContact(0);
            Vector3 contactPoint = contact.point;
            Vector3 contactNormal = contact.normal;

            // Check if collision is with terrain
            if (IsTerrainCollision(collision))
            {
                HandleTerrainCollision(collision, contactPoint, contactNormal);
            }
        }

        bool IsTerrainCollision(Collision collision)
        {
            // Check if collision is with planet terrain
            return collision.gameObject.GetComponent<PlanetBody>() != null ||
                   collision.gameObject.GetComponent<SphericalTerrainGenerator>() != null;
        }

        void HandleTerrainCollision(Collision collision, Vector3 contactPoint, Vector3 contactNormal)
        {
            // Get surface normal from planet
            Vector3 surfaceNormal = (contactPoint - planet.Center).normalized;

            // Apply bounce force
            Rigidbody rb = collision.rigidbody;
            if (rb != null)
            {
                Vector3 velocity = rb.linearVelocity;
                float impactSpeed = Vector3.Dot(velocity, -surfaceNormal);

                if (impactSpeed > 1f)
                {
                    // Bounce
                    Vector3 bounceVelocity = surfaceNormal * impactSpeed * bounceForce;
                    rb.AddForce(bounceVelocity, ForceMode.VelocityChange);
                }

                // Apply friction
                Vector3 tangentialVelocity = velocity - Vector3.Dot(velocity, surfaceNormal) * surfaceNormal;
                rb.AddForce(-tangentialVelocity * friction, ForceMode.VelocityChange);
            }
        }

        /// <summary>
        /// Check if a position is colliding with terrain.
        /// </summary>
        public bool IsCollidingWithTerrain(Vector3 position, float radius)
        {
            if (planet == null) return false;

            float distance = Vector3.Distance(position, planet.Center);
            return distance < planet.Radius + radius;
        }

        /// <summary>
        /// Get the closest point on terrain to a position.
        /// </summary>
        public Vector3 GetClosestTerrainPoint(Vector3 position)
        {
            if (planet == null) return position;

            Vector3 direction = (position - planet.Center).normalized;
            return planet.GetPointOnSurface(direction, 0f);
        }

        /// <summary>
        /// Get the terrain height at a position.
        /// </summary>
        public float GetTerrainHeight(Vector3 position)
        {
            if (planet == null) return 0f;

            Vector3 direction = (position - planet.Center).normalized;
            Vector3 surfacePoint = planet.GetPointOnSurface(direction, 0f);
            return (position - surfacePoint).magnitude;
        }

        class CollisionData
        {
            public float lastCollisionTime;
            public int collisionCount;
        }
    }
}
