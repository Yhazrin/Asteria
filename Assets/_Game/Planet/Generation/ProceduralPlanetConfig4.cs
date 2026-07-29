using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Configuration with texture generation parameters.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Procedural Planet Config 4")]
    public sealed class ProceduralPlanetConfig4 : ScriptableObject
    {
        [Header("Planet")]
        public string planetName = "New Planet";
        public float planetRadius = 300f;
        public int seed = 42;

        [Header("Texture")]
        public int textureResolution = 256;
        public float textureScale = 8f;

        [Header("Colors")]
        public Color grassColor = new(0.4f, 0.6f, 0.3f);
        public Color rockColor = new(0.5f, 0.5f, 0.4f);
        public Color snowColor = new(0.8f, 0.8f, 0.85f);

        /// <summary>
        /// Generate texture from config.
        /// </summary>
        public Texture2D GenerateTexture()
        {
            var tex = new Texture2D(textureResolution, textureResolution, TextureFormat.RGBA32, false);
            var pixels = new Color[textureResolution * textureResolution];

            for (int y = 0; y < textureResolution; y++)
            {
                for (int x = 0; x < textureResolution; x++)
                {
                    float nx = (float)x / textureResolution * textureScale;
                    float ny = (float)y / textureResolution * textureScale;

                    float noise = Mathf.PerlinNoise(nx + seed, ny + seed);

                    Color color;
                    if (noise < 0.3f)
                        color = grassColor;
                    else if (noise < 0.6f)
                        color = rockColor;
                    else
                        color = snowColor;

                    pixels[y * textureResolution + x] = color;
                }
            }

            tex.SetPixels(pixels);
            tex.Apply();
            return tex;
        }
    }
}
