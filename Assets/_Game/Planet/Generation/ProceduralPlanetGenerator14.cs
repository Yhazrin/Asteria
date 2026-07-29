using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Planet generator with mesh validation.
    /// </summary>
    public sealed class ProceduralPlanetGenerator14 : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float planetRadius = 300f;
        [SerializeField] int seed = 42;
        [SerializeField] int resolution = 128;

        [Header("References")]
        [SerializeField] PlanetBody planet;

        void Start()
        {
            if (planet == null)
                planet = FindFirstObjectByType<PlanetBody>();
        }

        /// <summary>
        /// Generate and validate mesh.
        /// </summary>
        public Mesh GenerateValidatedMesh()
        {
            var mesh = GenerateMesh();

            // Validate
            if (!ValidateMesh(mesh))
            {
                Debug.LogError("[ProceduralPlanetGenerator14] Mesh validation failed!");
                return null;
            }

            return mesh;
        }

        bool ValidateMesh(Mesh mesh)
        {
            if (mesh == null) return false;
            if (mesh.vertexCount == 0) return false;
            if (mesh.triangles.Length == 0) return false;

            // Check for degenerate triangles
            var triangles = mesh.triangles;
            for (int i = 0; i < triangles.Length; i += 3)
            {
                if (triangles[i] == triangles[i + 1] || triangles[i + 1] == triangles[i + 2] || triangles[i] == triangles[i + 2])
                {
                    return false;
                }
            }

            return true;
        }

        Mesh GenerateMesh()
        {
            int vertCount = (resolution + 1) * (resolution + 1);
            var vertices = new Vector3[vertCount];
            var normals = new Vector3[vertCount];
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
                    vertices[idx] = dir * planetRadius;
                    normals[idx] = dir;
                    uvs[idx] = new Vector2((float)lon / resolution, (float)lat / resolution);
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

            var mesh = new Mesh { name = "PlanetValidated" };
            mesh.vertices = vertices;
            mesh.normals = normals;
            mesh.uv = uvs;
            mesh.triangles = triangles;
            mesh.RecalculateBounds();

            return mesh;
        }
    }
}
