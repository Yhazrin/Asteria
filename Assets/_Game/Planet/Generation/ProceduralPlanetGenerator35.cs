using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Planet generator with mesh reindexing.
    /// </summary>
    public sealed class ProceduralPlanetGenerator35 : MonoBehaviour
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
        /// Reindex mesh triangles.
        /// </summary>
        public Mesh ReindexMesh(Mesh mesh)
        {
            if (mesh == null) return null;

            mesh.Optimize();
            mesh.RecalculateBounds();

            return mesh;
        }
    }
}
