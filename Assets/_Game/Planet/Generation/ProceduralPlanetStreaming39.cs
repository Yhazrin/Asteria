using System.Collections.Generic;
using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Streaming system with chunk warping.
    /// </summary>
    public sealed class ProceduralPlanetStreaming39 : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float warpAmount = 0.1f;

        [Header("References")]
        [SerializeField] ProceduralPlanetGenerator planetGenerator;

        void Start()
        {
            if (planetGenerator == null)
                planetGenerator = FindFirstObjectByType<ProceduralPlanetGenerator>();
        }

        /// <summary>
        /// Warp chunk mesh.
        /// </summary>
        public Mesh WarpChunk(Mesh mesh, float time)
        {
            if (mesh == null) return null;

            var vertices = mesh.vertices;
            for (int i = 0; i < vertices.Length; i++)
            {
                float warp = Mathf.Sin(vertices[i].x * 0.1f + time) * warpAmount;
                vertices[i].y += warp;
            }

            mesh.vertices = vertices;
            mesh.RecalculateNormals();

            return mesh;
        }
    }
}
