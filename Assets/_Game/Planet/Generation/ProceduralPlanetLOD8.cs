using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// LOD system with dynamic quality adjustment.
    /// </summary>
    public sealed class ProceduralPlanetLOD8 : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float targetFPS = 60f;
        [SerializeField] float adjustmentSpeed = 0.1f;
        [SerializeField] int minLOD = 0;
        [SerializeField] int maxLOD = 3;

        [Header("References")]
        [SerializeField] Camera mainCamera;
        [SerializeField] PlanetBody planet;

        float _currentLOD;
        float _fpsAccumulator;
        int _frameCount;

        void Start()
        {
            if (mainCamera == null)
                mainCamera = Camera.main;
            if (planet == null)
                planet = FindFirstObjectByType<PlanetBody>();
        }

        void Update()
        {
            UpdateFPS();
            AdjustLOD();
        }

        void UpdateFPS()
        {
            _fpsAccumulator += 1f / Time.deltaTime;
            _frameCount++;

            if (_frameCount >= 60)
            {
                float avgFPS = _fpsAccumulator / _frameCount;
                _fpsAccumulator = 0;
                _frameCount = 0;

                // Adjust LOD based on FPS
                if (avgFPS < targetFPS * 0.9f)
                {
                    _currentLOD = Mathf.Min(_currentLOD + adjustmentSpeed, maxLOD);
                }
                else if (avgFPS > targetFPS * 1.1f)
                {
                    _currentLOD = Mathf.Max(_currentLOD - adjustmentSpeed, minLOD);
                }
            }
        }

        void AdjustLOD()
        {
            // LOD is adjusted dynamically based on performance
        }

        /// <summary>
        /// Get the current LOD level.
        /// </summary>
        public int GetCurrentLOD()
        {
            return Mathf.RoundToInt(_currentLOD);
        }

        /// <summary>
        /// Force a specific LOD level.
        /// </summary>
        public void ForceLOD(int lod)
        {
            _currentLOD = Mathf.Clamp(lod, minLOD, maxLOD);
        }
    }
}
