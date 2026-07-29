using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Planet generator with mesh analysis.
    /// </summary>
    public sealed class ProceduralPlanetGenerator19 : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float planetRadius = 300f;
        [SerializeField] int seed = 42;
        [SerializeField] int resolution = 128;

        [Header("References")]
        [SerializeField] PlanetBody planet;

        void Start()
        {
            if (planet == null)
                planet = FindFirstObjectByType<PlanetBody>();
        }

        /// <summary>
        /// Analyze mesh quality.
        /// </summary>
        public MeshAnalysis AnalyzeMesh(Mesh mesh)
        {
            if (mesh == null) return null;

            return new MeshAnalysis
            {
                vertexCount = mesh.vertexCount,
                triangleCount = mesh.triangles.Length / 3,
                bounds = mesh.bounds,
                hasNormals = mesh.normals != null && mesh.normals.Length > 0,
                hasUVs = mesh.uv != null && mesh.uv.Length > 0
            };
        }

        public class MeshAnalysis
        {
            public int vertexCount;
            public int triangleCount;
            public Bounds bounds;
            public bool hasNormals;
            public bool hasUVs;
        }
    }
}
