using System.Collections.Generic;
using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Streaming system with chunk merging.
    /// </summary>
    public sealed class ProceduralPlanetStreaming34 : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] ProceduralPlanetGenerator planetGenerator;

        void Start()
        {
            if (planetGenerator == null)
                planetGenerator = FindFirstObjectByType<ProceduralPlanetGenerator>();
        }

        /// <summary>
        /// Merge multiple chunk meshes.
        /// </summary>
        public Mesh MergeChunks(Mesh[] chunks)
        {
            if (chunks == null || chunks.Length == 0) return null;
            if (chunks.Length == 1) return chunks[0];

            int totalVertices = 0;
            int totalTriangles = 0;

            foreach (var chunk in chunks)
            {
                if (chunk == null) continue;
                totalVertices += chunk.vertexCount;
                totalTriangles += chunk.triangles.Length;
            }

            var vertices = new Vector3[totalVertices];
            var normals = new Vector3[totalVertices];
            var uvs = new Vector2[totalVertices];
            var triangles = new int[totalTriangles];

            int vertexOffset = 0;
            int triangleOffset = 0;

            foreach (var chunk in chunks)
            {
                if (chunk == null) continue;

                var chunkVertices = chunk.vertices;
                var chunkNormals = chunk.normals;
                var chunkUVs = chunk.uv;
                var chunkTriangles = chunk.triangles;

                for (int i = 0; i < chunkVertices.Length; i++)
                {
                    vertices[vertexOffset + i] = chunkVertices[i];
                    normals[vertexOffset + i] = chunkNormals[i];
                    uvs[vertexOffset + i] = chunkUVs[i];
                }

                for (int i = 0; i < chunkTriangles.Length; i++)
                {
                    triangles[triangleOffset + i] = chunkTriangles[i] + vertexOffset;
                }

                vertexOffset += chunkVertices.Length;
                triangleOffset += chunkTriangles.Length;
            }

            var merged = new Mesh { name = "MergedChunks" };
            merged.vertices = vertices;
            merged.normals = normals;
            merged.uv = uvs;
            merged.triangles = triangles;
            merged.RecalculateBounds();

            return merged;
        }
    }
}
