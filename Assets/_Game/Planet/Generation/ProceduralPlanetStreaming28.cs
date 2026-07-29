using System.Collections.Generic;
using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Streaming system with chunk optimization.
    /// </summary>
    public sealed class ProceduralPlanetStreaming28 : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] ProceduralPlanetGenerator planetGenerator;

        void Start()
        {
            if (planetGenerator == null)
                planetGenerator = FindFirstObjectByType<ProceduralPlanetGenerator>();
        }

        /// <summary>
        /// Optimize chunk mesh.
        /// </summary>
        public Mesh OptimizeChunk(Mesh mesh)
        {
            if (mesh == null) return null;

            mesh.Optimize();
            mesh.RecalculateBounds();
            mesh.RecalculateNormals();

            return mesh;
        }
    }
}
