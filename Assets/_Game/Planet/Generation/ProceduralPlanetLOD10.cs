using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// LOD system with occlusion culling.
    /// </summary>
    public sealed class ProceduralPlanetLOD10 : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float occlusionDistance = 100f;
        [SerializeField] LayerMask occlusionLayer = ~0;

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
        /// Check if a point is occluded by terrain.
        /// </summary>
        public bool IsOccluded(Vector3 point)
        {
            if (mainCamera == null) return false;

            Vector3 direction = point - mainCamera.transform.position;
            float distance = direction.magnitude;

            if (distance > occlusionDistance) return false;

            Ray ray = new Ray(mainCamera.transform.position, direction.normalized);
            return Physics.Raycast(ray, out RaycastHit hit, distance, occlusionLayer);
        }

        /// <summary>
        /// Get occlusion factor (0 = fully occluded, 1 = fully visible).
        /// </summary>
        public float GetOcclusionFactor(Vector3 point)
        {
            if (mainCamera == null) return 1f;

            Vector3 direction = point - mainCamera.transform.position;
            float distance = direction.magnitude;

            if (distance > occlusionDistance) return 1f;

            Ray ray = new Ray(mainCamera.transform.position, direction.normalized);
            if (Physics.Raycast(ray, out RaycastHit hit, distance, occlusionLayer))
            {
                return hit.distance / distance;
            }

            return 1f;
        }
    }
}
