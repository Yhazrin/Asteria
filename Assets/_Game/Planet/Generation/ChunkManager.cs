using System.Collections.Generic;
using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Manages spherical chunks for streaming and LOD.
    /// Similar to Minecraft's chunk loading system but for sphere geometry.
    /// </summary>
    public sealed class ChunkManager : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float planetRadius = 300f;
        [SerializeField] int chunksPerRing = 8;
        [SerializeField] int rings = 6;
        [SerializeField] int chunkResolution = 32;
        [SerializeField] float viewDistance = 500f;
        [SerializeField] int maxActiveChunks = 50;

        [Header("References")]
        [SerializeField] Transform viewer;
        [SerializeField] Material terrainMaterial;

        readonly Dictionary<int, SphericalChunk> _chunks = new();
        readonly Dictionary<int, GameObject> _chunkObjects = new();
        readonly Queue<int> _generationQueue = new();
        readonly HashSet<int> _activeChunks = new();

        SphericalTerrainGenerator _terrainGenerator;
        BiomeMapper _biomeMapper;
        bool _initialized;

        void Start()
        {
            _terrainGenerator = GetComponent<SphericalTerrainGenerator>();
            _biomeMapper = new BiomeMapper(42);
            InitializeChunks();
        }

        void InitializeChunks()
        {
            // Generate chunk definitions covering the sphere
            int chunkId = 0;

            for (int ring = 0; ring < rings; ring++)
            {
                float phi = (float)ring / rings * Mathf.PI;
                float ringRadius = Mathf.Sin(phi);
                float ringY = Mathf.Cos(phi);

                int chunksInRing = Mathf.Max(4, Mathf.RoundToInt(chunksPerRing * ringRadius));

                for (int seg = 0; seg < chunksInRing; seg++)
                {
                    float theta = (float)seg / chunksInRing * Mathf.PI * 2f;

                    Vector3 centerDir = new Vector3(
                        ringRadius * Mathf.Cos(theta),
                        ringY,
                        ringRadius * Mathf.Sin(theta)
                    ).normalized;

                    float angularSize = Mathf.PI / rings * 1.5f;

                    _chunks[chunkId] = new SphericalChunk(chunkId, centerDir, angularSize);
                    chunkId++;
                }
            }

            Debug.Log($"[Asteria] Created {_chunks.Count} chunk definitions.");
            _initialized = true;
        }

        void Update()
        {
            if (!_initialized || viewer == null) return;

            UpdateActiveChunks();
            ProcessGenerationQueue();
        }

        void UpdateActiveChunks()
        {
            Vector3 viewerPos = viewer.position;
            Vector3 viewerDir = viewerPos.normalized;

            // Determine which chunks should be active
            var shouldBeActive = new HashSet<int>();

            foreach (var kvp in _chunks)
            {
                var chunk = kvp.Value;
                float dot = Vector3.Dot(chunk.CenterDirection, viewerDir);

                // Frustum + distance check
                if (dot > -0.3f && chunk.IsVisible(viewerPos, planetRadius, viewDistance))
                {
                    shouldBeActive.Add(chunk.ChunkId);
                }
            }

            // Activate new chunks
            foreach (int id in shouldBeActive)
            {
                if (!_activeChunks.Contains(id))
                {
                    ActivateChunk(id);
                }
            }

            // Deactivate distant chunks
            var toDeactivate = new List<int>();
            foreach (int id in _activeChunks)
            {
                if (!shouldBeActive.Contains(id))
                {
                    toDeactivate.Add(id);
                }
            }

            foreach (int id in toDeactivate)
            {
                DeactivateChunk(id);
            }
        }

        void ActivateChunk(int chunkId)
        {
            if (!_chunks.TryGetValue(chunkId, out var chunk)) return;

            // Limit active chunks
            if (_activeChunks.Count >= maxActiveChunks)
            {
                // Remove farthest chunk
                int farthest = FindFarthestChunk();
                if (farthest >= 0) DeactivateChunk(farthest);
            }

            _activeChunks.Add(chunkId);

            // Generate mesh if needed
            if (chunk.IsDirty)
            {
                _generationQueue.Enqueue(chunkId);
            }
            else
            {
                ShowChunk(chunkId);
            }
        }

        void DeactivateChunk(int chunkId)
        {
            _activeChunks.Remove(chunkId);

            if (_chunkObjects.TryGetValue(chunkId, out var obj))
            {
                obj.SetActive(false);
            }
        }

        void ShowChunk(int chunkId)
        {
            if (!_chunks.TryGetValue(chunkId, out var chunk)) return;

            if (!_chunkObjects.TryGetValue(chunkId, out var obj))
            {
                obj = new GameObject($"Chunk_{chunkId}");
                obj.transform.SetParent(transform, false);

                var meshFilter = obj.AddComponent<MeshFilter>();
                var meshRenderer = obj.AddComponent<MeshRenderer>();

                if (terrainMaterial != null)
                {
                    meshRenderer.material = terrainMaterial;
                }

                _chunkObjects[chunkId] = obj;
            }

            var filter = obj.GetComponent<MeshFilter>();
            filter.mesh = chunk.BuildMesh();
            obj.SetActive(true);
        }

        void ProcessGenerationQueue()
        {
            if (_generationQueue.Count == 0) return;

            int chunkId = _generationQueue.Dequeue();
            if (!_chunks.TryGetValue(chunkId, out var chunk)) return;

            chunk.Generate(_terrainGenerator, planetRadius, chunkResolution);

            if (_activeChunks.Contains(chunkId))
            {
                ShowChunk(chunkId);
            }
        }

        int FindFarthestChunk()
        {
            if (viewer == null || _activeChunks.Count == 0) return -1;

            Vector3 viewerDir = viewer.position.normalized;
            float farthestDot = float.MaxValue;
            int farthestId = -1;

            foreach (int id in _activeChunks)
            {
                if (!_chunks.TryGetValue(id, out var chunk)) continue;

                float dot = Vector3.Dot(chunk.CenterDirection, viewerDir);
                if (dot < farthestDot)
                {
                    farthestDot = dot;
                    farthestId = id;
                }
            }

            return farthestId;
        }

        /// <summary>
        /// Get the biome at a world position.
        /// </summary>
        public BiomeMapper.BiomeType GetBiomeAtPosition(Vector3 worldPosition)
        {
            Vector3 spherePoint = worldPosition.normalized;
            return _biomeMapper.GetBiome(spherePoint, planetRadius);
        }

        /// <summary>
        /// Force regenerate all chunks (used when seed changes).
        /// </summary>
        public void RegenerateAll()
        {
            foreach (var chunk in _chunks.Values)
            {
                chunk.Generate(_terrainGenerator, planetRadius, chunkResolution);
            }

            foreach (var kvp in _chunkObjects)
            {
                if (_activeChunks.Contains(kvp.Key))
                {
                    ShowChunk(kvp.Key);
                }
            }
        }
    }
}
