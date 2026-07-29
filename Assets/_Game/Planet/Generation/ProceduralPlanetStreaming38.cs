using System.Collections.Generic;
using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Streaming system with chunk prioritization.
    /// </summary>
    public sealed class ProceduralPlanetStreaming38 : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] ProceduralPlanetGenerator planetGenerator;
        [SerializeField] Transform player;

        void Start()
        {
            if (planetGenerator == null)
                planetGenerator = FindFirstObjectByType<ProceduralPlanetGenerator>();
            if (player == null)
            {
                var playerBody = FindFirstObjectByType<Player.SphericalGravityBody>();
                player = playerBody?.transform;
            }
        }

        /// <summary>
        /// Calculate chunk priority based on distance and visibility.
        /// </summary>
        public float CalculatePriority(Vector3 chunkCenter, Camera camera)
        {
            if (player == null || camera == null) return 0f;

            float distance = Vector3.Distance(player.position, chunkCenter);
            float distancePriority = 1f / (1f + distance * 0.01f);

            // Check if in frustum
            Plane[] planes = GeometryUtility.CalculateFrustumPlanes(camera);
            Bounds bounds = new(chunkCenter, Vector3.one * 50f);
            bool visible = GeometryUtility.TestPlanesAABB(planes, bounds);

            return visible ? distancePriority : distancePriority * 0.5f;
        }
    }
}
