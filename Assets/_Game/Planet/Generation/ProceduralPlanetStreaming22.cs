using System.Collections.Generic;
using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Streaming system with chunk expiration.
    /// </summary>
    public sealed class ProceduralPlanetStreaming22 : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float expirationTime = 300f; // 5 minutes

        [Header("References")]
        [SerializeField] ProceduralPlanetGenerator planetGenerator;

        readonly Dictionary<string, ChunkExpiry> _chunks = new();

        void Start()
        {
            if (planetGenerator == null)
                planetGenerator = FindFirstObjectByType<ProceduralPlanetGenerator>();
        }

        void Update()
        {
            CheckExpirations();
        }

        void CheckExpirations()
        {
            var expired = new List<string>();

            foreach (var kvp in _chunks)
            {
                if (Time.time - kvp.Value.loadTime > expirationTime)
                {
                    expired.Add(kvp.Key);
                }
            }

            foreach (var chunkId in expired)
            {
                ExpireChunk(chunkId);
            }
        }

        void ExpireChunk(string chunkId)
        {
            _chunks.Remove(chunkId);
            Debug.Log($"[ProceduralPlanetStreaming22] Expired chunk: {chunkId}");
        }

        /// <summary>
        /// Register a chunk with expiration.
        /// </summary>
        public void RegisterChunk(string chunkId)
        {
            _chunks[chunkId] = new ChunkExpiry
            {
                loadTime = Time.time
            };
        }

        class ChunkExpiry
        {
            public float loadTime;
        }
    }
}
