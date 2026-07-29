using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// LOD system with performance monitoring.
    /// </summary>
    public sealed class ProceduralPlanetLOD14 : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float targetFrameTime = 16.67f; // 60 FPS

        [Header("References")]
        [SerializeField] Camera mainCamera;
        [SerializeField] PlanetBody planet;

        float _averageFrameTime;
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
            UpdatePerformance();
        }

        void UpdatePerformance()
        {
            _averageFrameTime = (_averageFrameTime * _frameCount + Time.deltaTime) / (_frameCount + 1);
            _frameCount++;

            if (_frameCount > 60)
            {
                _frameCount = 60;
            }
        }

        /// <summary>
        /// Check if performance is acceptable.
        /// </summary>
        public bool IsPerformanceAcceptable()
        {
            return _averageFrameTime < targetFrameTime * 1.2f;
        }

        /// <summary>
        /// Get average frame time.
        /// </summary>
        public float GetAverageFrameTime()
        {
            return _averageFrameTime;
        }
    }
}
