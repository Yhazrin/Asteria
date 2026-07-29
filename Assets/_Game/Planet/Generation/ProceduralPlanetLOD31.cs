using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// LOD system with mesh subsetting.
    /// </summary>
    public sealed class ProceduralPlanetLOD31 : MonoBehaviour
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
        /// Subset mesh to fewer triangles.
        /// </summary>
        public Mesh SubsetMesh(Mesh mesh, int maxTriangles)
        {
            if (mesh == null) return null;

            var triangles = mesh.triangles;
            if (triangles.Length <= maxTriangles * 3) return mesh;

            var newTriangles = new int[maxTriangles * 3];
            System.Array.Copy(triangles, newTriangles, maxTriangles * 3);

            var newMesh = new Mesh();
            newMesh.vertices = mesh.vertices;
            newMesh.normals = mesh.normals;
            newMesh.uv = mesh.uv;
            newMesh.triangles = newTriangles;
            newMesh.RecalculateBounds();

            return newMesh;
        }
    }
}
