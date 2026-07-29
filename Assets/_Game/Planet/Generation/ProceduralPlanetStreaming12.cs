using System.Collections.Generic;
using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Streaming system with chunk pooling.
    /// </summary>
    public sealed class ProceduralPlanetStreaming12 : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] int poolSize = 20;

        [Header("References")]
        [SerializeField] ProceduralPlanetGenerator planetGenerator;

        readonly Queue<GameObject> _pool = new();
        readonly Dictionary<string, GameObject> _active = new();

        void Start()
        {
            if (planetGenerator == null)
                planetGenerator = FindFirstObjectByType<ProceduralPlanetGenerator>();

            // Initialize pool
            for (int i = 0; i < poolSize; i++)
            {
                var go = new GameObject($"ChunkPool_{i}");
                go.transform.SetParent(transform, false);
                go.SetActive(false);
                _pool.Enqueue(go);
            }
        }

        /// <summary>
        /// Get a chunk from the pool.
        /// </summary>
        public GameObject GetChunk()
        {
            if (_pool.Count > 0)
            {
                var chunk = _pool.Dequeue();
                chunk.SetActive(true);
                return chunk;
            }

            return null;
        }

        /// <summary>
        /// Return a chunk to the pool.
        /// </summary>
        public void ReturnChunk(string chunkId)
        {
            if (_active.TryGetValue(chunkId, out var chunk))
            {
                chunk.SetActive(false);
                _active.Remove(chunkId);
                _pool.Enqueue(chunk);
            }
        }

        /// <summary>
        /// Get pool size.
        /// </summary>
        public int GetPoolSize()
        {
            return _pool.Count;
        }
    }
}
