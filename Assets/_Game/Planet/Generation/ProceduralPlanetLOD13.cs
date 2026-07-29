using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// LOD system with quality settings.
    /// </summary>
    public sealed class ProceduralPlanetLOD13 : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float qualityMultiplier = 1f;

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
        /// Get LOD level adjusted by quality settings.
        /// </summary>
        public int GetAdjustedLOD(int baseLOD)
        {
            int adjusted = Mathf.RoundToInt(baseLOD * qualityMultiplier);
            return Mathf.Clamp(adjusted, 0, 3);
        }

        /// <summary>
        /// Set quality multiplier.
        /// </summary>
        public void SetQualityMultiplier(float multiplier)
        {
            qualityMultiplier = Mathf.Clamp(multiplier, 0.5f, 2f);
        }
    }
}
