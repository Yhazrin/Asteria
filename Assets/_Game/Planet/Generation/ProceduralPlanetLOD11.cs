using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// LOD system with batch updates.
    /// </summary>
    public sealed class ProceduralPlanetLOD11 : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] int batchSize = 10;
        [SerializeField] float updateInterval = 0.5f;

        [Header("References")]
        [SerializeField] Camera mainCamera;
        [SerializeField] PlanetBody planet;

        float _updateTimer;
        int _currentBatch;

        void Start()
        {
            if (mainCamera == null)
                mainCamera = Camera.main;
            if (planet == null)
                planet = FindFirstObjectByType<PlanetBody>();
        }

        void Update()
        {
            _updateTimer -= Time.deltaTime;
            if (_updateTimer <= 0f)
            {
                _updateTimer = updateInterval;
                UpdateBatch();
            }
        }

        void UpdateBatch()
        {
            // Update a batch of objects each frame
            _currentBatch = (_currentBatch + 1) % batchSize;
        }
    }
}
