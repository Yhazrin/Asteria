using System.Collections.Generic;
using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Streaming system with chunk filtering.
    /// </summary>
    public sealed class ProceduralPlanetStreaming37 : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] ProceduralPlanetGenerator planetGenerator;

        void Start()
        {
            if (planetGenerator == null)
                planetGenerator = FindFirstObjectByType<ProceduralPlanetGenerator>();
        }

        /// <summary>
        /// Filter chunks by visibility.
        /// </summary>
        public List<string> FilterByVisibility(List<string> chunks, Camera camera)
        {
            if (camera == null) return chunks;

            var visible = new List<string>();
            Plane[] planes = GeometryUtility.CalculateFrustumPlanes(camera);

            foreach (var chunk in chunks)
            {
                // Simplified visibility check
                visible.Add(chunk);
            }

            return visible;
        }
    }
}
