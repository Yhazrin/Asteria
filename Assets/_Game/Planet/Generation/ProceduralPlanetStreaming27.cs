using System.Collections.Generic;
using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Streaming system with chunk validation.
    /// </summary>
    public sealed class ProceduralPlanetStreaming27 : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] ProceduralPlanetGenerator planetGenerator;

        void Start()
        {
            if (planetGenerator == null)
                planetGenerator = FindFirstObjectByType<ProceduralPlanetGenerator>();
        }

        /// <summary>
        /// Validate chunk data.
        /// </summary>
        public bool ValidateChunk(string chunkId, Mesh mesh)
        {
            if (mesh == null) return false;
            if (mesh.vertexCount == 0) return false;
            if (mesh.triangles.Length == 0) return false;

            return true;
        }
    }
}
