using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Planet generator with mesh reversal.
    /// </summary>
    public sealed class ProceduralPlanetGenerator25 : MonoBehaviour
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
        /// Reverse mesh winding order.
        /// </summary>
        public Mesh ReverseWinding(Mesh mesh)
        {
            if (mesh == null) return null;

            var triangles = mesh.triangles;
            for (int i = 0; i < triangles.Length; i += 3)
            {
                (triangles[i + 1], triangles[i + 2]) = (triangles[i + 2], triangles[i + 1]);
            }

            mesh.triangles = triangles;
            mesh.RecalculateNormals();

            return mesh;
        }
    }
}
