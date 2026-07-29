using System.Collections.Generic;
using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Streaming system with chunk scheduling.
    /// </summary>
    public sealed class ProceduralPlanetStreaming17 : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float loadDistance = 500f;
        [SerializeField] float unloadDistance = 800f;
        [SerializeField] float scheduleInterval = 0.5f;

        [Header("References")]
        [SerializeField] ProceduralPlanetGenerator planetGenerator;
        [SerializeField] Transform player;

        readonly Queue<string> _scheduledChunks = new();
        float _scheduleTimer;

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
            _scheduleTimer -= Time.deltaTime;
            if (_scheduleTimer <= 0f)
            {
                _scheduleTimer = scheduleInterval;
                ProcessScheduledChunks();
            }
        }

        void ProcessScheduledChunks()
        {
            if (_scheduledChunks.Count == 0) return;

            string chunkId = _scheduledChunks.Dequeue();
            LoadChunk(chunkId);
        }

        void LoadChunk(string chunkId)
        {
            Debug.Log($"[ProceduralPlanetStreaming17] Loading chunk: {chunkId}");
        }

        /// <summary>
        /// Schedule a chunk for loading.
        /// </summary>
        public void ScheduleChunk(string chunkId)
        {
            _scheduledChunks.Enqueue(chunkId);
        }
    }
}
