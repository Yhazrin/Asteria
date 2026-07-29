using System.Collections.Generic;
using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Alternative LOD system with different approach.
    /// Uses distance-based mesh swapping.
    /// </summary>
    public sealed class ProceduralPlanetLOD2 : MonoBehaviour
    {
        [Header("LOD Settings")]
        [SerializeField] int maxLODLevels = 4;
        [SerializeField] float[] lodDistances = { 100f, 200f, 400f, 800f };
        [SerializeField] int[] lodResolutions = { 128, 64, 32, 16 };

        [Header("References")]
        [SerializeField] Camera mainCamera;
        [SerializeField] PlanetBody planet;

        readonly Dictionary<string, LODGroup2> _lodGroups = new();

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

            foreach (var kvp in _lodGroups)
            {
                var group = kvp.Value;
                float distance = Vector3.Distance(cameraPos, group.center);

                int newLOD = GetLODLevel(distance);
                if (newLOD != group.currentLOD)
                {
                    group.currentLOD = newLOD;
                    UpdateGroupMesh(group);
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

        void UpdateGroupMesh(LODGroup2 group)
        {
            if (group.currentLOD >= lodResolutions.Length) return;

            int resolution = lodResolutions[group.currentLOD];
            // Generate new mesh at this resolution
        }

        public void RegisterGroup(string id, Vector3 center, GameObject root)
        {
            _lodGroups[id] = new LODGroup2
            {
                id = id,
                center = center,
                root = root,
                currentLOD = 0
            };
        }

        public void UnregisterGroup(string id)
        {
            _lodGroups.Remove(id);
        }

        class LODGroup2
        {
            public string id;
            public Vector3 center;
            public GameObject root;
            public int currentLOD;
        }
    }
}
