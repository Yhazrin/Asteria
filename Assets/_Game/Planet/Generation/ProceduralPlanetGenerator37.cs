using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Planet generator with mesh extrusion.
    /// </summary>
    public sealed class ProceduralPlanetGenerator37 : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float planetRadius = 300f;
        [SerializeField] int seed = 42;
        [SerializeField] float extrudeAmount = 0.1f;

        [Header("References")]
        [SerializeField] PlanetBody planet;

        void Start()
        {
            if (planet == null)
                planet = FindFirstObjectByType<PlanetBody>();
        }

        /// <summary>
        /// Extrude mesh along normals.
        /// </summary>
        public Mesh ExtrudeMesh(Mesh mesh)
        {
            if (mesh == null) return null;

            var vertices = mesh.vertices;
            var normals = mesh.normals;

            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i] += normals[i] * extrudeAmount;
            }

            mesh.vertices = vertices;
            mesh.RecalculateBounds();

            return mesh;
        }
    }
}
