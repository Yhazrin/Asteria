using System.Collections.Generic;
using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Procedural terrain generator for spherical planets.
    /// Based on Minecraft's noise-based approach but adapted for sphere geometry.
    ///
    /// Key concepts from Minecraft:
    /// 1. Multi-octave noise (fBm) for natural terrain
    /// 2. Biome assignment based on noise values
    /// 3. Feature placement using noise thresholds
    /// 4. Chunk-based generation for performance
    /// </summary>
    public sealed class SphericalTerrainGenerator : MonoBehaviour
    {
        [Header("Planet")]
        [SerializeField] float planetRadius = 300f;
        [SerializeField] int seed = 42;

        [Header("Terrain Shape")]
        [SerializeField] int noiseOctaves = 6;
        [SerializeField] float noiseLacunarity = 2f;
        [SerializeField] float noisePersistence = 0.5f;
        [SerializeField] float terrainScale = 0.005f;
        [SerializeField] float terrainAmplitude = 30f;

        [Header("Biomes")]
        [SerializeField] float biomeScale = 0.002f;
        [SerializeField] int biomeOctaves = 3;

        [Header("Features")]
        [SerializeField] float featureDensity = 0.02f;
        [SerializeField] float featureNoiseScale = 0.01f;

        [Header("Mesh")]
        [SerializeField] int resolution = 128; // vertices per ring
        [SerializeField] int rings = 64;       // latitude rings

        NoiseGenerator _terrainNoise;
        NoiseGenerator _biomeNoise;
        NoiseGenerator _featureNoise;
        Mesh _generatedMesh;

        void Awake()
        {
            _terrainNoise = new NoiseGenerator(seed);
            _biomeNoise = new NoiseGenerator(seed + 1);
            _featureNoise = new NoiseGenerator(seed + 2);
        }

        /// <summary>
        /// Generate the full planet mesh with terrain variation.
        /// </summary>
        public Mesh GeneratePlanetMesh()
        {
            if (_generatedMesh != null) return _generatedMesh;

            int vertexCount = (resolution + 1) * (rings + 1);
            var vertices = new Vector3[vertexCount];
            var normals = new Vector3[vertexCount];
            var uvs = new Vector2[vertexCount];
            var colors = new Color[vertexCount];

            int index = 0;
            for (int ring = 0; ring <= rings; ring++)
            {
                float phi = (float)ring / rings * Mathf.PI; // 0 to PI (north to south)
                float v = (float)ring / rings;

                for (int seg = 0; seg <= resolution; seg++)
                {
                    float theta = (float)seg / resolution * Mathf.PI * 2f; // 0 to 2PI
                    float u = (float)seg / resolution;

                    // Spherical to Cartesian
                    float x = Mathf.Sin(phi) * Mathf.Cos(theta);
                    float y = Mathf.Cos(phi);
                    float z = Mathf.Sin(phi) * Mathf.Sin(theta);

                    Vector3 spherePoint = new Vector3(x, y, z);

                    // Sample terrain height using fBm noise
                    float height = SampleTerrainHeight(spherePoint);

                    // Sample biome for coloring
                    float biomeValue = SampleBiome(spherePoint);

                    // Apply height to radius
                    float finalRadius = planetRadius + height * terrainAmplitude;
                    vertices[index] = spherePoint * finalRadius;
                    normals[index] = spherePoint; // Normal points outward
                    uvs[index] = new Vector2(u, v);

                    // Color based on biome and height
                    colors[index] = GetTerrainColor(biomeValue, height, spherePoint.y);

                    index++;
                }
            }

            // Generate triangles
            int quadCount = resolution * rings;
            var triangles = new int[quadCount * 6];
            int t = 0;

            for (int ring = 0; ring < rings; ring++)
            {
                for (int seg = 0; seg < resolution; seg++)
                {
                    int current = ring * (resolution + 1) + seg;
                    int next = current + resolution + 1;

                    triangles[t++] = current;
                    triangles[t++] = next;
                    triangles[t++] = current + 1;

                    triangles[t++] = current + 1;
                    triangles[t++] = next;
                    triangles[t++] = next + 1;
                }
            }

            _generatedMesh = new Mesh
            {
                name = $"SphericalTerrain_{resolution}x{rings}",
                indexFormat = vertexCount > 65000
                    ? UnityEngine.Rendering.IndexFormat.UInt32
                    : UnityEngine.Rendering.IndexFormat.UInt16
            };

            _generatedMesh.vertices = vertices;
            _generatedMesh.normals = normals;
            _generatedMesh.uv = uvs;
            _generatedMesh.colors = colors;
            _generatedMesh.triangles = triangles;
            _generatedMesh.RecalculateBounds();

            return _generatedMesh;
        }

        /// <summary>
        /// Sample terrain height at a point on the sphere.
        /// Uses multi-octave fBm like Minecraft.
        /// </summary>
        float SampleTerrainHeight(Vector3 spherePoint)
        {
            // Convert sphere point to noise coordinates
            float nx = spherePoint.x * terrainScale * planetRadius;
            float ny = spherePoint.y * terrainScale * planetRadius;
            float nz = spherePoint.z * terrainScale * planetRadius;

            // Use 3D fBm for seamless sphere noise
            float height = _terrainNoise.FBM3D(nx, ny, nz, noiseOctaves, noiseLacunarity, noisePersistence);

            // Add domain warping for more organic shapes
            float warp = _terrainNoise.WarpedNoise2D(nx * 0.5f, nz * 0.5f, 10f, 3f);
            height += warp * 0.3f;

            return Mathf.Clamp01(height * 0.5f + 0.5f); // Normalize to 0-1
        }

        /// <summary>
        /// Sample biome value at a point.
        /// Similar to Minecraft's temperature/humidity biome map.
        /// </summary>
        float SampleBiome(Vector3 spherePoint)
        {
            float nx = spherePoint.x * biomeScale * planetRadius;
            float ny = spherePoint.y * biomeScale * planetRadius;
            float nz = spherePoint.z * biomeScale * planetRadius;

            return _biomeNoise.FBM3D(nx, ny, nz, biomeOctaves, 2f, 0.5f) * 0.5f + 0.5f;
        }

        /// <summary>
        /// Get terrain color based on biome, height, and latitude.
        /// </summary>
        Color GetTerrainColor(float biome, float height, float latitude)
        {
            // Base colors by biome
            Color lowColor, highColor;

            if (biome < 0.3f)
            {
                // Grassland (like Minecraft plains)
                lowColor = new Color(0.4f, 0.6f, 0.3f);  // Green
                highColor = new Color(0.5f, 0.5f, 0.4f);  // Gray rock
            }
            else if (biome < 0.5f)
            {
                // Forest (like Minecraft forest)
                lowColor = new Color(0.2f, 0.5f, 0.2f);  // Dark green
                highColor = new Color(0.4f, 0.4f, 0.3f);  // Brown rock
            }
            else if (biome < 0.7f)
            {
                // Desert (like Minecraft desert)
                lowColor = new Color(0.8f, 0.7f, 0.4f);  // Sand
                highColor = new Color(0.7f, 0.6f, 0.3f);  // Sandstone
            }
            else
            {
                // Snow (like Minecraft snowy biome)
                lowColor = new Color(0.8f, 0.8f, 0.8f);  // Snow
                highColor = new Color(0.6f, 0.6f, 0.7f);  // Ice
            }

            // Blend based on height
            Color terrainColor = Color.Lerp(lowColor, highColor, height);

            // Add latitude variation (poles are colder)
            float polarFactor = Mathf.Abs(latitude);
            if (polarFactor > 0.7f)
            {
                terrainColor = Color.Lerp(terrainColor, new Color(0.9f, 0.9f, 0.95f), (polarFactor - 0.7f) / 0.3f);
            }

            return terrainColor;
        }

        /// <summary>
        /// Check if a feature should be placed at this location.
        /// Similar to Minecraft's feature placement using noise thresholds.
        /// </summary>
        public bool ShouldPlaceFeature(Vector3 spherePoint, float threshold)
        {
            float nx = spherePoint.x * featureNoiseScale * planetRadius;
            float ny = spherePoint.y * featureNoiseScale * planetRadius;
            float nz = spherePoint.z * featureNoiseScale * planetRadius;

            float noise = _featureNoise.FBM3D(nx, ny, nz, 3, 2f, 0.5f);
            return noise > threshold;
        }

        /// <summary>
        /// Get feature type based on biome and noise.
        /// </summary>
        public FeatureType GetFeatureType(Vector3 spherePoint)
        {
            float biome = SampleBiome(spherePoint);
            float height = SampleTerrainHeight(spherePoint);

            if (biome < 0.3f && height < 0.4f)
            {
                return FeatureType.Grass;
            }
            else if (biome < 0.5f && height < 0.5f)
            {
                return FeatureType.Tree;
            }
            else if (biome > 0.7f && height > 0.6f)
            {
                return FeatureType.Rock;
            }
            else if (height > 0.8f)
            {
                return FeatureType.Crystal;
            }

            return FeatureType.None;
        }

        public enum FeatureType
        {
            None,
            Grass,
            Tree,
            Rock,
            Crystal,
            Flower,
            Mushroom
        }
    }
}
