using System.Collections.Generic;
using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Streaming system with chunk recycling.
    /// </summary>
    public sealed class ProceduralPlanetStreaming18 : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] int maxRecycledChunks = 10;

        [Header("References")]
        [SerializeField] ProceduralPlanetGenerator planetGenerator;

        readonly Queue<string> _recycledChunks = new();

        void Start()
        {
            if (planetGenerator == null)
                planetGenerator = FindFirstObjectByType<ProceduralPlanetGenerator>();
        }

        /// <summary>
        /// Recycle a chunk for reuse.
        /// </summary>
        public void RecycleChunk(string chunkId)
        {
            if (_recycledChunks.Count < maxRecycledChunks)
            {
                _recycledChunks.Enqueue(chunkId);
            }
        }

        /// <summary>
        /// Get a recycled chunk.
        /// </summary>
        public string GetRecycledChunk()
        {
            return _recycledChunks.Count > 0 ? _recycledChunks.Dequeue() : null;
        }

        /// <summary>
        /// Get recycled chunk count.
        /// </summary>
        public int GetRecycledCount()
        {
            return _recycledChunks.Count;
        }
    }
}
