using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Terrain configuration file database for the game.
    /// Contains all terrain parameters.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Terrain Config File Database")]
    public sealed class TerrainConfigFileDatabase : ScriptableObject
    {
        [Header("Noise")]
        public float noiseScale = 0.005f;
        public int noiseOctaves = 6;
        public float noiseLacunarity = 2f;
        public float noisePersistence = 0.5f;

        [Header("Height")]
        public float terrainAmplitude = 20f;
        public float minHeight = -10f;
        public float maxHeight = 30f;

        [Header("Resolution")]
        public int meshResolution = 128;
        public int meshRings = 64;

        [Header("Biomes")]
        public float biomeScale = 0.002f;
        public int biomeOctaves = 3;
    }
}
