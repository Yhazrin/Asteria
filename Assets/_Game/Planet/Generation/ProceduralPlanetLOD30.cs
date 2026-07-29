using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// LOD system with mesh tangents.
    /// </summary>
    public sealed class ProceduralPlanetLOD30 : MonoBehaviour
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
        /// Add tangents to mesh.
        /// </summary>
        public Mesh AddTangents(Mesh mesh)
        {
            if (mesh == null) return null;

            mesh.RecalculateTangents();
            return mesh;
        }
    }
}
