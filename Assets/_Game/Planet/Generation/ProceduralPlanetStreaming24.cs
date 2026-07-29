using System.Collections.Generic;
using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Streaming system with chunk batching.
    /// </summary>
    public sealed class ProceduralPlanetStreaming24 : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] int batchSize = 5;

        [Header("References")]
        [SerializeField] ProceduralPlanetGenerator planetGenerator;

        readonly Queue<string> _batch = new();

        void Start()
        {
            if (planetGenerator == null)
                planetGenerator = FindFirstObjectByType<ProceduralPlanetGenerator>();
        }

        void Update()
        {
            ProcessBatch();
        }

        void ProcessBatch()
        {
            int processed = 0;
            while (_batch.Count > 0 && processed < batchSize)
            {
                string chunkId = _batch.Dequeue();
                LoadChunk(chunkId);
                processed++;
            }
        }

        void LoadChunk(string chunkId)
        {
            // Load chunk
        }

        /// <summary>
        /// Add chunk to batch.
        /// </summary>
        public void AddToBatch(string chunkId)
        {
            _batch.Enqueue(chunkId);
        }
    }
}
