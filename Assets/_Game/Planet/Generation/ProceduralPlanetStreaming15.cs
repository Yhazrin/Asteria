using System.Collections.Generic;
using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Streaming system with chunk dependencies.
    /// </summary>
    public sealed class ProceduralPlanetStreaming15 : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float loadDistance = 500f;
        [SerializeField] float unloadDistance = 800f;

        [Header("References")]
        [SerializeField] ProceduralPlanetGenerator planetGenerator;
        [SerializeField] Transform player;

        readonly Dictionary<string, ChunkDependencies> _chunks = new();

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
        /// Register chunk dependencies.
        /// </summary>
        public void RegisterDependencies(string chunkId, string[] dependencies)
        {
            _chunks[chunkId] = new ChunkDependencies
            {
                dependencies = dependencies ?? new string[0]
            };
        }

        /// <summary>
        /// Check if all dependencies are loaded.
        /// </summary>
        public bool AreDependenciesLoaded(string chunkId)
        {
            if (!_chunks.TryGetValue(chunkId, out var chunk)) return true;

            foreach (var dep in chunk.dependencies)
            {
                if (!_chunks.ContainsKey(dep)) return false;
            }

            return true;
        }

        class ChunkDependencies
        {
            public string[] dependencies;
        }
    }
}
