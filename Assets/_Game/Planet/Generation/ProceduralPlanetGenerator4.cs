using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Planet generator with procedural textures.
    /// </summary>
    public sealed class ProceduralPlanetGenerator4 : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float planetRadius = 300f;
        [SerializeField] int seed = 42;
        [SerializeField] int textureResolution = 256;

        [Header("References")]
        [SerializeField] PlanetBody planet;

        void Start()
        {
            if (planet == null)
                planet = FindFirstObjectByType<PlanetBody>();
        }

        /// <summary>
        /// Generate procedural texture for planet.
        /// </summary>
        public Texture2D GenerateTexture()
        {
            var tex = new Texture2D(textureResolution, textureResolution, TextureFormat.RGBA32, false);
            var pixels = new Color[textureResolution * textureResolution];

            for (int y = 0; y < textureResolution; y++)
            {
                for (int x = 0; x < textureResolution; x++)
                {
                    float nx = (float)x / textureResolution * 8f;
                    float ny = (float)y / textureResolution * 8f;

                    float noise = Mathf.PerlinNoise(nx + seed, ny + seed);

                    Color color;
                    if (noise < 0.3f)
                        color = new Color(0.4f, 0.6f, 0.3f); // Grass
                    else if (noise < 0.6f)
                        color = new Color(0.5f, 0.5f, 0.4f); // Rock
                    else
                        color = new Color(0.8f, 0.8f, 0.85f); // Snow

                    pixels[y * textureResolution + x] = color;
                }
            }

            tex.SetPixels(pixels);
            tex.Apply();
            return tex;
        }
    }
}
