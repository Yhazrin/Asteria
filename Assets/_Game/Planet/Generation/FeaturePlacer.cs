using System.Collections.Generic;
using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Places features (trees, rocks, crystals, etc.) on the spherical terrain.
    /// Based on Minecraft's feature placement using noise thresholds and biome rules.
    /// </summary>
    public sealed class FeaturePlacer : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float planetRadius = 300f;
        [SerializeField] int seed = 42;
        [SerializeField] float featureDensity = 0.02f;
        [SerializeField] int maxFeaturesPerChunk = 20;

        [Header("Feature Prefabs")]
        [SerializeField] GameObject treePrefab;
        [SerializeField] GameObject rockPrefab;
        [SerializeField] GameObject crystalPrefab;
        [SerializeField] GameObject grassPrefab;
        [SerializeField] GameObject flowerPrefab;

        [Header("Generation")]
        [SerializeField] float minFeatureDistance = 5f;
        [SerializeField] float featureScaleVariation = 0.3f;

        NoiseGenerator _featureNoise;
        NoiseGenerator _placementNoise;
        BiomeMapper _biomeMapper;
        readonly List<FeatureInstance> _placedFeatures = new();

        void Awake()
        {
            _featureNoise = new NoiseGenerator(seed + 300);
            _placementNoise = new NoiseGenerator(seed + 400);
            _biomeMapper = new BiomeMapper(seed);
        }

        /// <summary>
        /// Generate features for a chunk area.
        /// </summary>
        public List<FeatureInstance> GenerateFeatures(Vector3 chunkCenter, float chunkRadius, int maxCount)
        {
            var features = new List<FeatureInstance>();
            var rng = new System.Random(seed + chunkCenter.GetHashCode());

            for (int i = 0; i < maxCount; i++)
            {
                // Random point in chunk
                Vector3 randomDir = RandomOnUnitSphere(rng);
                Vector3 spherePoint = (chunkCenter.normalized + randomDir * chunkRadius).normalized;

                // Check noise threshold (like Minecraft's feature placement)
                float noise = SampleFeatureNoise(spherePoint);
                if (noise < featureDensity) continue;

                // Get biome at this point
                var biome = _biomeMapper.GetBiome(spherePoint, planetRadius);

                // Get valid features for this biome
                var validFeatures = BiomeMapper.GetBiomeFeatures(biome);
                if (validFeatures.Length == 0) continue;

                // Select feature type
                var featureType = validFeatures[rng.Next(validFeatures.Length)];

                // Check minimum distance from other features
                Vector3 worldPos = spherePoint * planetRadius;
                if (!IsTooCloseToExisting(worldPos, minFeatureDistance))
                {
                    // Calculate height at this point
                    float height = SampleTerrainHeight(spherePoint);
                    worldPos = spherePoint * (planetRadius + height * 10f);

                    // Create feature instance
                    var feature = new FeatureInstance
                    {
                        Type = featureType,
                        Position = worldPos,
                        Rotation = Quaternion.FromToRotation(Vector3.up, spherePoint),
                        Scale = Vector3.one * (1f + (float)rng.NextDouble() * featureScaleVariation),
                        Biome = biome
                    };

                    features.Add(feature);
                    _placedFeatures.Add(feature);
                }
            }

            return features;
        }

        /// <summary>
        /// Place features in the scene.
        /// </summary>
        public void PlaceFeatures(List<FeatureInstance> features)
        {
            foreach (var feature in features)
            {
                GameObject prefab = GetPrefabForType(feature.Type);
                if (prefab == null) continue;

                var instance = Instantiate(prefab, feature.Position, feature.Rotation);
                instance.transform.localScale = feature.Scale;
                instance.transform.SetParent(transform);

                // Add random Y rotation
                instance.transform.Rotate(Vector3.up, Random.Range(0f, 360f), Space.Self);
            }
        }

        float SampleFeatureNoise(Vector3 spherePoint)
        {
            float nx = spherePoint.x * 0.01f * planetRadius;
            float ny = spherePoint.y * 0.01f * planetRadius;
            float nz = spherePoint.z * 0.01f * planetRadius;

            return _featureNoise.FBM3D(nx, ny, nz, 3, 2f, 0.5f) * 0.5f + 0.5f;
        }

        float SampleTerrainHeight(Vector3 spherePoint)
        {
            float nx = spherePoint.x * 0.005f * planetRadius;
            float ny = spherePoint.y * 0.005f * planetRadius;
            float nz = spherePoint.z * 0.005f * planetRadius;

            return _placementNoise.FBM3D(nx, ny, nz, 6, 2f, 0.5f) * 0.5f + 0.5f;
        }

        bool IsTooCloseToExisting(Vector3 position, float minDistance)
        {
            foreach (var existing in _placedFeatures)
            {
                if (Vector3.Distance(existing.Position, position) < minDistance)
                {
                    return true;
                }
            }
            return false;
        }

        GameObject GetPrefabForType(SphericalTerrainGenerator.FeatureType type)
        {
            return type switch
            {
                SphericalTerrainGenerator.FeatureType.Tree => treePrefab,
                SphericalTerrainGenerator.FeatureType.Rock => rockPrefab,
                SphericalTerrainGenerator.FeatureType.Crystal => crystalPrefab,
                SphericalTerrainGenerator.FeatureType.Grass => grassPrefab,
                SphericalTerrainGenerator.FeatureType.Flower => flowerPrefab,
                _ => null
            };
        }

        static Vector3 RandomOnUnitSphere(System.Random rng)
        {
            float theta = (float)(rng.NextDouble() * Mathf.PI * 2f);
            float phi = Mathf.Acos(2f * (float)rng.NextDouble() - 1f);

            return new Vector3(
                Mathf.Sin(phi) * Mathf.Cos(theta),
                Mathf.Cos(phi),
                Mathf.Sin(phi) * Mathf.Sin(theta)
            );
        }

        public struct FeatureInstance
        {
            public SphericalTerrainGenerator.FeatureType Type;
            public Vector3 Position;
            public Quaternion Rotation;
            public Vector3 Scale;
            public BiomeMapper.BiomeType Biome;
        }
    }
}
