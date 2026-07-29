using System.Collections.Generic;
using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Streaming system with chunk filtering.
    /// </summary>
    public sealed class ProceduralPlanetStreaming25 : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float filterDistance = 1000f;

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
        /// Filter chunks by distance.
        /// </summary>
        public List<string> FilterByDistance(List<string> chunks, Dictionary<string, Vector3> positions)
        {
            if (player == null) return chunks;

            var filtered = new List<string>();
            foreach (var chunk in chunks)
            {
                if (positions.TryGetValue(chunk, out var pos))
                {
                    float distance = Vector3.Distance(player.position, pos);
                    if (distance < filterDistance)
                    {
                        filtered.Add(chunk);
                    }
                }
            }

            return filtered;
        }
    }
}
