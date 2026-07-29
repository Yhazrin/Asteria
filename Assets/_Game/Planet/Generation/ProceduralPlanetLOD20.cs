using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// LOD system with mesh optimization.
    /// </summary>
    public sealed class ProceduralPlanetLOD20 : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] Camera mainCamera;
        [SerializeField] PlanetBody planet;

        void Start()
        {
            if (mainCamera == null)
                mainCamera = Camera.main;
            if (planet == null)
                planet = FindFirstObjectByType<PlanetBody>();
        }

        /// <summary>
        /// Optimize mesh for LOD.
        /// </summary>
        public Mesh OptimizeMesh(Mesh mesh)
        {
            if (mesh == null) return null;

            mesh.Optimize();
            mesh.RecalculateBounds();
            mesh.RecalculateNormals();

            return mesh;
        }
    }
}
