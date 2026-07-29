using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Configuration with LOD mesh parameters.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Procedural Planet Config 12")]
    public sealed class ProceduralPlanetConfig12 : ScriptableObject
    {
        [Header("Planet")]
        public string planetName = "New Planet";
        public float planetRadius = 300f;
        public int seed = 42;

        [Header("LOD")]
        public int[] lodResolutions = { 128, 64, 32, 16 };
        public float meshScale = 1f;

        /// <summary>
        /// Generate LOD meshes from config.
        /// </summary>
        public Mesh[] GenerateLODMeshes()
        {
            var meshes = new Mesh[lodResolutions.Length];

            for (int i = 0; i < lodResolutions.Length; i++)
            {
                meshes[i] = GenerateMesh(lodResolutions[i]);
            }

            return meshes;
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
                    vertices[idx] = dir * planetRadius * meshScale;
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

            var mesh = new Mesh { name = $"{planetName}_LOD_{resolution}" };
            mesh.vertices = vertices;
            mesh.normals = normals;
            mesh.uv = uvs;
            mesh.triangles = triangles;
            mesh.RecalculateBounds();

            return mesh;
        }
    }
}
