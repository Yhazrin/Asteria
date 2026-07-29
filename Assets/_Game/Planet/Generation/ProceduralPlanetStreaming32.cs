using System.Collections.Generic;
using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Streaming system with chunk visibility.
    /// </summary>
    public sealed class ProceduralPlanetStreaming32 : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] ProceduralPlanetGenerator planetGenerator;
        [SerializeField] Camera mainCamera;

        void Start()
        {
            if (planetGenerator == null)
                planetGenerator = FindFirstObjectByType<ProceduralPlanetGenerator>();
            if (mainCamera == null)
                mainCamera = Camera.main;
        }

        /// <summary>
        /// Check if chunk is visible from camera.
        /// </summary>
        public bool IsChunkVisible(Vector3 chunkCenter, float chunkRadius)
        {
            if (mainCamera == null) return true;

            Plane[] planes = GeometryUtility.CalculateFrustumPlanes(mainCamera);
            Bounds bounds = new(chunkCenter, Vector3.one * chunkRadius * 2f);

            return GeometryUtility.TestPlanesAABB(planes, bounds);
        }
    }
}
