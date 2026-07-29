using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Configuration with advanced noise parameters.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Procedural Planet Config 3")]
    public sealed class ProceduralPlanetConfig3 : ScriptableObject
    {
        [Header("Planet")]
        public string planetName = "New Planet";
        public float planetRadius = 300f;
        public int seed = 42;

        [Header("Noise")]
        public float noiseScale = 0.005f;
        public int noiseOctaves = 6;
        public float noiseLacunarity = 2f;
        public float noisePersistence = 0.5f;
        public float noiseAmplitude = 20f;

        [Header("Biomes")]
        public float biomeScale = 0.002f;
        public int biomeOctaves = 3;

        [Header("Features")]
        public bool hasWater = true;
        public float waterLevel = 0.4f;
        public bool hasAtmosphere = true;
        public float atmosphereRadius = 1.3f;
        public bool hasVegetation = true;
        public bool hasCreatures = true;

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
    }
}
