using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// LOD system with camera-relative sizing.
    /// </summary>
    public sealed class ProceduralPlanetLOD7 : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float[] lodScreenSizes = { 0.5f, 0.25f, 0.1f, 0.05f };

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
        /// Get LOD level based on screen-space size.
        /// </summary>
        public int GetLODLevel()
        {
            if (mainCamera == null || planet == null) return 0;

            float distance = Vector3.Distance(mainCamera.transform.position, planet.transform.position);
            float screenSize = (planet.Radius * 2f) / distance;

            for (int i = 0; i < lodScreenSizes.Length; i++)
            {
                if (screenSize > lodScreenSizes[i])
                    return i;
            }

            return lodScreenSizes.Length - 1;
        }

        /// <summary>
        /// Get screen-space size of the planet.
        /// </summary>
        public float GetScreenSize()
        {
            if (mainCamera == null || planet == null) return 0f;

            float distance = Vector3.Distance(mainCamera.transform.position, planet.transform.position);
            return (planet.Radius * 2f) / distance;
        }
    }
}
