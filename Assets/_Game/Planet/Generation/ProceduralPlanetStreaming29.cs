using System.Collections.Generic;
using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Streaming system with chunk compression.
    /// </summary>
    public sealed class ProceduralPlanetStreaming29 : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] ProceduralPlanetGenerator planetGenerator;

        void Start()
        {
            if (planetGenerator == null)
                planetGenerator = FindFirstObjectByType<ProceduralPlanetGenerator>();
        }

        /// <summary>
        /// Compress chunk data for storage.
        /// </summary>
        public byte[] CompressChunk(Mesh mesh)
        {
            if (mesh == null) return null;

            // Simplified compression
            var vertices = mesh.vertices;
            var bytes = new byte[vertices.Length * 12]; // 3 floats * 4 bytes

            for (int i = 0; i < vertices.Length; i++)
            {
                int offset = i * 12;
                System.Buffer.BlockCopy(new float[] { vertices[i].x }, 0, bytes, offset, 4);
                System.Buffer.BlockCopy(new float[] { vertices[i].y }, 0, bytes, offset + 4, 4);
                System.Buffer.BlockCopy(new float[] { vertices[i].z }, 0, bytes, offset + 8, 4);
            }

            return bytes;
        }
    }
}
