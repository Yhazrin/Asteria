using UnityEngine;

namespace Asteria.Art
{
    /// <summary>
    /// Advanced procedural asset generator.
    /// Creates high-quality meshes, materials, and textures entirely in code.
    /// No external assets needed.
    /// </summary>
    public static class AdvancedProceduralAssets
    {
        // === Texture Generation ===

        /// <summary>Generate a gradient texture</summary>
        public static Texture2D MakeGradientTexture(int width, int height, Color from, Color to, bool vertical = true)
        {
            var tex = new Texture2D(width, height, TextureFormat.RGBA32, false);
            var pixels = new Color[width * height];

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    float t = vertical ? (float)y / height : (float)x / width;
                    pixels[y * width + x] = Color.Lerp(from, to, t);
                }
            }

            tex.SetPixels(pixels);
            tex.Apply();
            return tex;
        }

        /// <summary>Generate a noise texture for terrain detail</summary>
        public static Texture2D MakeNoiseTexture(int size, float scale, int octaves, int seed)
        {
            var tex = new Texture2D(size, size, TextureFormat.RGBA32, false);
            var pixels = new Color[size * size];
            var rng = new System.Random(seed);

            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    float nx = x * scale / size;
                    float ny = y * scale / size;

                    float noise = 0f;
                    float amplitude = 1f;
                    float frequency = 1f;

                    for (int o = 0; o < octaves; o++)
                    {
                        noise += Mathf.PerlinNoise(nx * frequency + seed, ny * frequency + seed) * amplitude;
                        amplitude *= 0.5f;
                        frequency *= 2f;
                    }

                    noise = Mathf.Clamp01(noise);
                    pixels[y * size + x] = new Color(noise, noise, noise, 1f);
                }
            }

            tex.SetPixels(pixels);
            tex.Apply();
            return tex;
        }

        /// <summary>Generate a checker pattern texture</summary>
        public static Texture2D MakeCheckerTexture(int size, Color a, Color b, int cellSize = 8)
        {
            var tex = new Texture2D(size, size, TextureFormat.RGBA32, false);
            var pixels = new Color[size * size];

            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    bool isA = ((x / cellSize) + (y / cellSize)) % 2 == 0;
                    pixels[y * size + x] = isA ? a : b;
                }
            }

            tex.SetPixels(pixels);
            tex.Apply();
            return tex;
        }

        // === Advanced Meshes ===

        /// <summary>Generate a low-poly terrain chunk mesh</summary>
        public static Mesh MakeTerrainChunk(int resolution, float size, float heightScale, int seed)
        {
            int vertCount = (resolution + 1) * (resolution + 1);
            var vertices = new Vector3[vertCount];
            var normals = new Vector3[vertCount];
            var uvs = new Vector2[vertCount];
            var colors = new Color[vertCount];

            float halfSize = size * 0.5f;
            float step = size / resolution;

            for (int z = 0; z <= resolution; z++)
            {
                for (int x = 0; x <= resolution; x++)
                {
                    int idx = z * (resolution + 1) + x;
                    float px = -halfSize + x * step;
                    float pz = -halfSize + z * step;

                    // Height from noise
                    float height = SampleNoise(px, pz, 0.02f, 4, seed) * heightScale;

                    vertices[idx] = new Vector3(px, height, pz);
                    uvs[idx] = new Vector2((float)x / resolution, (float)z / resolution);

                    // Color based on height
                    float normalizedHeight = height / heightScale;
                    colors[idx] = GetTerrainColor(normalizedHeight);
                }
            }

            // Calculate normals
            for (int z = 0; z <= resolution; z++)
            {
                for (int x = 0; x <= resolution; x++)
                {
                    int idx = z * (resolution + 1) + x;
                    Vector3 normal = Vector3.up;

                    if (x > 0 && x < resolution && z > 0 && z < resolution)
                    {
                        Vector3 left = vertices[z * (resolution + 1) + (x - 1)];
                        Vector3 right = vertices[z * (resolution + 1) + (x + 1)];
                        Vector3 back = vertices[(z - 1) * (resolution + 1) + x];
                        Vector3 forward = vertices[(z + 1) * (resolution + 1) + x];

                        normal = Vector3.Cross(right - left, forward - back).normalized;
                    }

                    normals[idx] = normal;
                }
            }

            // Triangles
            int triCount = resolution * resolution * 6;
            var triangles = new int[triCount];
            int t = 0;

            for (int z = 0; z < resolution; z++)
            {
                for (int x = 0; x < resolution; x++)
                {
                    int current = z * (resolution + 1) + x;
                    int next = current + resolution + 1;

                    triangles[t++] = current;
                    triangles[t++] = next;
                    triangles[t++] = current + 1;

                    triangles[t++] = current + 1;
                    triangles[t++] = next;
                    triangles[t++] = next + 1;
                }
            }

            var mesh = new Mesh { name = "TerrainChunk" };
            mesh.vertices = vertices;
            mesh.normals = normals;
            mesh.uv = uvs;
            mesh.colors = colors;
            mesh.triangles = triangles;
            mesh.RecalculateBounds();
            return mesh;
        }

        /// <summary>Generate a stylized cloud mesh</summary>
        public static Mesh MakeCloudMesh(float size = 5f)
        {
            var combined = new System.Collections.Generic.List<Vector3>();
            var combinedTris = new System.Collections.Generic.List<int>();

            // Multiple overlapping spheres for cloud shape
            var positions = new Vector3[]
            {
                Vector3.zero,
                new Vector3(-0.4f, 0.1f, 0) * size,
                new Vector3(0.4f, 0.1f, 0) * size,
                new Vector3(0, 0.2f, 0.3f) * size,
                new Vector3(0, 0.1f, -0.3f) * size,
            };

            var radii = new float[] { 0.5f, 0.4f, 0.4f, 0.35f, 0.35f };

            int vertexOffset = 0;
            for (int i = 0; i < positions.Length; i++)
            {
                var sphere = MakeIcosphere(1, radii[i] * size);
                var verts = sphere.vertices;
                var tris = sphere.triangles;

                for (int v = 0; v < verts.Length; v++)
                {
                    combined.Add(verts[v] + positions[i]);
                }

                for (int t = 0; t < tris.Length; t++)
                {
                    combinedTris.Add(tris[t] + vertexOffset);
                }

                vertexOffset += verts.Length;
            }

            var mesh = new Mesh { name = "Cloud" };
            mesh.SetVertices(combined);
            mesh.SetTriangles(combinedTris, 0);
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            return mesh;
        }

        /// <summary>Generate a flower mesh</summary>
        public static Mesh MakeFlowerMesh(Color petalColor, Color centerColor)
        {
            var combined = new System.Collections.Generic.List<Vector3>();
            var combinedTris = new System.Collections.Generic.List<int>();
            var combinedColors = new System.Collections.Generic.List<Color>();

            // Stem
            var stem = MakeCylinder(0.02f, 0.5f, 4);
            AddMesh(combined, combinedTris, combinedColors, stem, Vector3.zero, Color.green);

            // Petals (5 small spheres around center)
            for (int i = 0; i < 5; i++)
            {
                float angle = i * 72f * Mathf.Deg2Rad;
                Vector3 pos = new Vector3(Mathf.Cos(angle) * 0.15f, 0.5f, Mathf.Sin(angle) * 0.15f);
                var petal = MakeIcosphere(1, 0.08f);
                AddMesh(combined, combinedTris, combinedColors, petal, pos, petalColor);
            }

            // Center
            var center = MakeIcosphere(1, 0.06f);
            AddMesh(combined, combinedTris, combinedColors, center, new Vector3(0, 0.5f, 0), centerColor);

            var mesh = new Mesh { name = "Flower" };
            mesh.SetVertices(combined);
            mesh.SetTriangles(combinedTris, 0);
            mesh.SetColors(combinedColors);
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            return mesh;
        }

        // === Helper Methods ===

        static float SampleNoise(float x, float z, float scale, int octaves, int seed)
        {
            float noise = 0f;
            float amplitude = 1f;
            float frequency = 1f;

            for (int i = 0; i < octaves; i++)
            {
                noise += Mathf.PerlinNoise(
                    x * scale * frequency + seed,
                    z * scale * frequency + seed
                ) * amplitude;
                amplitude *= 0.5f;
                frequency *= 2f;
            }

            return noise;
        }

        static Color GetTerrainColor(float height)
        {
            if (height < 0.2f) return new Color(0.4f, 0.6f, 0.3f);  // Grass
            if (height < 0.4f) return new Color(0.5f, 0.55f, 0.4f); // Light grass
            if (height < 0.6f) return new Color(0.6f, 0.55f, 0.45f); // Dirt
            if (height < 0.8f) return new Color(0.5f, 0.48f, 0.42f); // Rock
            return new Color(0.85f, 0.85f, 0.9f);                    // Snow
        }

        static void AddMesh(System.Collections.Generic.List<Vector3> verts,
            System.Collections.Generic.List<int> tris,
            System.Collections.Generic.List<Color> colors,
            Mesh mesh, Vector3 offset, Color color)
        {
            int offset_idx = verts.Count;
            foreach (var v in mesh.vertices) verts.Add(v + offset);
            foreach (var t in mesh.triangles) tris.Add(t + offset_idx);
            for (int i = 0; i < mesh.vertexCount; i++) colors.Add(color);
        }

        public static Mesh MakeIcosphere(int subdivisions, float radius)
        {
            float t = (1f + Mathf.Sqrt(5f)) / 2f;
            var verts = new System.Collections.Generic.List<Vector3>
            {
                new(-1, t, 0).normalized * radius,
                new(1, t, 0).normalized * radius,
                new(-1, -t, 0).normalized * radius,
                new(1, -t, 0).normalized * radius,
                new(0, -1, t).normalized * radius,
                new(0, 1, t).normalized * radius,
                new(0, -1, -t).normalized * radius,
                new(0, 1, -t).normalized * radius,
                new(t, 0, -1).normalized * radius,
                new(t, 0, 1).normalized * radius,
                new(-t, 0, -1).normalized * radius,
                new(-t, 0, 1).normalized * radius,
            };

            var tris = new System.Collections.Generic.List<int>
            {
                0,11,5, 0,5,1, 0,1,7, 0,7,10, 0,10,11,
                1,5,9, 5,11,4, 11,10,2, 10,7,6, 7,1,8,
                3,9,4, 3,4,2, 3,2,6, 3,6,8, 3,8,9,
                4,9,5, 2,4,11, 6,2,10, 8,6,7, 9,8,1,
            };

            for (int s = 0; s < subdivisions; s++)
            {
                var newTris = new System.Collections.Generic.List<int>();
                var midCache = new System.Collections.Generic.Dictionary<long, int>();

                for (int i = 0; i < tris.Count; i += 3)
                {
                    int a = tris[i], b = tris[i + 1], c = tris[i + 2];
                    int ab = GetMidpoint(a, b, verts, midCache, radius);
                    int bc = GetMidpoint(b, c, verts, midCache, radius);
                    int ca = GetMidpoint(c, a, verts, midCache, radius);
                    newTris.AddRange(new[] { a, ab, ca, b, bc, ab, c, ca, bc, ab, bc, ca });
                }
                tris = newTris;
            }

            var mesh = new Mesh { name = "Icosphere" };
            mesh.SetVertices(verts);
            mesh.SetTriangles(tris, 0);
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            return mesh;
        }

        public static Mesh MakeCylinder(float radius, float height, int segments)
        {
            var verts = new Vector3[segments * 2 + 2];
            var tris = new int[segments * 6];

            verts[0] = Vector3.up * height * 0.5f;
            verts[1] = Vector3.down * height * 0.5f;

            for (int i = 0; i < segments; i++)
            {
                float angle = (float)i / segments * Mathf.PI * 2f;
                float x = Mathf.Cos(angle) * radius;
                float z = Mathf.Sin(angle) * radius;
                verts[2 + i] = new Vector3(x, height * 0.5f, z);
                verts[2 + segments + i] = new Vector3(x, -height * 0.5f, z);
            }

            int t = 0;
            for (int i = 0; i < segments; i++)
            {
                int next = (i + 1) % segments;
                tris[t++] = 0; tris[t++] = 2 + i; tris[t++] = 2 + next;
                tris[t++] = 1; tris[t++] = 2 + segments + next; tris[t++] = 2 + segments + i;
                tris[t++] = 2 + i; tris[t++] = 2 + segments + i; tris[t++] = 2 + next;
                tris[t++] = 2 + next; tris[t++] = 2 + segments + i; tris[t++] = 2 + segments + next;
            }

            var mesh = new Mesh { name = "Cylinder" };
            mesh.vertices = verts;
            mesh.triangles = tris;
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            return mesh;
        }

        static int GetMidpoint(int a, int b, System.Collections.Generic.List<Vector3> verts,
            System.Collections.Generic.Dictionary<long, int> cache, float radius)
        {
            long key = ((long)Mathf.Min(a, b) << 32) | (long)Mathf.Max(a, b);
            if (cache.TryGetValue(key, out int idx)) return idx;
            Vector3 mid = ((verts[a] + verts[b]) * 0.5f).normalized * radius;
            idx = verts.Count;
            verts.Add(mid);
            cache[key] = idx;
            return idx;
        }
    }
}
