using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Collision system with physics material.
    /// </summary>
    public sealed class ProceduralPlanetCollision10 : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float bounciness = 0.3f;
        [SerializeField] float friction = 0.6f;

        [Header("References")]
        [SerializeField] PlanetBody planet;
        [SerializeField] PhysicMaterial physicsMaterial;

        void Start()
        {
            if (planet == null)
                planet = FindFirstObjectByType<PlanetBody>();

            CreatePhysicsMaterial();
        }

        void CreatePhysicsMaterial()
        {
            physicsMaterial = new PhysicMaterial("PlanetSurface");
            physicsMaterial.bounciness = bounciness;
            physicsMaterial.dynamicFriction = friction;
            physicsMaterial.staticFriction = friction;

            // Apply to planet collider
            var collider = planet?.GetComponent<Collider>();
            if (collider != null)
            {
                collider.material = physicsMaterial;
            }
        }

        /// <summary>
        /// Set bounciness.
        /// </summary>
        public void SetBounciness(float value)
        {
            bounciness = Mathf.Clamp01(value);
            if (physicsMaterial != null)
            {
                physicsMaterial.bounciness = bounciness;
            }
        }

        /// <summary>
        /// Set friction.
        /// </summary>
        public void SetFriction(float value)
        {
            friction = Mathf.Clamp01(value);
            if (physicsMaterial != null)
            {
                physicsMaterial.dynamicFriction = friction;
                physicsMaterial.staticFriction = friction;
            }
        }
    }
}
