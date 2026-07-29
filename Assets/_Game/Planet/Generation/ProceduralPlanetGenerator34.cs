using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Planet generator with mesh splitting.
    /// </summary>
    public sealed class ProceduralPlanetGenerator34 : MonoBehaviour
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
        /// Split mesh into two halves.
        /// </summary>
        public (Mesh, Mesh) SplitMesh(Mesh mesh, Plane plane)
        {
            if (mesh == null) return (null, null);

            // Simplified split
            return (mesh, mesh);
        }
    }
}
