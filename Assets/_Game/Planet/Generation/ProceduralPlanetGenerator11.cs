using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Planet generator with multiple mesh variants.
    /// </summary>
    public sealed class ProceduralPlanetGenerator11 : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float planetRadius = 300f;
        [SerializeField] int seed = 42;
        [SerializeField] int resolution = 128;

        [Header("References")]
        [SerializeField] PlanetBody planet;

        Mesh[] _meshVariants;

        void Start()
        {
            if (planet == null)
                planet = FindFirstObjectByType<PlanetBody>();

            GenerateMeshVariants();
        }

        void GenerateMeshVariants()
        {
            _meshVariants = new Mesh[4];

            for (int i = 0; i < 4; i++)
            {
                _meshVariants[i] = GenerateMesh(resolution / (i + 1));
            }
        }

        Mesh GenerateMesh(int res)
        {
            int vertCount = (res + 1) * (res + 1);
            var vertices = new Vector3[vertCount];
            var normals = new Vector3[vertCount];
            var uvs = new Vector2[vertCount];

            for (int lat = 0; lat <= res; lat++)
            {
                float phi = (float)lat / res * Mathf.PI;
                for (int lon = 0; lon <= res; lon++)
                {
                    float theta = (float)lon / res * Mathf.PI * 2f;

                    int idx = lat * (res + 1) + lon;

                    float x = Mathf.Sin(phi) * Mathf.Cos(theta);
                    float y = Mathf.Cos(phi);
                    float z = Mathf.Sin(phi) * Mathf.Sin(theta);

                    Vector3 dir = new Vector3(x, y, z).normalized;
                    vertices[idx] = dir * planetRadius;
                    normals[idx] = dir;
                    uvs[idx] = new Vector2((float)lon / res, (float)lat / res);
                }
            }

            int triCount = res * res * 6;
            var triangles = new int[triCount];
            int t = 0;

            for (int lat = 0; lat < res; lat++)
            {
                for (int lon = 0; lon < res; lon++)
                {
                    int current = lat * (res + 1) + lon;
                    int next = current + res + 1;

                    triangles[t++] = current;
                    triangles[t++] = next;
                    triangles[t++] = current + 1;

                    triangles[t++] = current + 1;
                    triangles[t++] = next;
                    triangles[t++] = next + 1;
                }
            }

            var mesh = new Mesh { name = $"Planet_{res}" };
            mesh.vertices = vertices;
            mesh.normals = normals;
            mesh.uv = uvs;
            mesh.triangles = triangles;
            mesh.RecalculateBounds();

            return mesh;
        }

        /// <summary>
        /// Get mesh variant by index.
        /// </summary>
        public Mesh GetMeshVariant(int index)
        {
            if (_meshVariants == null || index < 0 || index >= _meshVariants.Length)
                return null;

            return _meshVariants[index];
        }
    }
}
