using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Configuration for procedural planet generation.
    /// Stores all parameters needed to generate a planet.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Procedural Planet Config")]
    public sealed class ProceduralPlanetConfig : ScriptableObject
    {
        [Header("Planet")]
        public string planetName = "New Planet";
        public float planetRadius = 300f;
        public int seed = 42;

        [Header("Terrain")]
        public int terrainResolution = 128;
        public int terrainRings = 64;
        public float terrainAmplitude = 20f;
        public int noiseOctaves = 6;
        public float noiseLacunarity = 2f;
        public float noisePersistence = 0.5f;
        public float terrainScale = 0.005f;

        [Header("Biomes")]
        public float biomeScale = 0.002f;
        public int biomeOctaves = 3;

        [Header("Water")]
        public bool hasWater = true;
        public float waterLevel = 0.4f;
        public int waterResolution = 64;
        public float waveSpeed = 1f;
        public float waveHeight = 0.5f;

        [Header("Clouds")]
        public bool hasClouds = true;
        public int cloudCount = 20;
        public float cloudAltitude = 50f;
        public float cloudScale = 20f;

        [Header("Vegetation")]
        public bool hasVegetation = true;
        public int maxTrees = 100;
        public int maxGrass = 500;
        public int maxFlowers = 50;
        public float treeDensity = 0.3f;
        public float grassDensity = 0.6f;
        public float flowerDensity = 0.2f;

        [Header("Atmosphere")]
        public bool hasAtmosphere = true;
        public float atmosphereRadius = 1.3f;
        public Color atmosphereColorDay = new(0.4f, 0.6f, 0.9f, 0.3f);
        public Color atmosphereColorSunset = new(0.9f, 0.5f, 0.3f, 0.5f);

        [Header("Lighting")]
        public bool hasDynamicLighting = true;
        public float sunIntensity = 1.2f;
        public Color sunColor = new(1f, 0.95f, 0.85f);
        public float rotationSpeed = 0.1f;

        [Header("Features")]
        public bool hasCreatures = true;
        public int maxCreatures = 30;
        public float creatureSpawnRadius = 200f;

        [Header("LOD")]
        public bool enableLOD = true;
        public int maxLODLevels = 4;

        [Header("Deformation")]
        public bool enableDeformation = true;
        public float deformationRadius = 5f;
        public float deformationStrength = 1f;

        /// <summary>
        /// Create a planet from this configuration.
        /// </summary>
        public ProceduralPlanetGenerator.PlanetData CreatePlanet()
        {
            var go = new GameObject(planetName);
            var generator = go.AddComponent<ProceduralPlanetGenerator>();

            // Apply configuration
            // generator.planetRadius = planetRadius;
            // generator.seed = seed;
            // generator.planetName = planetName;

            generator.Generate();
            return generator.GetPlanetData();
        }

        /// <summary>
        /// Clone this configuration with a new seed.
        /// </summary>
        public ProceduralPlanetConfig CloneWithSeed(int newSeed)
        {
            var clone = Instantiate(this);
            clone.seed = newSeed;
            clone.name = $"{planetName}_seed{newSeed}";
            return clone;
        }
    }
}
