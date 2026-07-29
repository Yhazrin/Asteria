using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// LOD system with distance-based quality adjustment.
    /// </summary>
    public sealed class ProceduralPlanetLOD17 : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float highQualityDistance = 100f;
        [SerializeField] float mediumQualityDistance = 300f;
        [SerializeField] float lowQualityDistance = 600f;

        [Header("References")]
        [SerializeField] Camera mainCamera;
        [SerializeField] PlanetBody planet;

        void Start()
        {
            if (mainCamera == null)
                mainCamera = Camera.main;
            if (planet == null)
                planet = FindFirstObjectByType<PlanetBody>();
        }

        /// <summary>
        /// Get quality level based on distance.
        /// </summary>
        public int GetQualityLevel(float distance)
        {
            if (distance < highQualityDistance) return 0; // High
            if (distance < mediumQualityDistance) return 1; // Medium
            if (distance < lowQualityDistance) return 2; // Low
            return 3; // Very Low
        }
    }
}
