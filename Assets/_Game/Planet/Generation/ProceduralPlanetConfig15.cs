using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Configuration with simplification parameters.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Procedural Planet Config 15")]
    public sealed class ProceduralPlanetConfig15 : ScriptableObject
    {
        [Header("Planet")]
        public string planetName = "New Planet";
        public float planetRadius = 300f;
        public int seed = 42;

        [Header("Simplification")]
        public int resolution = 128;
        public float simplificationFactor = 0.5f;
        public float meshScale = 1f;

        /// <summary>
        /// Generate simplified mesh from config.
        /// </summary>
        public Mesh GenerateMesh()
        {
            int simplifiedRes = Mathf.Max(8, Mathf.RoundToInt(resolution * simplificationFactor));

            int vertCount = (simplifiedRes + 1) * (simplifiedRes + 1);
            var vertices = new Vector3[vertCount];
            var normals = new Vector3[vertCount];
            var uvs = new Vector2[vertCount];

            for (int lat = 0; lat <= simplifiedRes; lat++)
            {
                float phi = (float)lat / simplifiedRes * Mathf.PI;
                for (int lon = 0; lon <= simplifiedRes; lon++)
                {
                    float theta = (float)lon / simplifiedRes * Mathf.PI * 2f;

                    int idx = lat * (simplifiedRes + 1) + lon;

                    float x = Mathf.Sin(phi) * Mathf.Cos(theta);
                    float y = Mathf.Cos(phi);
                    float z = Mathf.Sin(phi) * Mathf.Sin(theta);

                    Vector3 dir = new Vector3(x, y, z).normalized;
                    vertices[idx] = dir * planetRadius * meshScale;
                    normals[idx] = dir;
                    uvs[idx] = new Vector2((float)lon / simplifiedRes, (float)lat / simplifiedRes);
                }
            }

            int triCount = simplifiedRes * simplifiedRes * 6;
            var triangles = new int[triCount];
            int t = 0;

            for (int lat = 0; lat < simplifiedRes; lat++)
            {
                for (int lon = 0; lon < simplifiedRes; lon++)
                {
                    int current = lat * (simplifiedRes + 1) + lon;
                    int next = current + simplifiedRes + 1;

                    triangles[t++] = current;
                    triangles[t++] = next;
                    triangles[t++] = current + 1;

                    triangles[t++] = current + 1;
                    triangles[t++] = next;
                    triangles[t++] = next + 1;
                }
            }

            var mesh = new Mesh { name = $"{planetName}_Simplified" };
            mesh.vertices = vertices;
            mesh.normals = normals;
            mesh.uv = uvs;
            mesh.triangles = triangles;
            mesh.RecalculateBounds();

            return mesh;
        }
    }
}
