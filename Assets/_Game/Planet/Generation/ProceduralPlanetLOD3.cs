using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// LOD system with screen-space size calculation.
    /// </summary>
    public sealed class ProceduralPlanetLOD3 : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float screenPercentageThreshold = 0.1f;
        [SerializeField] int maxLODLevels = 4;

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
        /// Get the LOD level based on screen-space size.
        /// </summary>
        public int GetLODLevel(GameObject obj)
        {
            if (mainCamera == null || obj == null) return 0;

            // Calculate screen-space size
            Vector3 center = obj.transform.position;
            float distance = Vector3.Distance(mainCamera.transform.position, center);
            float screenSize = (planet.Radius * 2f) / distance;

            // Map to LOD level
            if (screenSize > screenPercentageThreshold * 4f) return 0;
            if (screenSize > screenPercentageThreshold * 2f) return 1;
            if (screenSize > screenPercentageThreshold) return 2;
            return 3;
        }
    }
}
