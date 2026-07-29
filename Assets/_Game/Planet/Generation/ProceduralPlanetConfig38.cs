using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Configuration with bending parameters.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Procedural Planet Config 38")]
    public sealed class ProceduralPlanetConfig38 : ScriptableObject
    {
        [Header("Planet")]
        public string planetName = "New Planet";
        public float planetRadius = 300f;
        public int seed = 42;

        [Header("Bending")]
        public int resolution = 128;
        public float bendAmount = 0.1f;
        public Vector3 bendAxis = Vector3.up;
        public float meshScale = 1f;

        /// <summary>
        /// Generate mesh from config.
        /// </summary>
        public Mesh GenerateMesh()
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

                    // Apply bending
                    float t = Vector3.Dot(dir * planetRadius, bendAxis);
                    float bend = Mathf.Sin(t * bendAmount) * bendAmount;
                    dir += bendAxis * bend;
                    dir = dir.normalized;

                    vertices[idx] = dir * planetRadius * meshScale;
                    normals[idx] = dir;
                    uvs[idx] = new Vector2((float)lon / resolution, (float)lat / resolution);
                }
            }

            int triCount = resolution * resolution * 6;
            var triangles = new int[triCount];
            int t2 = 0;

            for (int lat = 0; lat < resolution; lat++)
            {
                for (int lon = 0; lon < resolution; lon++)
                {
                    int current = lat * (resolution + 1) + lon;
                    int next = current + resolution + 1;

                    triangles[t2++] = current;
                    triangles[t2++] = next;
                    triangles[t2++] = current + 1;

                    triangles[t2++] = current + 1;
                    triangles[t2++] = next;
                    triangles[t2++] = next + 1;
                }
            }

            var mesh = new Mesh { name = $"{planetName}_Bent" };
            mesh.vertices = vertices;
            mesh.normals = normals;
            mesh.uv = uvs;
            mesh.triangles = triangles;
            mesh.RecalculateBounds();

            return mesh;
        }
    }
}
