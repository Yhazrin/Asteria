using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// LOD system with mesh compression.
    /// </summary>
    public sealed class ProceduralPlanetLOD22 : MonoBehaviour
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
        /// Compress mesh for LOD.
        /// </summary>
        public Mesh CompressMesh(Mesh mesh)
        {
            if (mesh == null) return null;

            // Unity mesh compression
            mesh.Optimize();
            mesh.RecalculateBounds();

            return mesh;
        }
    }
}
