using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Planet generator with mesh stitching.
    /// </summary>
    public sealed class ProceduralPlanetGenerator21 : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float planetRadius = 300f;
        [SerializeField] int seed = 42;

        [Header("References")]
        [SerializeField] PlanetBody planet;

        void Start()
        {
            if (planet == null)
                planet = FindFirstObjectByType<PlanetBody>();
        }

        /// <summary>
        /// Stitch two meshes together.
        /// </summary>
        public Mesh StitchMeshes(Mesh mesh1, Mesh mesh2)
        {
            if (mesh1 == null) return mesh2;
            if (mesh2 == null) return mesh1;

            var vertices1 = mesh1.vertices;
            var vertices2 = mesh2.vertices;
            var normals1 = mesh1.normals;
            var normals2 = mesh2.normals;
            var uvs1 = mesh1.uv;
            var uvs2 = mesh2.uv;
            var triangles1 = mesh1.triangles;
            var triangles2 = mesh2.triangles;

            int offset = vertices1.Length;

            var vertices = new Vector3[vertices1.Length + vertices2.Length];
            vertices1.CopyTo(vertices, 0);
            vertices2.CopyTo(vertices, vertices1.Length);

            var normals = new Vector3[normals1.Length + normals2.Length];
            normals1.CopyTo(normals, 0);
            normals2.CopyTo(normals, normals1.Length);

            var uvs = new Vector2[uvs1.Length + uvs2.Length];
            uvs1.CopyTo(uvs, 0);
            uvs2.CopyTo(uvs, uvs1.Length);

            var triangles = new int[triangles1.Length + triangles2.Length];
            triangles1.CopyTo(triangles, 0);
            for (int i = 0; i < triangles2.Length; i++)
            {
                triangles[triangles1.Length + i] = triangles2[i] + offset;
            }

            var mesh = new Mesh { name = "StitchedMesh" };
            mesh.vertices = vertices;
            mesh.normals = normals;
            mesh.uv = uvs;
            mesh.triangles = triangles;
            mesh.RecalculateBounds();

            return mesh;
        }
    }
}
