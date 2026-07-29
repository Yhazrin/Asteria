using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Planet generator with mesh statistics.
    /// </summary>
    public sealed class ProceduralPlanetGenerator28 : MonoBehaviour
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
        /// Get mesh statistics.
        /// </summary>
        public MeshStats GetStats(Mesh mesh)
        {
            if (mesh == null) return null;

            return new MeshStats
            {
                vertexCount = mesh.vertexCount,
                triangleCount = mesh.triangles.Length / 3,
                boundsSize = mesh.bounds.size,
                hasNormals = mesh.normals != null && mesh.normals.Length > 0,
                hasUVs = mesh.uv != null && mesh.uv.Length > 0,
                hasColors = mesh.colors != null && mesh.colors.Length > 0
            };
        }

        public class MeshStats
        {
            public int vertexCount;
            public int triangleCount;
            public Vector3 boundsSize;
            public bool hasNormals;
            public bool hasUVs;
            public bool hasColors;
        }
    }
}
