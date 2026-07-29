using System.Collections;
using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Handles streaming of procedural planet content.
    /// Loads and unloads terrain chunks, vegetation, and features.
    /// </summary>
    public sealed class ProceduralPlanetStreaming : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float loadDistance = 500f;
        [SerializeField] float unloadDistance = 800f;
        [SerializeField] int chunksPerFrame = 2;
        [SerializeField] float updateInterval = 0.5f;

        [Header("References")]
        [SerializeField] ProceduralPlanetGenerator planetGenerator;
        [SerializeField] Transform player;

        // Streaming state
        bool _isStreaming;
        float _updateTimer;
        readonly System.Collections.Generic.Queue<ChunkLoadRequest> _loadQueue = new();
        readonly System.Collections.Generic.Queue<ChunkUnloadRequest> _unloadQueue = new();

        void Start()
        {
            if (planetGenerator == null)
                planetGenerator = FindFirstObjectByType<ProceduralPlanetGenerator>();

            if (player == null)
            {
                var playerBody = FindFirstObjectByType<Player.SphericalGravityBody>();
                player = playerBody?.transform;
            }

            StartStreaming();
        }

        void Update()
        {
            if (!_isStreaming) return;

            _updateTimer -= Time.deltaTime;
            if (_updateTimer <= 0f)
            {
                _updateTimer = updateInterval;
                UpdateStreaming();
            }

            ProcessQueues();
        }

        /// <summary>
        /// Start streaming content.
        /// </summary>
        public void StartStreaming()
        {
            _isStreaming = true;
            Debug.Log("[ProceduralPlanetStreaming] Streaming started.");
        }

        /// <summary>
        /// Stop streaming content.
        /// </summary>
        public void StopStreaming()
        {
            _isStreaming = false;
            Debug.Log("[ProceduralPlanetStreaming] Streaming stopped.");
        }

        void UpdateStreaming()
        {
            if (player == null || planetGenerator == null) return;

            Vector3 playerPos = player.position;
            var planetData = planetGenerator.GetPlanetData();

            if (planetData == null) return;

            // Check terrain chunks
            // (In a real implementation, this would check which chunks are visible)

            // Check vegetation
            var vegetation = planetData.renderer?.GetVegetationSystem();
            if (vegetation != null)
            {
                // Vegetation system handles its own streaming
            }
        }

        void ProcessQueues()
        {
            // Process load queue
            int loaded = 0;
            while (_loadQueue.Count > 0 && loaded < chunksPerFrame)
            {
                var request = _loadQueue.Dequeue();
                LoadChunk(request);
                loaded++;
            }

            // Process unload queue
            while (_unloadQueue.Count > 0)
            {
                var request = _unloadQueue.Dequeue();
                UnloadChunk(request);
            }
        }

        void LoadChunk(ChunkLoadRequest request)
        {
            // Load chunk content
            Debug.Log($"[ProceduralPlanetStreaming] Loading chunk: {request.chunkId}");
        }

        void UnloadChunk(ChunkUnloadRequest request)
        {
            // Unload chunk content
            Debug.Log($"[ProceduralPlanetStreaming] Unloading chunk: {request.chunkId}");
        }

        /// <summary>
        /// Request a chunk to be loaded.
        /// </summary>
        public void RequestChunkLoad(string chunkId, Vector3 position)
        {
            _loadQueue.Enqueue(new ChunkLoadRequest
            {
                chunkId = chunkId,
                position = position
            });
        }

        /// <summary>
        /// Request a chunk to be unloaded.
        /// </summary>
        public void RequestChunkUnload(string chunkId)
        {
            _unloadQueue.Enqueue(new ChunkUnloadRequest
            {
                chunkId = chunkId
            });
        }

        struct ChunkLoadRequest
        {
            public string chunkId;
            public Vector3 position;
        }

        struct ChunkUnloadRequest
        {
            public string chunkId;
        }
    }
}
