using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Planet generator with mesh smoothing.
    /// </summary>
    public sealed class ProceduralPlanetGenerator33 : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float planetRadius = 300f;
        [SerializeField] int seed = 42;
        [SerializeField] int smoothIterations = 3;

        [Header("References")]
        [SerializeField] PlanetBody planet;

        void Start()
        {
            if (planet == null)
                planet = FindFirstObjectByType<PlanetBody>();
        }

        /// <summary>
        /// Smooth mesh vertices.
        /// </summary>
        public Mesh SmoothMesh(Mesh mesh)
        {
            if (mesh == null) return null;

            var vertices = mesh.vertices;
            var triangles = mesh.triangles;

            for (int iter = 0; iter < smoothIterations; iter++)
            {
                var newVertices = new Vector3[vertices.Length];
                var counts = new int[vertices.Length];

                for (int i = 0; i < triangles.Length; i += 3)
                {
                    int a = triangles[i];
                    int b = triangles[i + 1];
                    int c = triangles[i + 2];

                    newVertices[a] += vertices[b] + vertices[c];
                    counts[a] += 2;
                    newVertices[b] += vertices[a] + vertices[c];
                    counts[b] += 2;
                    newVertices[c] += vertices[a] + vertices[b];
                    counts[c] += 2;
                }

                for (int i = 0; i < vertices.Length; i++)
                {
                    if (counts[i] > 0)
                    {
                        vertices[i] = newVertices[i] / counts[i];
                    }
                }
            }

            mesh.vertices = vertices;
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();

            return mesh;
        }
    }
}
