using System.Collections.Generic;
using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Streaming system with priority queue.
    /// </summary>
    public sealed class ProceduralPlanetStreaming8 : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float loadDistance = 500f;
        [SerializeField] float unloadDistance = 800f;

        [Header("References")]
        [SerializeField] ProceduralPlanetGenerator planetGenerator;
        [SerializeField] Transform player;

        readonly SortedDictionary<float, string> _loadQueue = new();
        readonly HashSet<string> _loaded = new();

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

            // Process load queue (closest first)
            var toLoad = new List<string>();
            foreach (var kvp in _loadQueue)
            {
                if (!_loaded.Contains(kvp.Value))
                {
                    toLoad.Add(kvp.Value);
                }
            }

            foreach (var chunkId in toLoad)
            {
                LoadChunk(chunkId);
                _loaded.Add(chunkId);
            }

            _loadQueue.Clear();
        }

        void LoadChunk(string chunkId)
        {
            Debug.Log($"[ProceduralPlanetStreaming8] Loaded chunk: {chunkId}");
        }

        /// <summary>
        /// Request a chunk to be loaded.
        /// </summary>
        public void RequestLoad(string chunkId, float distance)
        {
            _loadQueue[distance] = chunkId;
        }

        /// <summary>
        /// Get loaded chunk count.
        /// </summary>
        public int GetLoadedCount()
        {
            return _loaded.Count;
        }
    }
}
