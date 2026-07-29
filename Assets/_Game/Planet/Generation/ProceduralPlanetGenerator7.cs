using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Planet generator with procedural colors.
    /// </summary>
    public sealed class ProceduralPlanetGenerator7 : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float planetRadius = 300f;
        [SerializeField] int seed = 42;
        [SerializeField] int resolution = 128;

        [Header("Colors")]
        [SerializeField] Color lowColor = new(0.4f, 0.6f, 0.3f);
        [SerializeField] Color midColor = new(0.5f, 0.5f, 0.4f);
        [SerializeField] Color highColor = new(0.8f, 0.8f, 0.85f);

        [Header("References")]
        [SerializeField] PlanetBody planet;

        void Start()
        {
            if (planet == null)
                planet = FindFirstObjectByType<PlanetBody>();
        }

        /// <summary>
        /// Generate mesh with vertex colors.
        /// </summary>
        public Mesh GenerateMeshWithColors()
        {
            int vertCount = (resolution + 1) * (resolution + 1);
            var vertices = new Vector3[vertCount];
            var normals = new Vector3[vertCount];
            var colors = new Color[vertCount];
            var uvs = new Vector2[vertCount];

            for (int lat = 0; lat <= resolution; lat++)
            {
                float phi = (float)lat / resolution * Mathf.PI;
                for (int lon = 0; lon <= resolution; lon++)
                {
                    float theta = (float)lon / resolution * Mathf.PI * 2f;

                    int idx = lat * (resolution + 1) + lon;

                    float x = Mathf.Sin(phi) * Mathf.Cos(theta);
                    float y = Mathf.Cos(phi);
                    float z = Mathf.Sin(phi) * Mathf.Sin(theta);

                    Vector3 dir = new Vector3(x, y, z).normalized;

                    // Sample noise for height
                    float nx = dir.x * 0.005f * planetRadius;
                    float ny = dir.y * 0.005f * planetRadius;
                    float nz = dir.z * 0.005f * planetRadius;
                    float noise = Mathf.PerlinNoise(nx + seed, ny + seed);

                    float radius = planetRadius + noise * 20f;
                    vertices[idx] = dir * radius;
                    normals[idx] = dir;
                    uvs[idx] = new Vector2((float)lon / resolution, (float)lat / resolution);

                    // Color based on height
                    float height = noise;
                    if (height < 0.3f)
                        colors[idx] = lowColor;
                    else if (height < 0.6f)
                        colors[idx] = Color.Lerp(lowColor, midColor, (height - 0.3f) / 0.3f);
                    else
                        colors[idx] = Color.Lerp(midColor, highColor, (height - 0.6f) / 0.4f);
                }
            }

            int triCount = resolution * resolution * 6;
            var triangles = new int[triCount];
            int t = 0;

            for (int lat = 0; lat < resolution; lat++)
            {
                for (int lon = 0; lon < resolution; lon++)
                {
                    int current = lat * (resolution + 1) + lon;
                    int next = current + resolution + 1;

                    triangles[t++] = current;
                    triangles[t++] = next;
                    triangles[t++] = current + 1;

                    triangles[t++] = current + 1;
                    triangles[t++] = next;
                    triangles[t++] = next + 1;
                }
            }

            var mesh = new Mesh { name = "PlanetColored" };
            mesh.vertices = vertices;
            mesh.normals = normals;
            mesh.colors = colors;
            mesh.uv = uvs;
            mesh.triangles = triangles;
            mesh.RecalculateBounds();

            return mesh;
        }
    }
}
