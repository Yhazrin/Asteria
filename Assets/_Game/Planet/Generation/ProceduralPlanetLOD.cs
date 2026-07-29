using System.Collections.Generic;
using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// LOD (Level of Detail) system for procedural planets.
    /// Manages mesh resolution based on camera distance.
    /// </summary>
    public sealed class ProceduralPlanetLOD : MonoBehaviour
    {
        [Header("LOD Settings")]
        [SerializeField] int maxLODLevels = 4;
        [SerializeField] float[] lodDistances = { 100f, 200f, 400f, 800f };
        [SerializeField] int[] lodResolutions = { 128, 64, 32, 16 };

        [Header("References")]
        [SerializeField] Camera mainCamera;
        [SerializeField] PlanetBody planet;

        readonly Dictionary<string, LODGroup> _lodGroups = new();
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
            ProcessUpdateQueue();
        }

        void UpdateLODLevels()
        {
            if (mainCamera == null || planet == null) return;

            Vector3 cameraPos = mainCamera.transform.position;

            foreach (var kvp in _lodGroups)
            {
                var group = kvp.Value;
                float distance = Vector3.Distance(cameraPos, group.center);

                int newLOD = GetLODLevel(distance);
                if (newLOD != group.currentLOD)
                {
                    group.currentLOD = newLOD;
                    _updateQueue.Enqueue(kvp.Key);
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

        void ProcessUpdateQueue()
        {
            int processed = 0;
            while (_updateQueue.Count > 0 && processed < 5)
            {
                string groupId = _updateQueue.Dequeue();
                if (_lodGroups.TryGetValue(groupId, out var group))
                {
                    UpdateGroupMesh(group);
                }
                processed++;
            }
        }

        void UpdateGroupMesh(LODGroup group)
        {
            if (group.currentLOD >= lodResolutions.Length) return;

            int resolution = lodResolutions[group.currentLOD];
            // Generate new mesh at this resolution
            // (In real implementation, this would regenerate the mesh)
        }

        /// <summary>
        /// Register a LOD group.
        /// </summary>
        public void RegisterGroup(string id, Vector3 center, GameObject root)
        {
            _lodGroups[id] = new LODGroup
            {
                id = id,
                center = center,
                root = root,
                currentLOD = 0
            };
        }

        /// <summary>
        /// Unregister a LOD group.
        /// </summary>
        public void UnregisterGroup(string id)
        {
            _lodGroups.Remove(id);
        }

        class LODGroup
        {
            public string id;
            public Vector3 center;
            public GameObject root;
            public int currentLOD;
        }
    }
}
