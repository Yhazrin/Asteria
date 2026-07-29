using System.Collections.Generic;
using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Streaming system with distance-based prioritization.
    /// </summary>
    public sealed class ProceduralPlanetStreaming11 : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float loadDistance = 500f;
        [SerializeField] float unloadDistance = 800f;

        [Header("References")]
        [SerializeField] ProceduralPlanetGenerator planetGenerator;
        [SerializeField] Transform player;

        readonly SortedList<float, string> _loadQueue = new();
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
            ProcessLoadQueue();
        }

        void ProcessLoadQueue()
        {
            if (_loadQueue.Count == 0) return;

            // Load closest chunk first
            var first = _loadQueue.GetEnumerator();
            if (first.MoveNext())
            {
                string chunkId = first.Current.Value;
                _loadQueue.RemoveAt(0);

                if (!_loaded.Contains(chunkId))
                {
                    LoadChunk(chunkId);
                    _loaded.Add(chunkId);
                }
            }
        }

        void LoadChunk(string chunkId)
        {
            Debug.Log($"[ProceduralPlanetStreaming11] Loaded chunk: {chunkId}");
        }

        /// <summary>
        /// Request chunk load with distance priority.
        /// </summary>
        public void RequestLoad(string chunkId, float distance)
        {
            if (!_loaded.Contains(chunkId))
            {
                _loadQueue[distance] = chunkId;
            }
        }
    }
}
