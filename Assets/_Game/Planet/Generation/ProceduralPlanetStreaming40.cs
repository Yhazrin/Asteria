using System.Collections.Generic;
using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Streaming system with chunk oscillation.
    /// </summary>
    public sealed class ProceduralPlanetStreaming40 : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float oscillationAmount = 0.5f;

        [Header("References")]
        [SerializeField] ProceduralPlanetGenerator planetGenerator;

        void Start()
        {
            if (planetGenerator == null)
                planetGenerator = FindFirstObjectByType<ProceduralPlanetGenerator>();
        }

        /// <summary>
        /// Apply oscillation to chunk.
        /// </summary>
        public Mesh ApplyOscillation(Mesh mesh, float time)
        {
            if (mesh == null) return null;

            var vertices = mesh.vertices;
            for (int i = 0; i < vertices.Length; i++)
            {
                float oscillation = Mathf.Sin(time + vertices[i].x * 0.1f) * oscillationAmount;
                vertices[i].y += oscillation;
            }

            mesh.vertices = vertices;
            mesh.RecalculateNormals();

            return mesh;
        }
    }
}
