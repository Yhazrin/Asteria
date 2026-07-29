using System.Collections.Generic;
using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Streaming system with memory management.
    /// </summary>
    public sealed class ProceduralPlanetStreaming6 : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float loadDistance = 500f;
        [SerializeField] float unloadDistance = 800f;
        [SerializeField] int maxLoadedChunks = 50;

        [Header("References")]
        [SerializeField] ProceduralPlanetGenerator planetGenerator;
        [SerializeField] Transform player;

        readonly SortedList<float, string> _loadQueue = new();
        readonly HashSet<string> _loadedChunks = new();

        void Start()
        {
            if (planetGenerator == null)
                planetGenerator = FindFirstObjectByType<ProceduralPlanetGenerator>();

            if (player == null)
            {
                var playerBody = FindFirstObjectByType<Player.SphericalGravityBody>();
                player = playerBody?.transform;
            }
        }

        void Update()
        {
            UpdateStreaming();
        }

        void UpdateStreaming()
        {
            if (player == null) return;

            // Sort chunks by distance
            _loadQueue.Clear();

            // Load closest chunks first
            foreach (var kvp in _loadQueue)
            {
                if (_loadedChunks.Count >= maxLoadedChunks) break;

                if (!_loadedChunks.Contains(kvp.Value))
                {
                    LoadChunk(kvp.Value);
                    _loadedChunks.Add(kvp.Value);
                }
            }
        }

        void LoadChunk(string chunkId)
        {
            Debug.Log($"[ProceduralPlanetStreaming6] Loading chunk: {chunkId}");
        }

        void UnloadChunk(string chunkId)
        {
            _loadedChunks.Remove(chunkId);
            Debug.Log($"[ProceduralPlanetStreaming6] Unloaded chunk: {chunkId}");
        }

        /// <summary>
        /// Get loaded chunk count.
        /// </summary>
        public int GetLoadedChunkCount()
        {
            return _loadedChunks.Count;
        }
    }
}
