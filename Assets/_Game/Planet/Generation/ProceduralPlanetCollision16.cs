using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Collision system with material-based effects.
    /// </summary>
    public sealed class ProceduralPlanetCollision16 : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] PhysicMaterial iceMaterial;
        [SerializeField] PhysicMaterial mudMaterial;
        [SerializeField] PhysicMaterial rockMaterial;

        [Header("References")]
        [SerializeField] PlanetBody planet;

        void Start()
        {
            if (planet == null)
                planet = FindFirstObjectByType<PlanetBody>();

            CreateMaterials();
        }

        void CreateMaterials()
        {
            iceMaterial = new PhysicMaterial("Ice");
            iceMaterial.dynamicFriction = 0.05f;
            iceMaterial.staticFriction = 0.05f;
            iceMaterial.bounciness = 0.1f;

            mudMaterial = new PhysicMaterial("Mud");
            mudMaterial.dynamicFriction = 0.8f;
            mudMaterial.staticFriction = 0.9f;
            mudMaterial.bounciness = 0f;

            rockMaterial = new PhysicMaterial("Rock");
            rockMaterial.dynamicFriction = 0.6f;
            rockMaterial.staticFriction = 0.7f;
            rockMaterial.bounciness = 0.2f;
        }

        /// <summary>
        /// Get material for terrain type.
        /// </summary>
        public PhysicMaterial GetMaterial(string terrainType)
        {
            return terrainType switch
            {
                "ice" => iceMaterial,
                "mud" => mudMaterial,
                "rock" => rockMaterial,
                _ => rockMaterial
            };
        }
    }
}
