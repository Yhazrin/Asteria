using System.Collections;
using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Streaming system with async loading.
    /// </summary>
    public sealed class ProceduralPlanetStreaming3 : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float loadDistance = 500f;
        [SerializeField] float unloadDistance = 800f;

        [Header("References")]
        [SerializeField] ProceduralPlanetGenerator planetGenerator;
        [SerializeField] Transform player;

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
            StartCoroutine(LoadChunkCoroutine(chunkId));
        }

        IEnumerator LoadChunkCoroutine(string chunkId)
        {
            Debug.Log($"[ProceduralPlanetStreaming3] Loading chunk: {chunkId}");

            // Simulate async loading
            yield return new WaitForSeconds(0.1f);

            Debug.Log($"[ProceduralPlanetStreaming3] Chunk loaded: {chunkId}");
        }

        /// <summary>
        /// Unload a chunk asynchronously.
        /// </summary>
        public void UnloadChunkAsync(string chunkId)
        {
            StartCoroutine(UnloadChunkCoroutine(chunkId));
        }

        IEnumerator UnloadChunkCoroutine(string chunkId)
        {
            Debug.Log($"[ProceduralPlanetStreaming3] Unloading chunk: {chunkId}");

            yield return new WaitForSeconds(0.05f);

            Debug.Log($"[ProceduralPlanetStreaming3] Chunk unloaded: {chunkId}");
        }
    }
}
