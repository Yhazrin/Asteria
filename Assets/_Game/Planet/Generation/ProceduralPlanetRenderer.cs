using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Main renderer for the procedural planet.
    /// Combines terrain, water, clouds, vegetation, and atmosphere.
    /// </summary>
    public sealed class ProceduralPlanetRenderer : MonoBehaviour
    {
        [Header("Planet")]
        [SerializeField] float planetRadius = 300f;
        [SerializeField] int seed = 42;

        [Header("Components")]
        [SerializeField] bool generateTerrain = true;
        [SerializeField] bool generateWater = true;
        [SerializeField] bool generateClouds = true;
        [SerializeField] bool generateVegetation = true;
        [SerializeField] bool generateAtmosphere = true;
        [SerializeField] bool generateStars = true;

        [Header("References")]
        [SerializeField] PlanetBody planetBody;

        // Sub-systems
        SphericalTerrainGenerator _terrainGenerator;
        WaterSystem _waterSystem;
        ProceduralClouds _cloudSystem;
        ProceduralVegetation _vegetationSystem;
        AtmosphereRenderer _atmosphereSystem;
        ProceduralStars _starsSystem;
        ProceduralLighting _lightingSystem;
        ProceduralPostProcessing _postProcessing;

        void Start()
        {
            if (planetBody == null)
            {
                planetBody = FindFirstObjectByType<PlanetBody>();
                if (planetBody == null)
                {
                    var go = new GameObject("Planet");
                    planetBody = go.AddComponent<PlanetBody>();
                    planetBody.Configure(planetRadius, 9.81f);
                }
            }

            GeneratePlanet();
        }

        void GeneratePlanet()
        {
            Debug.Log($"[ProceduralPlanetRenderer] Generating planet with seed {seed}");

            // Terrain
            if (generateTerrain)
            {
                var terrainGo = new GameObject("Terrain");
                terrainGo.transform.SetParent(transform, false);
                _terrainGenerator = terrainGo.AddComponent<SphericalTerrainGenerator>();
                // Configure terrain generator
            }

            // Water
            if (generateWater)
            {
                var waterGo = new GameObject("Water");
                waterGo.transform.SetParent(transform, false);
                _waterSystem = waterGo.AddComponent<WaterSystem>();
            }

            // Clouds
            if (generateClouds)
            {
                var cloudsGo = new GameObject("Clouds");
                cloudsGo.transform.SetParent(transform, false);
                _cloudSystem = cloudsGo.AddComponent<ProceduralClouds>();
            }

            // Vegetation
            if (generateVegetation)
            {
                var vegetationGo = new GameObject("Vegetation");
                vegetationGo.transform.SetParent(transform, false);
                _vegetationSystem = vegetationGo.AddComponent<ProceduralVegetation>();
            }

            // Atmosphere
            if (generateAtmosphere)
            {
                var atmosphereGo = new GameObject("Atmosphere");
                atmosphereGo.transform.SetParent(transform, false);
                _atmosphereSystem = atmosphereGo.AddComponent<AtmosphereRenderer>();
            }

            // Stars
            if (generateStars)
            {
                var starsGo = new GameObject("Stars");
                starsGo.transform.SetParent(transform, false);
                _starsSystem = starsGo.AddComponent<ProceduralStars>();
            }

            // Lighting
            var lightingGo = new GameObject("Lighting");
            lightingGo.transform.SetParent(transform, false);
            _lightingSystem = lightingGo.AddComponent<ProceduralLighting>();

            // Post-processing
            var postProcessingGo = new GameObject("PostProcessing");
            postProcessingGo.transform.SetParent(transform, false);
            _postProcessing = postProcessingGo.AddComponent<ProceduralPostProcessing>();

            Debug.Log("[ProceduralPlanetRenderer] Planet generation complete.");
        }

        /// <summary>
        /// Regenerate the planet with a new seed.
        /// </summary>
        public void Regenerate(int newSeed)
        {
            seed = newSeed;

            // Destroy old children
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }

            // Regenerate
            GeneratePlanet();
        }

        /// <summary>
        /// Get the terrain generator.
        /// </summary>
        public SphericalTerrainGenerator GetTerrainGenerator()
        {
            return _terrainGenerator;
        }

        /// <summary>
        /// Get the water system.
        /// </summary>
        public WaterSystem GetWaterSystem()
        {
            return _waterSystem;
        }

        /// <summary>
        /// Get the cloud system.
        /// </summary>
        public ProceduralClouds GetCloudSystem()
        {
            return _cloudSystem;
        }

        /// <summary>
        /// Get the vegetation system.
        /// </summary>
        public ProceduralVegetation GetVegetationSystem()
        {
            return _vegetationSystem;
        }

        /// <summary>
        /// Get the atmosphere renderer.
        /// </summary>
        public AtmosphereRenderer GetAtmosphereRenderer()
        {
            return _atmosphereSystem;
        }

        /// <summary>
        /// Get the stars system.
        /// </summary>
        public ProceduralStars GetStarsSystem()
        {
            return _starsSystem;
        }

        /// <summary>
        /// Get the lighting system.
        /// </summary>
        public ProceduralLighting GetLightingSystem()
        {
            return _lightingSystem;
        }

        /// <summary>
        /// Get the post-processing system.
        /// </summary>
        public ProceduralPostProcessing GetPostProcessing()
        {
            return _postProcessing;
        }
    }
}
