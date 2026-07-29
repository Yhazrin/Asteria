using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// LOD system with mesh offsetting.
    /// </summary>
    public sealed class ProceduralPlanetLOD38 : MonoBehaviour
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
        /// Offset mesh vertices.
        /// </summary>
        public Mesh OffsetMesh(Mesh mesh, Vector3 offset)
        {
            if (mesh == null) return null;

            var vertices = mesh.vertices;
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i] += offset;
            }

            mesh.vertices = vertices;
            mesh.RecalculateBounds();

            return mesh;
        }
    }
}
