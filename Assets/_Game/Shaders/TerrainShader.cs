using UnityEngine;

namespace Asteria.Shaders
{
    /// <summary>
    /// Terrain shader utilities for blending multiple biome textures.
    /// Creates materials for terrain rendering with height-based blending.
    /// </summary>
    public static class TerrainShader
    {
        /// <summary>
        /// Create a terrain material that blends between biomes based on height.
        /// </summary>
        public static Material CreateTerrainMaterial(Color lowColor, Color midColor, Color highColor)
        {
            var shader = Shader.Find("Universal Render Pipeline/Lit")
                ?? Shader.Find("Standard");

            var mat = new Material(shader);
            mat.name = "M_Terrain_Blend";

            SetColor(mat, midColor);
            if (mat.HasProperty("_Smoothness")) mat.SetFloat("_Smoothness", 0.2f);
            if (mat.HasProperty("_Metallic")) mat.SetFloat("_Metallic", 0f);

            // Store height colors in emission for shader access
            if (mat.HasProperty("_EmissionColor"))
            {
                mat.EnableKeyword("_EMISSION");
                // Pack height colors into emission (shader will decode)
                mat.SetColor("_EmissionColor", new Color(lowColor.r, midColor.r, highColor.r, 1f));
            }

            return mat;
        }

        /// <summary>
        /// Create a terrain material from a biome definition.
        /// </summary>
        public static Material CreateBiomeMaterial(Data.BiomeDefinition biome)
        {
            if (biome == null) return CreateTerrainMaterial(
                new Color(0.4f, 0.6f, 0.3f),
                new Color(0.5f, 0.5f, 0.4f),
                new Color(0.7f, 0.7f, 0.65f));

            Color baseColor = biome.ambientColor;

            // Derive low/mid/high from base
            Color low = baseColor * 0.8f;
            Color mid = baseColor;
            Color high = Color.Lerp(baseColor, Color.white, 0.3f);

            return CreateTerrainMaterial(low, mid, high);
        }

        /// <summary>
        /// Create a procedural noise texture for terrain detail.
        /// </summary>
        public static Texture2D CreateNoiseTexture(int size, float scale, int seed)
        {
            var tex = new Texture2D(size, size, TextureFormat.RGBA32, false);
            var pixels = new Color[size * size];

            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    float nx = x * scale / size + seed;
                    float ny = y * scale / size + seed;

                    float noise = Mathf.PerlinNoise(nx, ny);
                    pixels[y * size + x] = new Color(noise, noise, noise, 1f);
                }
            }

            tex.SetPixels(pixels);
            tex.Apply();
            return tex;
        }

        /// <summary>
        /// Create a heightmap texture from mesh vertices.
        /// </summary>
        public static Texture2D CreateHeightmap(Mesh mesh, int resolution)
        {
            if (mesh == null) return null;

            var tex = new Texture2D(resolution, resolution, TextureFormat.RGBA32, false);
            var pixels = new Color[resolution * resolution];

            var vertices = mesh.vertices;
            float minY = float.MaxValue;
            float maxY = float.MinValue;

            foreach (var v in vertices)
            {
                if (v.y < minY) minY = v.y;
                if (v.y > maxY) maxY = v.y;
            }

            float range = maxY - minY;
            if (range < 0.001f) range = 1f;

            // Simple UV-based height sampling
            for (int y = 0; y < resolution; y++)
            {
                for (int x = 0; x < resolution; x++)
                {
                    float u = (float)x / resolution;
                    float v = (float)y / resolution;

                    // Find closest vertex by UV
                    float closestDist = float.MaxValue;
                    float closestHeight = 0f;

                    for (int i = 0; i < vertices.Length; i += 10) // Sample every 10th vertex
                    {
                        float vertU = (i % (resolution + 1)) / (float)(resolution);
                        float vertV = (i / (resolution + 1)) / (float)(resolution);

                        float dist = (vertU - u) * (vertU - u) + (vertV - v) * (vertV - v);
                        if (dist < closestDist)
                        {
                            closestDist = dist;
                            closestHeight = vertices[i].y;
                        }
                    }

                    float normalizedHeight = (closestHeight - minY) / range;
                    pixels[y * resolution + x] = new Color(normalizedHeight, normalizedHeight, normalizedHeight, 1f);
                }
            }

            tex.SetPixels(pixels);
            tex.Apply();
            return tex;
        }

        static void SetColor(Material mat, Color color)
        {
            if (mat.HasProperty("_BaseColor")) mat.SetColor("_BaseColor", color);
            if (mat.HasProperty("_Color")) mat.SetColor("_Color", color);
            mat.color = color;
        }
    }
}
