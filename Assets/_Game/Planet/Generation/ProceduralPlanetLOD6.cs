using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// LOD system with smooth transitions.
    /// </summary>
    public sealed class ProceduralPlanetLOD6 : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float[] lodDistances = { 100f, 200f, 400f, 800f };
        [SerializeField] float transitionSpeed = 2f;

        [Header("References")]
        [SerializeField] Camera mainCamera;
        [SerializeField] PlanetBody planet;

        float _currentLOD;
        float _targetLOD;

        void Start()
        {
            if (mainCamera == null)
                mainCamera = Camera.main;
            if (planet == null)
                planet = FindFirstObjectByType<PlanetBody>();
        }

        void Update()
        {
            UpdateLOD();
        }

        void UpdateLOD()
        {
            if (mainCamera == null || planet == null) return;

            float distance = Vector3.Distance(mainCamera.transform.position, planet.transform.position);
            _targetLOD = GetLODLevel(distance);
            _currentLOD = Mathf.Lerp(_currentLOD, _targetLOD, Time.deltaTime * transitionSpeed);
        }

        int GetLODLevel(float distance)
        {
            for (int i = 0; i < lodDistances.Length; i++)
            {
                if (distance < lodDistances[i])
                    return i;
            }
            return lodDistances.Length - 1;
        }

        /// <summary>
        /// Get the current LOD level (smooth).
        /// </summary>
        public float GetCurrentLOD()
        {
            return _currentLOD;
        }

        /// <summary>
        /// Get the target LOD level.
        /// </summary>
        public int GetTargetLOD()
        {
            return Mathf.RoundToInt(_targetLOD);
        }
    }
}
