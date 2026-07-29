using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// LOD system with mesh color.
    /// </summary>
    public sealed class ProceduralPlanetLOD28 : MonoBehaviour
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
        /// Add vertex colors to mesh.
        /// </summary>
        public Mesh AddVertexColors(Mesh mesh, Color[] colors)
        {
            if (mesh == null || colors == null) return mesh;
            if (colors.Length != mesh.vertexCount) return mesh;

            mesh.colors = colors;
            return mesh;
        }
    }
}
