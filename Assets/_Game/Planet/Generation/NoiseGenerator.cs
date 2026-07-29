using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Simplex-like noise generator for spherical terrain.
    /// Based on Minecraft's Perlin noise approach but adapted for 3D sphere coordinates.
    /// </summary>
    public sealed class NoiseGenerator
    {
        readonly int _seed;
        readonly int[] _perm;

        public NoiseGenerator(int seed)
        {
            _seed = seed;
            _perm = new int[512];
            var p = new int[256];

            // Initialize permutation table with seed
            var rng = new System.Random(seed);
            for (int i = 0; i < 256; i++) p[i] = i;

            // Fisher-Yates shuffle
            for (int i = 255; i > 0; i--)
            {
                int j = rng.Next(i + 1);
                (p[i], p[j]) = (p[j], p[i]);
            }

            for (int i = 0; i < 512; i++) _perm[i] = p[i & 255];
        }

        /// <summary>
        /// 2D Perlin noise. Used for heightmaps on sphere surface.
        /// </summary>
        public float Noise2D(float x, float y)
        {
            int xi = Mathf.FloorToInt(x) & 255;
            int yi = Mathf.FloorToInt(y) & 255;
            float xf = x - Mathf.Floor(x);
            float yf = y - Mathf.Floor(y);

            float u = Fade(xf);
            float v = Fade(yf);

            int aa = _perm[_perm[xi] + yi];
            int ab = _perm[_perm[xi] + yi + 1];
            int ba = _perm[_perm[xi + 1] + yi];
            int bb = _perm[_perm[xi + 1] + yi + 1];

            float x1 = Mathf.Lerp(Grad(aa, xf, yf), Grad(ba, xf - 1, yf), u);
            float x2 = Mathf.Lerp(Grad(ab, xf, yf - 1), Grad(bb, xf - 1, yf - 1), u);

            return Mathf.Lerp(x1, x2, v);
        }

        /// <summary>
        /// 3D Perlin noise. Used for caves, overhangs, 3D features.
        /// </summary>
        public float Noise3D(float x, float y, float z)
        {
            int xi = Mathf.FloorToInt(x) & 255;
            int yi = Mathf.FloorToInt(y) & 255;
            int zi = Mathf.FloorToInt(z) & 255;
            float xf = x - Mathf.Floor(x);
            float yf = y - Mathf.Floor(y);
            float zf = z - Mathf.Floor(z);

            float u = Fade(xf);
            float v = Fade(yf);
            float w = Fade(zf);

            int a = _perm[xi] + yi;
            int aa = _perm[a] + zi;
            int ab = _perm[a + 1] + zi;
            int b = _perm[xi + 1] + yi;
            int ba = _perm[b] + zi;
            int bb = _perm[b + 1] + zi;

            return Mathf.Lerp(
                Mathf.Lerp(
                    Mathf.Lerp(Grad(_perm[aa], xf, yf, zf), Grad(_perm[ba], xf - 1, yf, zf), u),
                    Mathf.Lerp(Grad(_perm[ab], xf, yf - 1, zf), Grad(_perm[bb], xf - 1, yf - 1, zf), u),
                    v),
                Mathf.Lerp(
                    Mathf.Lerp(Grad(_perm[aa + 1], xf, yf, zf - 1), Grad(_perm[ba + 1], xf - 1, yf, zf - 1), u),
                    Mathf.Lerp(Grad(_perm[ab + 1], xf, yf - 1, zf - 1), Grad(_perm[bb + 1], xf - 1, yf - 1, zf - 1), u),
                    v),
                w);
        }

        /// <summary>
        /// Fractal Brownian Motion (fBm) — multiple noise octaves叠加.
        /// This is the core of Minecraft-style terrain generation.
        /// </summary>
        public float FBM2D(float x, float y, int octaves, float lacunarity, float persistence)
        {
            float total = 0f;
            float frequency = 1f;
            float amplitude = 1f;
            float maxValue = 0f;

            for (int i = 0; i < octaves; i++)
            {
                total += Noise2D(x * frequency, y * frequency) * amplitude;
                maxValue += amplitude;
                amplitude *= persistence;
                frequency *= lacunarity;
            }

            return total / maxValue;
        }

        /// <summary>
        /// 3D fBm for caves and volumetric features.
        /// </summary>
        public float FBM3D(float x, float y, float z, int octaves, float lacunarity, float persistence)
        {
            float total = 0f;
            float frequency = 1f;
            float amplitude = 1f;
            float maxValue = 0f;

            for (int i = 0; i < octaves; i++)
            {
                total += Noise3D(x * frequency, y * frequency, z * frequency) * amplitude;
                maxValue += amplitude;
                amplitude *= persistence;
                frequency *= lacunarity;
            }

            return total / maxValue;
        }

        /// <summary>
        /// Domain warping — distorts noise coordinates for more organic shapes.
        /// Minecraft uses this for biome boundaries and terrain features.
        /// </summary>
        public float WarpedNoise2D(float x, float y, float warpStrength, int octaves)
        {
            float warpX = FBM2D(x + 5.2f, y + 1.3f, octaves, 2f, 0.5f) * warpStrength;
            float warpY = FBM2D(x + 9.7f, y + 2.8f, octaves, 2f, 0.5f) * warpStrength;
            return FBM2D(x + warpX, y + warpY, octaves, 2f, 0.5f);
        }

        static float Fade(float t) => t * t * t * (t * (t * 6 - 15) + 10);

        static float Grad(int hash, float x, float y)
        {
            int h = hash & 3;
            float u = h < 2 ? x : y;
            float v = h < 2 ? y : x;
            return ((h & 1) == 0 ? u : -u) + ((h & 2) == 0 ? v : -v);
        }

        static float Grad(int hash, float x, float y, float z)
        {
            int h = hash & 15;
            float u = h < 8 ? x : y;
            float v = h < 4 ? y : h == 12 || h == 14 ? x : z;
            return ((h & 1) == 0 ? u : -u) + ((h & 2) == 0 ? v : -v);
        }
    }
}
