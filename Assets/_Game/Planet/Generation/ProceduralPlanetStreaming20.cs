using System.Collections.Generic;
using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Streaming system with chunk splitting.
    /// </summary>
    public sealed class ProceduralPlanetStreaming20 : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float splitDistance = 200f;

        [Header("References")]
        [SerializeField] ProceduralPlanetGenerator planetGenerator;

        readonly Dictionary<string, ChunkSplit> _splits = new();

        void Start()
        {
            if (planetGenerator == null)
                planetGenerator = FindFirstObjectByType<ProceduralPlanetGenerator>();
        }

        /// <summary>
        /// Split a chunk into smaller pieces.
        /// </summary>
        public void SplitChunk(string chunkId, int pieces)
        {
            _splits[chunkId] = new ChunkSplit
            {
                pieces = pieces,
                timestamp = Time.time
            };
        }

        /// <summary>
        /// Get chunk split info.
        /// </summary>
        public ChunkSplit GetSplitInfo(string chunkId)
        {
            return _splits.TryGetValue(chunkId, out var split) ? split : null;
        }

        public class ChunkSplit
        {
            public int pieces;
            public float timestamp;
        }
    }
}
