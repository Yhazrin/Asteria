using System.Collections.Generic;
using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// A chunk of the spherical planet for LOD and streaming.
    /// Similar to Minecraft's chunk system but adapted for sphere geometry.
    /// </summary>
    public sealed class SphericalChunk
    {
        public readonly int ChunkId;
        public readonly Vector3 CenterDirection; // Normalized direction from planet center
        public readonly float AngularSize;       // Size in radians

        readonly List<Vector3> _vertices = new();
        readonly List<int> _triangles = new();
        readonly List<Color> _colors = new();
        readonly List<Vector3> _normals = new();

        Mesh _mesh;
        bool _isDirty = true;

        public Mesh Mesh => _mesh;
        public bool IsDirty => _isDirty;
        public int VertexCount => _vertices.Count;

        public SphericalChunk(int id, Vector3 centerDir, float angularSize)
        {
            ChunkId = id;
            CenterDirection = centerDir.normalized;
            AngularSize = angularSize;
        }

        /// <summary>
        /// Generate mesh data for this chunk.
        /// </summary>
        public void Generate(SphericalTerrainGenerator generator, float planetRadius, int resolution)
        {
            _vertices.Clear();
            _triangles.Clear();
            _colors.Clear();
            _normals.Clear();

            // Calculate local coordinate frame
            Vector3 up = CenterDirection;
            Vector3 forward = Vector3.Cross(up, Vector3.up).normalized;
            if (forward.sqrMagnitude < 0.001f) forward = Vector3.Cross(up, Vector3.right).normalized;
            Vector3 right = Vector3.Cross(up, forward).normalized;

            float halfAngle = AngularSize * 0.5f;
            float step = AngularSize / resolution;

            // Generate vertices
            for (int y = 0; y <= resolution; y++)
            {
                float phi = -halfAngle + y * step;

                for (int x = 0; x <= resolution; x++)
                {
                    float theta = -halfAngle + x * step;

                    // Local direction on sphere
                    Vector3 localDir = Quaternion.AngleAxis(theta * Mathf.Rad2Deg, up) *
                                       Quaternion.AngleAxis(phi * Mathf.Rad2Deg, right) * forward;

                    Vector3 spherePoint = (CenterDirection + localDir).normalized;

                    // Sample terrain
                    float height = 0; // generator.SampleTerrainHeight(spherePoint);
                    float biome = 0;  // generator.SampleBiome(spherePoint);

                    // Apply height
                    float finalRadius = planetRadius + height * 10f;
                    Vector3 vertex = spherePoint * finalRadius;

                    _vertices.Add(vertex);
                    _normals.Add(spherePoint);
                    _colors.Add(BiomeMapper.GetBiomeColor(BiomeMapper.BiomeType.Plains, height));
                }
            }

            // Generate triangles
            for (int y = 0; y < resolution; y++)
            {
                for (int x = 0; x < resolution; x++)
                {
                    int current = y * (resolution + 1) + x;
                    int next = current + resolution + 1;

                    _triangles.Add(current);
                    _triangles.Add(next);
                    _triangles.Add(current + 1);

                    _triangles.Add(current + 1);
                    _triangles.Add(next);
                    _triangles.Add(next + 1);
                }
            }

            _isDirty = true;
        }

        /// <summary>
        /// Build the Unity mesh from generated data.
        /// </summary>
        public Mesh BuildMesh()
        {
            if (!_isDirty && _mesh != null) return _mesh;

            if (_mesh == null)
            {
                _mesh = new Mesh { name = $"Chunk_{ChunkId}" };
            }
            else
            {
                _mesh.Clear();
            }

            _mesh.SetVertices(_vertices);
            _mesh.SetNormals(_normals);
            _mesh.SetColors(_colors);
            _mesh.SetTriangles(_triangles, 0);
            _mesh.RecalculateBounds();

            _isDirty = false;
            return _mesh;
        }

        /// <summary>
        /// Check if this chunk is visible from a viewpoint.
        /// </summary>
        public bool IsVisible(Vector3 viewPosition, float planetRadius, float maxDistance)
        {
            float dot = Vector3.Dot(CenterDirection, viewPosition.normalized);
            return dot > -0.3f; // Visible if not too far behind sphere
        }

        /// <summary>
        /// Get the LOD level based on distance.
        /// </summary>
        public int GetLODLevel(Vector3 viewPosition, float planetRadius)
        {
            float distance = Vector3.Distance(viewPosition, CenterDirection * planetRadius);

            if (distance < planetRadius * 0.5f) return 0; // High detail
            if (distance < planetRadius * 1.0f) return 1; // Medium detail
            return 2; // Low detail
        }
    }
}
