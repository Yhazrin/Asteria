using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Planet generator with mesh merging.
    /// </summary>
    public sealed class ProceduralPlanetGenerator22 : MonoBehaviour
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
        /// Merge multiple meshes into one.
        /// </summary>
        public Mesh MergeMeshes(Mesh[] meshes)
        {
            if (meshes == null || meshes.Length == 0) return null;
            if (meshes.Length == 1) return meshes[0];

            int totalVertices = 0;
            int totalTriangles = 0;

            foreach (var mesh in meshes)
            {
                if (mesh == null) continue;
                totalVertices += mesh.vertexCount;
                totalTriangles += mesh.triangles.Length;
            }

            var vertices = new Vector3[totalVertices];
            var normals = new Vector3[totalVertices];
            var uvs = new Vector2[totalVertices];
            var triangles = new int[totalTriangles];

            int vertexOffset = 0;
            int triangleOffset = 0;

            foreach (var mesh in meshes)
            {
                if (mesh == null) continue;

                var meshVertices = mesh.vertices;
                var meshNormals = mesh.normals;
                var meshUVs = mesh.uv;
                var meshTriangles = mesh.triangles;

                for (int i = 0; i < meshVertices.Length; i++)
                {
                    vertices[vertexOffset + i] = meshVertices[i];
                    normals[vertexOffset + i] = meshNormals[i];
                    uvs[vertexOffset + i] = meshUVs[i];
                }

                for (int i = 0; i < meshTriangles.Length; i++)
                {
                    triangles[triangleOffset + i] = meshTriangles[i] + vertexOffset;
                }

                vertexOffset += meshVertices.Length;
                triangleOffset += meshTriangles.Length;
            }

            var merged = new Mesh { name = "MergedMesh" };
            merged.vertices = vertices;
            merged.normals = normals;
            merged.uv = uvs;
            merged.triangles = triangles;
            merged.RecalculateBounds();

            return merged;
        }
    }
}
