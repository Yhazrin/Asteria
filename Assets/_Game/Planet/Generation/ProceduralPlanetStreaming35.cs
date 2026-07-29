using System.Collections.Generic;
using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Streaming system with chunk deduplication.
    /// </summary>
    public sealed class ProceduralPlanetStreaming35 : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] ProceduralPlanetGenerator planetGenerator;

        readonly HashSet<string> _loadedChunks = new();

        void Start()
        {
            if (planetGenerator == null)
                planetGenerator = FindFirstObjectByType<ProceduralPlanetGenerator>();
        }

        /// <summary>
        /// Check if chunk is already loaded.
        /// </summary>
        public bool IsChunkLoaded(string chunkId)
        {
            return _loadedChunks.Contains(chunkId);
        }

        /// <summary>
        /// Mark chunk as loaded.
        /// </summary>
        public void MarkLoaded(string chunkId)
        {
            _loadedChunks.Add(chunkId);
        }

        /// <summary>
        /// Mark chunk as unloaded.
        /// </summary>
        public void MarkUnloaded(string chunkId)
        {
            _loadedChunks.Remove(chunkId);
        }
    }
}
