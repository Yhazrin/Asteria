using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// LOD system with mesh morphing.
    /// </summary>
    public sealed class ProceduralPlanetLOD25 : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float morphSpeed = 2f;

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
        /// Morph between two meshes.
        /// </summary>
        public Mesh MorphMeshes(Mesh from, Mesh to, float t)
        {
            if (from == null || to == null) return from;
            if (from.vertexCount != to.vertexCount) return from;

            var vertices = new Vector3[from.vertexCount];
            var fromVerts = from.vertices;
            var toVerts = to.vertices;

            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i] = Vector3.Lerp(fromVerts[i], toVerts[i], t);
            }

            var mesh = new Mesh();
            mesh.vertices = vertices;
            mesh.normals = from.normals;
            mesh.uv = from.uv;
            mesh.triangles = from.triangles;
            mesh.RecalculateBounds();

            return mesh;
        }
    }
}
