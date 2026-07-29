using System.Collections.Generic;
using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Streaming system with chunk priorities.
    /// </summary>
    public sealed class ProceduralPlanetStreaming16 : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float loadDistance = 500f;
        [SerializeField] float unloadDistance = 800f;

        [Header("References")]
        [SerializeField] ProceduralPlanetGenerator planetGenerator;
        [SerializeField] Transform player;

        readonly SortedList<int, ChunkPriority> _queue = new();

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
        /// Queue chunk with priority.
        /// </summary>
        public void QueueChunk(string chunkId, int priority)
        {
            _queue[priority] = new ChunkPriority
            {
                chunkId = chunkId,
                priority = priority
            };
        }

        /// <summary>
        /// Dequeue highest priority chunk.
        /// </summary>
        public string DequeueChunk()
        {
            if (_queue.Count == 0) return null;

            var first = _queue.GetEnumerator();
            if (first.MoveNext())
            {
                string chunkId = first.Current.Value.chunkId;
                _queue.RemoveAt(0);
                return chunkId;
            }

            return null;
        }

        class ChunkPriority
        {
            public string chunkId;
            public int priority;
        }
    }
}
