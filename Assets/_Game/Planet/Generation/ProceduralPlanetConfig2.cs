using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Alternative planet configuration with different parameters.
    /// Supports more detailed customization.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Procedural Planet Config 2")]
    public sealed class ProceduralPlanetConfig2 : ScriptableObject
    {
        [Header("Planet")]
        public string planetName = "New Planet";
        public float planetRadius = 300f;
        public int seed = 42;

        [Header("Terrain")]
        public int terrainResolution = 128;
        public float terrainAmplitude = 20f;
        public int noiseOctaves = 6;
        public float noiseLacunarity = 2f;
        public float noisePersistence = 0.5f;

        [Header("Biomes")]
        public int biomeCount = 5;
        public float biomeScale = 0.002f;

        [Header("Water")]
        public bool hasWater = true;
        public float waterLevel = 0.4f;

        [Header("Atmosphere")]
        public bool hasAtmosphere = true;
        public float atmosphereRadius = 1.3f;

        [Header("Vegetation")]
        public bool hasVegetation = true;
        public int maxTrees = 100;
        public int maxGrass = 500;

        [Header("Creatures")]
        public bool hasCreatures = true;
        public int maxCreatures = 30;

        /// <summary>
        /// Create a planet from this configuration.
        /// </summary>
        public ProceduralPlanetGenerator.PlanetData CreatePlanet()
        {
            var go = new GameObject(planetName);
            var generator = go.AddComponent<ProceduralPlanetGenerator>();

            generator.Generate();
            return generator.GetPlanetData();
        }

        /// <summary>
        /// Clone this configuration with a new seed.
        /// </summary>
        public ProceduralPlanetConfig2 CloneWithSeed(int newSeed)
        {
            var clone = Instantiate(this);
            clone.seed = newSeed;
            clone.name = $"{planetName}_seed{newSeed}";
            return clone;
        }
    }
}
