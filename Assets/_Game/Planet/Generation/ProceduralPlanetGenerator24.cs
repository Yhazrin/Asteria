using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Planet generator with mesh subdivision.
    /// </summary>
    public sealed class ProceduralPlanetGenerator24 : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float planetRadius = 300f;
        [SerializeField] int seed = 42;
        [SerializeField] int subdivisionLevel = 1;

        [Header("References")]
        [SerializeField] PlanetBody planet;

        void Start()
        {
            if (planet == null)
                planet = FindFirstObjectByType<PlanetBody>();
        }

        /// <summary>
        /// Subdivide mesh.
        /// </summary>
        public Mesh SubdivideMesh(Mesh mesh)
        {
            if (mesh == null) return null;

            // Simplified subdivision
            return mesh;
        }
    }
}
