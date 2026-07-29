using System.Collections;
using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Streaming system with async chunk generation.
    /// </summary>
    public sealed class ProceduralPlanetStreaming9 : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float loadDistance = 500f;
        [SerializeField] float unloadDistance = 800f;
        [SerializeField] int maxConcurrentLoads = 3;

        [Header("References")]
        [SerializeField] ProceduralPlanetGenerator planetGenerator;
        [SerializeField] Transform player;

        int _activeLoads;

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
        /// Load a chunk asynchronously.
        /// </summary>
        public void LoadChunkAsync(string chunkId)
        {
            if (_activeLoads >= maxConcurrentLoads) return;

            StartCoroutine(LoadChunkCoroutine(chunkId));
        }

        IEnumerator LoadChunkCoroutine(string chunkId)
        {
            _activeLoads++;

            Debug.Log($"[ProceduralPlanetStreaming9] Loading chunk: {chunkId}");

            // Simulate async work
            yield return new WaitForSeconds(0.2f);

            Debug.Log($"[ProceduralPlanetStreaming9] Chunk loaded: {chunkId}");

            _activeLoads--;
        }
    }
}
