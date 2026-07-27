using UnityEngine;

namespace Asteria.Planet
{
    /// <summary>
    /// Builds a smoother UV sphere mesh than Unity's built-in low-poly sphere.
    /// </summary>
    public static class PlanetMeshFactory
    {
        public static Mesh CreateUvSphere(int longitudeSegments, int latitudeSegments, float radius = 0.5f)
        {
            longitudeSegments = Mathf.Max(3, longitudeSegments);
            latitudeSegments = Mathf.Max(2, latitudeSegments);

            int vertexCount = (longitudeSegments + 1) * (latitudeSegments + 1);
            var vertices = new Vector3[vertexCount];
            var normals = new Vector3[vertexCount];
            var uvs = new Vector2[vertexCount];

            int index = 0;
            for (int lat = 0; lat <= latitudeSegments; lat++)
            {
                float v = (float)lat / latitudeSegments;
                float phi = v * Mathf.PI;
                float y = Mathf.Cos(phi);
                float ringRadius = Mathf.Sin(phi);

                for (int lon = 0; lon <= longitudeSegments; lon++)
                {
                    float u = (float)lon / longitudeSegments;
                    float theta = u * Mathf.PI * 2f;
                    float x = ringRadius * Mathf.Cos(theta);
                    float z = ringRadius * Mathf.Sin(theta);
                    Vector3 point = new Vector3(x, y, z);
                    vertices[index] = point * radius;
                    normals[index] = point.normalized;
                    uvs[index] = new Vector2(u, 1f - v);
                    index++;
                }
            }

            int quadCount = longitudeSegments * latitudeSegments;
            var triangles = new int[quadCount * 6];
            int t = 0;
            for (int lat = 0; lat < latitudeSegments; lat++)
            {
                for (int lon = 0; lon < longitudeSegments; lon++)
                {
                    int current = lat * (longitudeSegments + 1) + lon;
                    int next = current + longitudeSegments + 1;

                    triangles[t++] = current;
                    triangles[t++] = next;
                    triangles[t++] = current + 1;

                    triangles[t++] = current + 1;
                    triangles[t++] = next;
                    triangles[t++] = next + 1;
                }
            }

            var mesh = new Mesh
            {
                name = $"UvSphere_{longitudeSegments}x{latitudeSegments}",
                indexFormat = vertexCount > 65000
                    ? UnityEngine.Rendering.IndexFormat.UInt32
                    : UnityEngine.Rendering.IndexFormat.UInt16
            };
            mesh.vertices = vertices;
            mesh.normals = normals;
            mesh.uv = uvs;
            mesh.triangles = triangles;
            mesh.RecalculateBounds();
            return mesh;
        }
    }
}
