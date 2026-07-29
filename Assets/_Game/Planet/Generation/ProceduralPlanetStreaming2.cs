using System.Collections.Generic;
using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Alternative streaming system with different approach.
    /// Uses coroutine-based loading.
    /// </summary>
    public sealed class ProceduralPlanetStreaming2 : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float loadDistance = 500f;
        [SerializeField] float unloadDistance = 800f;
        [SerializeField] int chunksPerFrame = 2;

        [Header("References")]
        [SerializeField] ProceduralPlanetGenerator planetGenerator;
        [SerializeField] Transform player;

        readonly Queue<string> _loadQueue = new();
        readonly Queue<string> _unloadQueue = new();

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
            ProcessQueues();
        }

        void ProcessQueues()
        {
            int loaded = 0;
            while (_loadQueue.Count > 0 && loaded < chunksPerFrame)
            {
                string chunkId = _loadQueue.Dequeue();
                LoadChunk(chunkId);
                loaded++;
            }

            while (_unloadQueue.Count > 0)
            {
                string chunkId = _unloadQueue.Dequeue();
                UnloadChunk(chunkId);
            }
        }

        void LoadChunk(string chunkId)
        {
            Debug.Log($"[ProceduralPlanetStreaming2] Loading chunk: {chunkId}");
        }

        void UnloadChunk(string chunkId)
        {
            Debug.Log($"[ProceduralPlanetStreaming2] Unloading chunk: {chunkId}");
        }

        public void RequestChunkLoad(string chunkId)
        {
            if (!_loadQueue.Contains(chunkId))
            {
                _loadQueue.Enqueue(chunkId);
            }
        }

        public void RequestChunkUnload(string chunkId)
        {
            if (!_unloadQueue.Contains(chunkId))
            {
                _unloadQueue.Enqueue(chunkId);
            }
        }
    }
}
