using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Alternative planet renderer with different rendering approach.
    /// Supports custom shaders and advanced effects.
    /// </summary>
    public sealed class ProceduralPlanetRenderer2 : MonoBehaviour
    {
        [Header("Rendering")]
        [SerializeField] Material planetMaterial;
        [SerializeField] Material atmosphereMaterial;
        [SerializeField] Material waterMaterial;

        [Header("Settings")]
        [SerializeField] bool enableAtmosphere = true;
        [SerializeField] bool enableWater = true;
        [SerializeField] bool enableClouds = true;

        [Header("References")]
        [SerializeField] PlanetBody planet;

        void Start()
        {
            if (planet == null)
                planet = FindFirstObjectByType<PlanetBody>();
        }

        /// <summary>
        /// Set the planet material.
        /// </summary>
        public void SetPlanetMaterial(Material material)
        {
            planetMaterial = material;
            var renderer = planet?.GetComponent<MeshRenderer>();
            if (renderer != null)
            {
                renderer.material = material;
            }
        }

        /// <summary>
        /// Set the atmosphere material.
        /// </summary>
        public void SetAtmosphereMaterial(Material material)
        {
            atmosphereMaterial = material;
        }

        /// <summary>
        /// Set the water material.
        /// </summary>
        public void SetWaterMaterial(Material material)
        {
            waterMaterial = material;
        }

        /// <summary>
        /// Enable or disable atmosphere rendering.
        /// </summary>
        public void SetAtmosphereEnabled(bool enabled)
        {
            enableAtmosphere = enabled;
        }

        /// <summary>
        /// Enable or disable water rendering.
        /// </summary>
        public void SetWaterEnabled(bool enabled)
        {
            enableWater = enabled;
        }

        /// <summary>
        /// Enable or disable cloud rendering.
        /// </summary>
        public void SetCloudsEnabled(bool enabled)
        {
            enableClouds = enabled;
        }
    }
}
