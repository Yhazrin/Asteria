using System.Collections.Generic;
using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Allows real-time terrain deformation on the planet surface.
    /// Supports digging, building up, and smoothing.
    /// </summary>
    public sealed class TerrainDeformation : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float deformationRadius = 5f;
        [SerializeField] float deformationStrength = 1f;
        [SerializeField] float smoothingFactor = 0.5f;
        [SerializeField] int maxDeformations = 100;

        [Header("References")]
        [SerializeField] PlanetBody planet;
        [SerializeField] SphericalTerrainGenerator terrainGenerator;

        readonly List<DeformationRecord> _deformations = new();
        MeshFilter _terrainMeshFilter;
        Vector3[] _originalVertices;
        Vector3[] _deformedVertices;

        void Start()
        {
            if (planet == null)
                planet = FindFirstObjectByType<PlanetBody>();
            if (terrainGenerator == null)
                terrainGenerator = FindFirstObjectByType<SphericalTerrainGenerator>();

            InitializeTerrain();
        }

        void InitializeTerrain()
        {
            _terrainMeshFilter = planet?.GetComponent<MeshFilter>();
            if (_terrainMeshFilter == null) return;

            _originalVertices = _terrainMeshFilter.mesh.vertices;
            _deformedVertices = new Vector3[_originalVertices.Length];
            System.Array.Copy(_originalVertices, _deformedVertices, _originalVertices.Length);
        }

        /// <summary>
        /// Deform terrain at a world position.
        /// </summary>
        public void Deform(Vector3 worldPosition, float strength, DeformType type)
        {
            if (_terrainMeshFilter == null) return;
            if (_deformations.Count >= maxDeformations) return;

            // Convert to sphere direction
            Vector3 direction = (worldPosition - planet.Center).normalized;

            // Find affected vertices
            var affectedVertices = new List<int>();
            for (int i = 0; i < _deformedVertices.Length; i++)
            {
                Vector3 vertexDir = _deformedVertices[i].normalized;
                float angle = Vector3.Angle(direction, vertexDir);

                if (angle < deformationRadius)
                {
                    affectedVertices.Add(i);
                }
            }

            // Apply deformation
            foreach (int idx in affectedVertices)
            {
                Vector3 vertexDir = _deformedVertices[idx].normalized;
                float angle = Vector3.Angle(direction, vertexDir);
                float falloff = 1f - (angle / deformationRadius);
                falloff = Mathf.Pow(falloff, 2f); // Smooth falloff

                float deformation = strength * deformationStrength * falloff;

                switch (type)
                {
                    case DeformType.Dig:
                        _deformedVertices[idx] = vertexDir * (_deformedVertices[idx].magnitude - deformation);
                        break;
                    case DeformType.Build:
                        _deformedVertices[idx] = vertexDir * (_deformedVertices[idx].magnitude + deformation);
                        break;
                    case DeformType.Smooth:
                        // Average with neighbors
                        Vector3 average = Vector3.zero;
                        int count = 0;
                        foreach (int neighborIdx in affectedVertices)
                        {
                            if (neighborIdx != idx)
                            {
                                average += _deformedVertices[neighborIdx];
                                count++;
                            }
                        }
                        if (count > 0)
                        {
                            average /= count;
                            _deformedVertices[idx] = Vector3.Lerp(_deformedVertices[idx], average, smoothingFactor);
                        }
                        break;
                }
            }

            // Apply to mesh
            _terrainMeshFilter.mesh.vertices = _deformedVertices;
            _terrainMeshFilter.mesh.RecalculateNormals();
            _terrainMeshFilter.mesh.RecalculateBounds();

            // Record deformation
            _deformations.Add(new DeformationRecord
            {
                position = direction,
                strength = strength,
                type = type,
                timestamp = Time.time
            });
        }

        /// <summary>
        /// Undo the last deformation.
        /// </summary>
        public void UndoLastDeformation()
        {
            if (_deformations.Count == 0) return;

            _deformations.RemoveAt(_deformations.Count - 1);
            RebuildTerrain();
        }

        /// <summary>
        /// Reset all deformations.
        /// </summary>
        public void ResetDeformations()
        {
            _deformations.Clear();

            if (_terrainMeshFilter != null && _originalVertices != null)
            {
                _terrainMeshFilter.mesh.vertices = _originalVertices;
                _terrainMeshFilter.mesh.RecalculateNormals();
                _terrainMeshFilter.mesh.RecalculateBounds();
                System.Array.Copy(_originalVertices, _deformedVertices, _originalVertices.Length);
            }
        }

        void RebuildTerrain()
        {
            if (_originalVertices == null) return;

            System.Array.Copy(_originalVertices, _deformedVertices, _originalVertices.Length);

            // Replay all deformations
            foreach (var deformation in _deformations)
            {
                Deform(
                    planet.Center + deformation.position * planet.Radius,
                    deformation.strength,
                    deformation.type);
            }
        }

        /// <summary>
        /// Get the height at a world position.
        /// </summary>
        public float GetHeightAtPosition(Vector3 worldPosition)
        {
            if (_terrainMeshFilter == null) return 0f;

            Vector3 direction = (worldPosition - planet.Center).normalized;

            // Find nearest vertex
            float minAngle = float.MaxValue;
            int nearestIdx = 0;

            for (int i = 0; i < _deformedVertices.Length; i++)
            {
                Vector3 vertexDir = _deformedVertices[i].normalized;
                float angle = Vector3.Angle(direction, vertexDir);

                if (angle < minAngle)
                {
                    minAngle = angle;
                    nearestIdx = i;
                }
            }

            return _deformedVertices[nearestIdx].magnitude - planet.Radius;
        }

        public enum DeformType
        {
            Dig,
            Build,
            Smooth
        }

        class DeformationRecord
        {
            public Vector3 position;
            public float strength;
            public DeformType type;
            public float timestamp;
        }
    }
}
