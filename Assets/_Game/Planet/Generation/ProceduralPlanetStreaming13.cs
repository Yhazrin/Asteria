using System.Collections.Generic;
using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Streaming system with chunk preloading.
    /// </summary>
    public sealed class ProceduralPlanetStreaming13 : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float preloadDistance = 600f;
        [SerializeField] float loadDistance = 500f;

        [Header("References")]
        [SerializeField] ProceduralPlanetGenerator planetGenerator;
        [SerializeField] Transform player;

        readonly HashSet<string> _preloaded = new();
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

            // Preload chunks within preload distance
            // Load chunks within load distance
            // Unload chunks beyond unload distance
        }

        /// <summary>
        /// Preload a chunk (prepare data but don't activate).
        /// </summary>
        public void PreloadChunk(string chunkId)
        {
            if (!_preloaded.Contains(chunkId))
            {
                _preloaded.Add(chunkId);
                Debug.Log($"[ProceduralPlanetStreaming13] Preloaded chunk: {chunkId}");
            }
        }

        /// <summary>
        /// Load a preloaded chunk.
        /// </summary>
        public void LoadChunk(string chunkId)
        {
            if (_preloaded.Contains(chunkId) && !_loaded.Contains(chunkId))
            {
                _loaded.Add(chunkId);
                Debug.Log($"[ProceduralPlanetStreaming13] Loaded chunk: {chunkId}");
            }
        }
    }
}
