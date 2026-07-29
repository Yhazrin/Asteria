using System.Collections.Generic;
using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Streaming system with chunk merging.
    /// </summary>
    public sealed class ProceduralPlanetStreaming19 : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float mergeDistance = 50f;

        [Header("References")]
        [SerializeField] ProceduralPlanetGenerator planetGenerator;

        readonly Dictionary<string, ChunkGroup> _groups = new();

        void Start()
        {
            if (planetGenerator == null)
                planetGenerator = FindFirstObjectByType<ProceduralPlanetGenerator>();
        }

        /// <summary>
        /// Merge nearby chunks into groups.
        /// </summary>
        public void MergeChunks(string chunkId1, string chunkId2)
        {
            if (!_groups.TryGetValue(chunkId1, out var group1))
            {
                group1 = new ChunkGroup { id = chunkId1 };
                _groups[chunkId1] = group1;
            }

            if (!_groups.TryGetValue(chunkId2, out var group2))
            {
                group2 = new ChunkGroup { id = chunkId2 };
                _groups[chunkId2] = group2;
            }

            // Merge group2 into group1
            group1.chunks.AddRange(group2.chunks);
            _groups.Remove(chunkId2);
        }

        /// <summary>
        /// Get chunk group.
        /// </summary>
        public ChunkGroup GetGroup(string chunkId)
        {
            return _groups.TryGetValue(chunkId, out var group) ? group : null;
        }

        public class ChunkGroup
        {
            public string id;
            public List<string> chunks = new();
        }
    }
}
