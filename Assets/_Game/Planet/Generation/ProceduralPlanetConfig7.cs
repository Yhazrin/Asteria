using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Configuration with color parameters.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Procedural Planet Config 7")]
    public sealed class ProceduralPlanetConfig7 : ScriptableObject
    {
        [Header("Planet")]
        public string planetName = "New Planet";
        public float planetRadius = 300f;
        public int seed = 42;

        [Header("Colors")]
        public Color lowColor = new(0.4f, 0.6f, 0.3f);
        public Color midColor = new(0.5f, 0.5f, 0.4f);
        public Color highColor = new(0.8f, 0.8f, 0.85f);

        [Header("Mesh")]
        public int resolution = 128;
        public float meshScale = 1f;

        /// <summary>
        /// Generate colored mesh from config.
        /// </summary>
        public Mesh GenerateColoredMesh()
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
                    vertices[idx] = dir * planetRadius * meshScale;
                    normals[idx] = dir;
                    uvs[idx] = new Vector2((float)lon / resolution, (float)lat / resolution);

                    // Color based on latitude
                    float latitude = Mathf.Abs(y);
                    if (latitude < 0.3f)
                        colors[idx] = lowColor;
                    else if (latitude < 0.6f)
                        colors[idx] = midColor;
                    else
                        colors[idx] = highColor;
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

            var mesh = new Mesh { name = $"{planetName}_Colored" };
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
