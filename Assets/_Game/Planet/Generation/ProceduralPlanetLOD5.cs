using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// LOD system with hysteresis to prevent flickering.
    /// </summary>
    public sealed class ProceduralPlanetLOD5 : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float[] lodDistances = { 100f, 200f, 400f, 800f };
        [SerializeField] float hysteresis = 10f;

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
        /// Get LOD level with hysteresis.
        /// </summary>
        public int GetLODLevel(float distance, int currentLOD)
        {
            // Check if we should switch to a higher LOD (closer)
            for (int i = 0; i < currentLOD; i++)
            {
                if (distance < lodDistances[i] - hysteresis)
                    return i;
            }

            // Check if we should switch to a lower LOD (farther)
            for (int i = currentLOD; i < lodDistances.Length; i++)
            {
                if (distance < lodDistances[i] + hysteresis)
                    return i;
            }

            return lodDistances.Length - 1;
        }
    }
}
