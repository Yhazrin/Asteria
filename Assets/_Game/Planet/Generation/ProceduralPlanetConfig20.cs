using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Configuration with decimation parameters.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Procedural Planet Config 20")]
    public sealed class ProceduralPlanetConfig20 : ScriptableObject
    {
        [Header("Planet")]
        public string planetName = "New Planet";
        public float planetRadius = 300f;
        public int seed = 42;

        [Header("Decimation")]
        public int resolution = 128;
        public float decimationFactor = 0.5f;
        public float meshScale = 1f;

        /// <summary>
        /// Generate decimated mesh from config.
        /// </summary>
        public Mesh GenerateMesh()
        {
            int decimatedRes = Mathf.Max(8, Mathf.RoundToInt(resolution * decimationFactor));

            int vertCount = (decimatedRes + 1) * (decimatedRes + 1);
            var vertices = new Vector3[vertCount];
            var normals = new Vector3[vertCount];
            var uvs = new Vector2[vertCount];

            for (int lat = 0; lat <= decimatedRes; lat++)
            {
                float phi = (float)lat / decimatedRes * Mathf.PI;
                for (int lon = 0; lon <= decimatedRes; lon++)
                {
                    float theta = (float)lon / decimatedRes * Mathf.PI * 2f;

                    int idx = lat * (decimatedRes + 1) + lon;

                    float x = Mathf.Sin(phi) * Mathf.Cos(theta);
                    float y = Mathf.Cos(phi);
                    float z = Mathf.Sin(phi) * Mathf.Sin(theta);

                    Vector3 dir = new Vector3(x, y, z).normalized;
                    vertices[idx] = dir * planetRadius * meshScale;
                    normals[idx] = dir;
                    uvs[idx] = new Vector2((float)lon / decimatedRes, (float)lat / decimatedRes);
                }
            }

            int triCount = decimatedRes * decimatedRes * 6;
            var triangles = new int[triCount];
            int t = 0;

            for (int lat = 0; lat < decimatedRes; lat++)
            {
                for (int lon = 0; lon < decimatedRes; lon++)
                {
                    int current = lat * (decimatedRes + 1) + lon;
                    int next = current + decimatedRes + 1;

                    triangles[t++] = current;
                    triangles[t++] = next;
                    triangles[t++] = current + 1;

                    triangles[t++] = current + 1;
                    triangles[t++] = next;
                    triangles[t++] = next + 1;
                }
            }

            var mesh = new Mesh { name = $"{planetName}_Decimated" };
            mesh.vertices = vertices;
            mesh.normals = normals;
            mesh.uv = uvs;
            mesh.triangles = triangles;
            mesh.RecalculateBounds();

            return mesh;
        }
    }
}
