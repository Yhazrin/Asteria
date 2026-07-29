using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// LOD system with mesh culling.
    /// </summary>
    public sealed class ProceduralPlanetLOD32 : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float cullDistance = 1000f;

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
        /// Check if mesh should be culled.
        /// </summary>
        public bool ShouldCull(Vector3 meshCenter)
        {
            if (mainCamera == null) return false;

            float distance = Vector3.Distance(mainCamera.transform.position, meshCenter);
            return distance > cullDistance;
        }
    }
}
