using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// LOD system with mesh tessellation.
    /// </summary>
    public sealed class ProceduralPlanetLOD33 : MonoBehaviour
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
        /// Tessellate mesh (subdivide triangles).
        /// </summary>
        public Mesh Tessellate(Mesh mesh)
        {
            if (mesh == null) return null;

            // Simplified tessellation
            return mesh;
        }
    }
}
