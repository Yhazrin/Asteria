using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Streaming system with priority-based loading.
    /// </summary>
    public sealed class ProceduralPlanetStreaming4 : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float loadDistance = 500f;
        [SerializeField] float unloadDistance = 800f;
        [SerializeField] int maxLoadsPerFrame = 2;

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
        /// Get loading priority based on distance.
        /// </summary>
        public int GetLoadPriority(Vector3 chunkPosition)
        {
            if (player == null) return 0;

            float distance = Vector3.Distance(player.position, chunkPosition);

            if (distance < loadDistance * 0.5f) return 0; // Highest priority
            if (distance < loadDistance) return 1;
            if (distance < unloadDistance) return 2;
            return 3; // Lowest priority
        }
    }
}
