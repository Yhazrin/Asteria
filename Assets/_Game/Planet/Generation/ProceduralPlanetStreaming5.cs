using System.Collections.Generic;
using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Streaming system with distance-based LOD.
    /// </summary>
    public sealed class ProceduralPlanetStreaming5 : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float loadDistance = 500f;
        [SerializeField] float unloadDistance = 800f;

        [Header("References")]
        [SerializeField] ProceduralPlanetGenerator planetGenerator;
        [SerializeField] Transform player;

        readonly Dictionary<string, ChunkState> _chunks = new();

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
                    LoadChunk(kvp.Key);
                }
                else if (distance > unloadDistance && kvp.Value.isLoaded)
                {
                    UnloadChunk(kvp.Key);
                }
            }
        }

        void LoadChunk(string chunkId)
        {
            if (_chunks.TryGetValue(chunkId, out var state))
            {
                state.isLoaded = true;
                Debug.Log($"[ProceduralPlanetStreaming5] Loaded chunk: {chunkId}");
            }
        }

        void UnloadChunk(string chunkId)
        {
            if (_chunks.TryGetValue(chunkId, out var state))
            {
                state.isLoaded = false;
                Debug.Log($"[ProceduralPlanetStreaming5] Unloaded chunk: {chunkId}");
            }
        }

        class ChunkState
        {
            public Vector3 position;
            public bool isLoaded;
        }
    }
}
