using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Planet generator with mesh inversion.
    /// </summary>
    public sealed class ProceduralPlanetGenerator31 : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float planetRadius = 300f;
        [SerializeField] int seed = 42;

        [Header("References")]
        [SerializeField] PlanetBody planet;

        void Start()
        {
            if (planet == null)
                planet = FindFirstObjectByType<PlanetBody>();
        }

        /// <summary>
        /// Invert mesh normals.
        /// </summary>
        public Mesh InvertNormals(Mesh mesh)
        {
            if (mesh == null) return null;

            var normals = mesh.normals;
            for (int i = 0; i < normals.Length; i++)
            {
                normals[i] = -normals[i];
            }

            mesh.normals = normals;

            // Also reverse triangle winding
            var triangles = mesh.triangles;
            for (int i = 0; i < triangles.Length; i += 3)
            {
                (triangles[i + 1], triangles[i + 2]) = (triangles[i + 2], triangles[i + 1]);
            }

            mesh.triangles = triangles;

            return mesh;
        }
    }
}
