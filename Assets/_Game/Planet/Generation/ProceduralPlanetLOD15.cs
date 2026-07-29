using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// LOD system with mesh swapping.
    /// </summary>
    public sealed class ProceduralPlanetLOD15 : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] Camera mainCamera;
        [SerializeField] PlanetBody planet;
        [SerializeField] MeshFilter meshFilter;

        Mesh[] _lodMeshes;

        void Start()
        {
            if (mainCamera == null)
                mainCamera = Camera.main;
            if (planet == null)
                planet = FindFirstObjectByType<PlanetBody>();
        }

        /// <summary>
        /// Set LOD meshes.
        /// </summary>
        public void SetLODMeshes(Mesh[] meshes)
        {
            _lodMeshes = meshes;
        }

        /// <summary>
        /// Swap to appropriate LOD mesh.
        /// </summary>
        public void SwapLOD(int lodLevel)
        {
            if (_lodMeshes == null || meshFilter == null) return;
            if (lodLevel < 0 || lodLevel >= _lodMeshes.Length) return;

            meshFilter.mesh = _lodMeshes[lodLevel];
        }
    }
}
