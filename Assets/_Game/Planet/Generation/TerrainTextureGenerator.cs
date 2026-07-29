using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Generates procedural textures for terrain rendering.
    /// Creates biome-specific textures with noise patterns.
    /// </summary>
    public static class TerrainTextureGenerator
    {
        /// <summary>
        /// Generate a terrain texture for a specific biome.
        /// </summary>
        public static Texture2D GenerateBiomeTexture(BiomeMapper.BiomeType biome, int size = 256)
        {
            return biome switch
            {
                BiomeMapper.BiomeType.Plains => GenerateGrassTexture(size),
                BiomeMapper.BiomeType.Forest => GenerateForestTexture(size),
                BiomeMapper.BiomeType.Desert => GenerateSandTexture(size),
                BiomeMapper.BiomeType.Snowy => GenerateSnowTexture(size),
                BiomeMapper.BiomeType.Swamp => GenerateSwampTexture(size),
                _ => GenerateGrassTexture(size)
            };
        }

        /// <summary>
        /// Generate grass texture with subtle variation.
        /// </summary>
        public static Texture2D GenerateGrassTexture(int size = 256)
        {
            var tex = new Texture2D(size, size, TextureFormat.RGBA32, false);
            var pixels = new Color[size * size];

            var baseColor = new Color(0.45f, 0.62f, 0.48f);
            var darkColor = new Color(0.35f, 0.52f, 0.38f);
            var lightColor = new Color(0.55f, 0.72f, 0.58f);

            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    float nx = (float)x / size * 8f;
                    float ny = (float)y / size * 8f;

                    float noise = Mathf.PerlinNoise(nx, ny);
                    float detail = Mathf.PerlinNoise(nx * 4f, ny * 4f) * 0.3f;

                    Color color;
                    if (noise < 0.3f)
                        color = Color.Lerp(darkColor, baseColor, noise / 0.3f);
                    else if (noise > 0.7f)
                        color = Color.Lerp(baseColor, lightColor, (noise - 0.7f) / 0.3f);
                    else
                        color = baseColor;

                    // Add detail
                    color = Color.Lerp(color, darkColor, detail * 0.2f);

                    pixels[y * size + x] = color;
                }
            }

            tex.SetPixels(pixels);
            tex.Apply();
            return tex;
        }

        /// <summary>
        /// Generate forest texture with tree-like patterns.
        /// </summary>
        public static Texture2D GenerateForestTexture(int size = 256)
        {
            var tex = new Texture2D(size, size, TextureFormat.RGBA32, false);
            var pixels = new Color[size * size];

            var baseColor = new Color(0.2f, 0.4f, 0.2f);
            var darkColor = new Color(0.15f, 0.3f, 0.15f);
            var lightColor = new Color(0.3f, 0.5f, 0.3f);

            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    float nx = (float)x / size * 6f;
                    float ny = (float)y / size * 6f;

                    float noise = Mathf.PerlinNoise(nx, ny);
                    float trees = Mathf.PerlinNoise(nx * 3f + 100f, ny * 3f + 100f);

                    Color color;
                    if (trees > 0.6f)
                        color = darkColor;
                    else if (noise < 0.3f)
                        color = Color.Lerp(darkColor, baseColor, noise / 0.3f);
                    else
                        color = baseColor;

                    pixels[y * size + x] = color;
                }
            }

            tex.SetPixels(pixels);
            tex.Apply();
            return tex;
        }

        /// <summary>
        /// Generate sand texture.
        /// </summary>
        public static Texture2D GenerateSandTexture(int size = 256)
        {
            var tex = new Texture2D(size, size, TextureFormat.RGBA32, false);
            var pixels = new Color[size * size];

            var baseColor = new Color(0.85f, 0.75f, 0.5f);
            var darkColor = new Color(0.75f, 0.65f, 0.4f);
            var lightColor = new Color(0.95f, 0.85f, 0.6f);

            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    float nx = (float)x / size * 10f;
                    float ny = (float)y / size * 10f;

                    float noise = Mathf.PerlinNoise(nx, ny);
                    float dunes = Mathf.Sin(nx * 2f + noise * 3f) * 0.5f + 0.5f;

                    Color color = Color.Lerp(darkColor, lightColor, dunes);
                    color = Color.Lerp(color, baseColor, noise * 0.3f);

                    pixels[y * size + x] = color;
                }
            }

            tex.SetPixels(pixels);
            tex.Apply();
            return tex;
        }

        /// <summary>
        /// Generate snow texture.
        /// </summary>
        public static Texture2D GenerateSnowTexture(int size = 256)
        {
            var tex = new Texture2D(size, size, TextureFormat.RGBA32, false);
            var pixels = new Color[size * size];

            var baseColor = new Color(0.92f, 0.92f, 0.95f);
            var shadowColor = new Color(0.8f, 0.82f, 0.88f);
            var sparkleColor = new Color(0.98f, 0.98f, 1f);

            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    float nx = (float)x / size * 12f;
                    float ny = (float)y / size * 12f;

                    float noise = Mathf.PerlinNoise(nx, ny);
                    float sparkle = Mathf.PerlinNoise(nx * 5f + 50f, ny * 5f + 50f);

                    Color color;
                    if (sparkle > 0.8f)
                        color = sparkleColor;
                    else if (noise < 0.3f)
                        color = shadowColor;
                    else
                        color = baseColor;

                    pixels[y * size + x] = color;
                }
            }

            tex.SetPixels(pixels);
            tex.Apply();
            return tex;
        }

        /// <summary>
        /// Generate swamp texture.
        /// </summary>
        public static Texture2D GenerateSwampTexture(int size = 256)
        {
            var tex = new Texture2D(size, size, TextureFormat.RGBA32, false);
            var pixels = new Color[size * size];

            var baseColor = new Color(0.3f, 0.4f, 0.25f);
            var waterColor = new Color(0.2f, 0.35f, 0.3f);
            var mudColor = new Color(0.35f, 0.3f, 0.2f);

            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    float nx = (float)x / size * 6f;
                    float ny = (float)y / size * 6f;

                    float noise = Mathf.PerlinNoise(nx, ny);
                    float water = Mathf.PerlinNoise(nx * 2f + 200f, ny * 2f + 200f);

                    Color color;
                    if (water > 0.6f)
                        color = waterColor;
                    else if (noise < 0.3f)
                        color = mudColor;
                    else
                        color = baseColor;

                    pixels[y * size + x] = color;
                }
            }

            tex.SetPixels(pixels);
            tex.Apply();
            return tex;
        }
    }
}
