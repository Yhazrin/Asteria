using System.Collections.Generic;
using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Level of Detail system for planet terrain.
    /// Automatically adjusts mesh resolution based on camera distance.
    /// </summary>
    public sealed class LODSystem : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] int maxLODLevels = 4;
        [SerializeField] float[] lodDistances = { 100f, 200f, 400f, 800f };
        [SerializeField] int[] lodResolutions = { 128, 64, 32, 16 };

        [Header("References")]
        [SerializeField] Camera mainCamera;
        [SerializeField] PlanetBody planet;

        readonly Dictionary<string, LODChunk> _chunks = new();
        readonly Queue<string> _updateQueue = new();

        void Start()
        {
            if (mainCamera == null)
                mainCamera = Camera.main;
            if (planet == null)
                planet = FindFirstObjectByType<PlanetBody>();
        }

        void Update()
        {
            UpdateLODLevels();
        }

        void UpdateLODLevels()
        {
            if (mainCamera == null || planet == null) return;

            Vector3 cameraPos = mainCamera.transform.position;

            foreach (var kvp in _chunks)
            {
                var chunk = kvp.Value;
                float distance = Vector3.Distance(cameraPos, chunk.center);

                int newLOD = GetLODLevel(distance);
                if (newLOD != chunk.currentLOD)
                {
                    chunk.currentLOD = newLOD;
                    UpdateChunkMesh(chunk);
                }
            }
        }

        int GetLODLevel(float distance)
        {
            for (int i = 0; i < maxLODLevels; i++)
            {
                if (distance < lodDistances[i])
                    return i;
            }
            return maxLODLevels - 1;
        }

        void UpdateChunkMesh(LODChunk chunk)
        {
            if (chunk.currentLOD >= lodResolutions.Length) return;

            int resolution = lodResolutions[chunk.currentLOD];
            Mesh newMesh = GenerateChunkMesh(chunk, resolution);

            var filter = chunk.root.GetComponent<MeshFilter>();
            if (filter != null)
            {
                filter.mesh = newMesh;
            }
        }

        Mesh GenerateChunkMesh(LODChunk chunk, int resolution)
        {
            int vertCount = (resolution + 1) * (resolution + 1);
            var vertices = new Vector3[vertCount];
            var normals = new Vector3[vertCount];
            var uvs = new Vector2[vertCount];

            float angularSize = chunk.angularSize;
            float halfAngle = angularSize * 0.5f;
            float step = angularSize / resolution;

            for (int y = 0; y <= resolution; y++)
            {
                float phi = -halfAngle + y * step;
                for (int x = 0; x <= resolution; x++)
                {
                    float theta = -halfAngle + x * step;

                    int idx = y * (resolution + 1) + x;

                    Vector3 localDir = Quaternion.AngleAxis(theta * Mathf.Rad2Deg, chunk.center.normalized) *
                                       Quaternion.AngleAxis(phi * Mathf.Rad2Deg, Vector3.Cross(chunk.center.normalized, Vector3.up).normalized) *
                                       chunk.center.normalized;

                    Vector3 spherePoint = (chunk.center.normalized + localDir * 0.1f).normalized;
                    float height = SampleHeight(spherePoint);
                    float radius = planet.Radius + height * 10f;

                    vertices[idx] = spherePoint * radius;
                    normals[idx] = spherePoint;
                    uvs[idx] = new Vector2((float)x / resolution, (float)y / resolution);
                }
            }

            // Generate triangles
            int triCount = resolution * resolution * 6;
            var triangles = new int[triCount];
            int t = 0;

            for (int y = 0; y < resolution; y++)
            {
                for (int x = 0; x < resolution; x++)
                {
                    int current = y * (resolution + 1) + x;
                    int next = current + resolution + 1;

                    triangles[t++] = current;
                    triangles[t++] = next;
                    triangles[t++] = current + 1;

                    triangles[t++] = current + 1;
                    triangles[t++] = next;
                    triangles[t++] = next + 1;
                }
            }

            var mesh = new Mesh { name = $"LODChunk_{chunk.id}_{resolution}" };
            mesh.vertices = vertices;
            mesh.normals = normals;
            mesh.uv = uvs;
            mesh.triangles = triangles;
            mesh.RecalculateBounds();

            return mesh;
        }

        float SampleHeight(Vector3 spherePoint)
        {
            // Use noise for height
            float nx = spherePoint.x * 0.005f * planet.Radius;
            float ny = spherePoint.y * 0.005f * planet.Radius;
            float nz = spherePoint.z * 0.005f * planet.Radius;

            return Mathf.PerlinNoise(nx + ny, nz) * 0.5f + 0.5f;
        }

        /// <summary>
        /// Register a chunk for LOD management.
        /// </summary>
        public void RegisterChunk(string id, Vector3 center, float angularSize, GameObject root)
        {
            _chunks[id] = new LODChunk
            {
                id = id,
                center = center,
                angularSize = angularSize,
                root = root,
                currentLOD = 0
            };
        }

        /// <summary>
        /// Unregister a chunk.
        /// </summary>
        public void UnregisterChunk(string id)
        {
            _chunks.Remove(id);
        }

        class LODChunk
        {
            public string id;
            public Vector3 center;
            public float angularSize;
            public GameObject root;
            public int currentLOD;
        }
    }
}
