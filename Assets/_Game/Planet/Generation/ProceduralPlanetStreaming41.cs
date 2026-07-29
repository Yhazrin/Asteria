using System.Collections.Generic;
using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Streaming system with chunk breathing.
    /// </summary>
    public sealed class ProceduralPlanetStreaming41 : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float breatheAmount = 0.05f;

        [Header("References")]
        [SerializeField] ProceduralPlanetGenerator planetGenerator;

        void Start()
        {
            if (planetGenerator == null)
                planetGenerator = FindFirstObjectByType<ProceduralPlanetGenerator>();
        }

        /// <summary>
        /// Apply breathing to chunk.
        /// </summary>
        public Mesh ApplyBreathing(Mesh mesh, float time)
        {
            if (mesh == null) return null;

            var vertices = mesh.vertices;
            float breathe = Mathf.Sin(time * 0.3f) * breatheAmount;

            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i] = vertices[i].normalized * (vertices[i].magnitude + breathe);
            }

            mesh.vertices = vertices;
            mesh.RecalculateNormals();

            return mesh;
        }
    }
}
