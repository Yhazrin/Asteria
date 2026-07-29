using System.Collections.Generic;
using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Streaming system with chunk versioning.
    /// </summary>
    public sealed class ProceduralPlanetStreaming14 : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float loadDistance = 500f;
        [SerializeField] float unloadDistance = 800f;

        [Header("References")]
        [SerializeField] ProceduralPlanetGenerator planetGenerator;
        [SerializeField] Transform player;

        readonly Dictionary<string, ChunkVersion> _chunks = new();

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

        /// <summary>
        /// Check if a chunk needs updating.
        /// </summary>
        public bool NeedsUpdate(string chunkId, int currentVersion)
        {
            if (_chunks.TryGetValue(chunkId, out var chunk))
            {
                return chunk.version < currentVersion;
            }

            return true; // New chunk
        }

        /// <summary>
        /// Update chunk version.
        /// </summary>
        public void UpdateVersion(string chunkId, int version)
        {
            _chunks[chunkId] = new ChunkVersion
            {
                version = version,
                timestamp = Time.time
            };
        }

        class ChunkVersion
        {
            public int version;
            public float timestamp;
        }
    }
}
