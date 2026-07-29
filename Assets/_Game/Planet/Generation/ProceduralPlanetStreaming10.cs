using System.Collections.Generic;
using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Streaming system with chunk dependencies.
    /// </summary>
    public sealed class ProceduralPlanetStreaming10 : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float loadDistance = 500f;
        [SerializeField] float unloadDistance = 800f;

        [Header("References")]
        [SerializeField] ProceduralPlanetGenerator planetGenerator;
        [SerializeField] Transform player;

        readonly Dictionary<string, ChunkData> _chunks = new();
        readonly Dictionary<string, List<string>> _dependencies = new();

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
            UpdateChunks();
        }

        void UpdateChunks()
        {
            if (player == null) return;

            foreach (var kvp in _chunks)
            {
                float distance = Vector3.Distance(player.position, kvp.Value.position);

                if (distance < loadDistance && !kvp.Value.isLoaded)
                {
                    // Check if dependencies are loaded
                    if (AreDependenciesLoaded(kvp.Key))
                    {
                        LoadChunk(kvp.Key);
                    }
                }
                else if (distance > unloadDistance && kvp.Value.isLoaded)
                {
                    // Check if any loaded chunk depends on this one
                    if (!IsRequiredByOther(kvp.Key))
                    {
                        UnloadChunk(kvp.Key);
                    }
                }
            }
        }

        bool AreDependenciesLoaded(string chunkId)
        {
            if (!_dependencies.TryGetValue(chunkId, out var deps)) return true;

            foreach (var dep in deps)
            {
                if (_chunks.TryGetValue(dep, out var data) && !data.isLoaded)
                    return false;
            }

            return true;
        }

        bool IsRequiredByOther(string chunkId)
        {
            foreach (var kvp in _dependencies)
            {
                if (kvp.Value.Contains(chunkId) && _chunks.TryGetValue(kvp.Key, out var data) && data.isLoaded)
                    return true;
            }

            return false;
        }

        void LoadChunk(string chunkId)
        {
            if (_chunks.TryGetValue(chunkId, out var data))
            {
                data.isLoaded = true;
                Debug.Log($"[ProceduralPlanetStreaming10] Loaded chunk: {chunkId}");
            }
        }

        void UnloadChunk(string chunkId)
        {
            if (_chunks.TryGetValue(chunkId, out var data))
            {
                data.isLoaded = false;
                Debug.Log($"[ProceduralPlanetStreaming10] Unloaded chunk: {chunkId}");
            }
        }

        /// <summary>
        /// Register a chunk with dependencies.
        /// </summary>
        public void RegisterChunk(string chunkId, Vector3 position, string[] dependencies = null)
        {
            _chunks[chunkId] = new ChunkData
            {
                position = position,
                isLoaded = false
            };

            if (dependencies != null && dependencies.Length > 0)
            {
                _dependencies[chunkId] = new List<string>(dependencies);
            }
        }

        class ChunkData
        {
            public Vector3 position;
            public bool isLoaded;
        }
    }
}
