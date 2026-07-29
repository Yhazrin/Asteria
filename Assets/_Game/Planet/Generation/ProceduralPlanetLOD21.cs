using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// LOD system with mesh validation.
    /// </summary>
    public sealed class ProceduralPlanetLOD21 : MonoBehaviour
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
        /// Validate mesh for LOD.
        /// </summary>
        public bool ValidateMesh(Mesh mesh)
        {
            if (mesh == null) return false;
            if (mesh.vertexCount == 0) return false;
            if (mesh.triangles.Length == 0) return false;

            return true;
        }
    }
}
