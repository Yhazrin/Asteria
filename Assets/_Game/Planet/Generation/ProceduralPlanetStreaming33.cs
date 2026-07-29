using System.Collections.Generic;
using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Streaming system with chunk distance sorting.
    /// </summary>
    public sealed class ProceduralPlanetStreaming33 : MonoBehaviour
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
        /// Sort chunks by distance from player.
        /// </summary>
        public List<string> SortByDistance(List<string> chunks, Dictionary<string, Vector3> positions)
        {
            if (player == null) return chunks;

            chunks.Sort((a, b) =>
            {
                float distA = positions.TryGetValue(a, out var posA) ? Vector3.Distance(player.position, posA) : float.MaxValue;
                float distB = positions.TryGetValue(b, out var posB) ? Vector3.Distance(player.position, posB) : float.MaxValue;
                return distA.CompareTo(distB);
            });

            return chunks;
        }
    }
}
