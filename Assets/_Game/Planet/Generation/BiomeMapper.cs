using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Maps noise values to biomes on the sphere surface.
    /// Based on Minecraft's climate-based biome system using temperature and humidity.
    /// </summary>
    public sealed class BiomeMapper
    {
        readonly NoiseGenerator _temperatureNoise;
        readonly NoiseGenerator _humidityNoise;
        readonly int _seed;

        public BiomeMapper(int seed)
        {
            _seed = seed;
            _temperatureNoise = new NoiseGenerator(seed + 100);
            _humidityNoise = new NoiseGenerator(seed + 200);
        }

        /// <summary>
        /// Get the biome at a sphere point using temperature/humidity mapping.
        /// Similar to Minecraft's Whittaker climate classification.
        /// </summary>
        public BiomeType GetBiome(Vector3 spherePoint, float planetRadius)
        {
            float temperature = GetTemperature(spherePoint, planetRadius);
            float humidity = GetHumidity(spherePoint, planetRadius);

            return ClassifyBiome(temperature, humidity);
        }

        /// <summary>
        /// Get temperature (0-1) at a sphere point.
        /// Based on latitude (like Minecraft) with noise variation.
        /// </summary>
        float GetTemperature(Vector3 spherePoint, float planetRadius)
        {
            // Base temperature from latitude (y coordinate)
            // Equator (y=0) is hot, poles (y=±1) are cold
            float latitudeFactor = 1f - Mathf.Abs(spherePoint.y);

            // Add noise variation
            float nx = spherePoint.x * 0.003f * planetRadius;
            float ny = spherePoint.y * 0.003f * planetRadius;
            float nz = spherePoint.z * 0.003f * planetRadius;
            float noise = _temperatureNoise.FBM3D(nx, ny, nz, 3, 2f, 0.5f) * 0.2f;

            return Mathf.Clamp01(latitudeFactor + noise);
        }

        /// <summary>
        /// Get humidity (0-1) at a sphere point.
        /// Based on noise (like Minecraft's continentalness).
        /// </summary>
        float GetHumidity(Vector3 spherePoint, float planetRadius)
        {
            float nx = spherePoint.x * 0.004f * planetRadius;
            float ny = spherePoint.y * 0.004f * planetRadius;
            float nz = spherePoint.z * 0.004f * planetRadius;

            return Mathf.Clamp01(_humidityNoise.FBM3D(nx, ny, nz, 4, 2f, 0.5f) * 0.5f + 0.5f);
        }

        /// <summary>
        /// Classify biome based on temperature and humidity.
        /// Based on Minecraft's Whittaker climate zones.
        /// </summary>
        BiomeType ClassifyBiome(float temperature, float humidity)
        {
            // Cold biomes (like Minecraft taiga/snowy)
            if (temperature < 0.2f)
            {
                return humidity > 0.5f ? BiomeType.Snowy : BiomeType.Tundra;
            }

            // Temperate biomes (like Minecraft plains/forest)
            if (temperature < 0.5f)
            {
                if (humidity < 0.3f) return BiomeType.Plains;
                if (humidity < 0.6f) return BiomeType.Forest;
                return BiomeType.Swamp;
            }

            // Warm biomes (like Minecraft savanna/desert)
            if (temperature < 0.8f)
            {
                if (humidity < 0.3f) return BiomeType.Savanna;
                if (humidity < 0.6f) return BiomeType.Woodland;
                return BiomeType.Jungle;
            }

            // Hot biomes (like Minecraft desert/badlands)
            if (humidity < 0.3f) return BiomeType.Desert;
            if (humidity < 0.6f) return BiomeType.Badlands;
            return BiomeType.Oasis;
        }

        /// <summary>
        /// Get biome color for rendering.
        /// </summary>
        public static Color GetBiomeColor(BiomeType biome, float height)
        {
            Color baseColor = biome switch
            {
                BiomeType.Snowy => new Color(0.9f, 0.9f, 0.95f),
                BiomeType.Tundra => new Color(0.7f, 0.7f, 0.65f),
                BiomeType.Plains => new Color(0.5f, 0.7f, 0.3f),
                BiomeType.Forest => new Color(0.2f, 0.5f, 0.2f),
                BiomeType.Swamp => new Color(0.3f, 0.4f, 0.2f),
                BiomeType.Savanna => new Color(0.7f, 0.6f, 0.3f),
                BiomeType.Woodland => new Color(0.4f, 0.5f, 0.2f),
                BiomeType.Jungle => new Color(0.1f, 0.4f, 0.1f),
                BiomeType.Desert => new Color(0.8f, 0.7f, 0.4f),
                BiomeType.Badlands => new Color(0.7f, 0.4f, 0.2f),
                BiomeType.Oasis => new Color(0.3f, 0.6f, 0.3f),
                _ => new Color(0.5f, 0.5f, 0.5f)
            };

            // Blend with height for terrain variation
            Color highColor = new Color(0.6f, 0.55f, 0.5f); // Rock
            return Color.Lerp(baseColor, highColor, Mathf.Clamp01(height * 2f - 0.5f));
        }

        /// <summary>
        /// Get biome-specific features (trees, rocks, etc.)
        /// </summary>
        public static SphericalTerrainGenerator.FeatureType[] GetBiomeFeatures(BiomeType biome)
        {
            return biome switch
            {
                BiomeType.Forest => new[] {
                    SphericalTerrainGenerator.FeatureType.Tree,
                    SphericalTerrainGenerator.FeatureType.Grass,
                    SphericalTerrainGenerator.FeatureType.Mushroom
                },
                BiomeType.Jungle => new[] {
                    SphericalTerrainGenerator.FeatureType.Tree,
                    SphericalTerrainGenerator.FeatureType.Tree, // Dense
                    SphericalTerrainGenerator.FeatureType.Flower
                },
                BiomeType.Plains => new[] {
                    SphericalTerrainGenerator.FeatureType.Grass,
                    SphericalTerrainGenerator.FeatureType.Flower
                },
                BiomeType.Desert => new[] {
                    SphericalTerrainGenerator.FeatureType.Rock,
                    SphericalTerrainGenerator.FeatureType.Crystal
                },
                BiomeType.Snowy => new[] {
                    SphericalTerrainGenerator.FeatureType.Rock,
                    SphericalTerrainGenerator.FeatureType.Crystal
                },
                _ => new[] { SphericalTerrainGenerator.FeatureType.Grass }
            };
        }

        public enum BiomeType
        {
            Snowy,
            Tundra,
            Plains,
            Forest,
            Swamp,
            Savanna,
            Woodland,
            Jungle,
            Desert,
            Badlands,
            Oasis
        }
    }
}
