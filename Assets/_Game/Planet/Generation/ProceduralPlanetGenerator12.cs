using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Planet generator with LOD mesh variants.
    /// </summary>
    public sealed class ProceduralPlanetGenerator12 : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float planetRadius = 300f;
        [SerializeField] int seed = 42;
        [SerializeField] int[] lodResolutions = { 128, 64, 32, 16 };

        [Header("References")]
        [SerializeField] PlanetBody planet;

        Mesh[] _lodMeshes;

        void Start()
        {
            if (planet == null)
                planet = FindFirstObjectByType<PlanetBody>();

            GenerateLODMeshes();
        }

        void GenerateLODMeshes()
        {
            _lodMeshes = new Mesh[lodResolutions.Length];

            for (int i = 0; i < lodResolutions.Length; i++)
            {
                _lodMeshes[i] = GenerateMesh(lodResolutions[i]);
            }
        }

        Mesh GenerateMesh(int resolution)
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

            var mesh = new Mesh { name = $"Planet_LOD_{resolution}" };
            mesh.vertices = vertices;
            mesh.normals = normals;
            mesh.uv = uvs;
            mesh.triangles = triangles;
            mesh.RecalculateBounds();

            return mesh;
        }

        /// <summary>
        /// Get LOD mesh by level.
        /// </summary>
        public Mesh GetLODMesh(int level)
        {
            if (_lodMeshes == null || level < 0 || level >= _lodMeshes.Length)
                return null;

            return _lodMeshes[level];
        }
    }
}
