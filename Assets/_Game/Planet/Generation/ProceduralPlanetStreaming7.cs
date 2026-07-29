using System.Collections;
using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Streaming system with coroutine-based unloading.
    /// </summary>
    public sealed class ProceduralPlanetStreaming7 : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float loadDistance = 500f;
        [SerializeField] float unloadDistance = 800f;
        [SerializeField] float unloadDelay = 5f;

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
        /// Schedule a chunk for delayed unloading.
        /// </summary>
        public void ScheduleUnload(string chunkId, float delay)
        {
            StartCoroutine(UnloadAfterDelay(chunkId, delay));
        }

        IEnumerator UnloadAfterDelay(string chunkId, float delay)
        {
            yield return new WaitForSeconds(delay);
            UnloadChunk(chunkId);
        }

        void UnloadChunk(string chunkId)
        {
            Debug.Log($"[ProceduralPlanetStreaming7] Unloaded chunk: {chunkId}");
        }
    }
}
