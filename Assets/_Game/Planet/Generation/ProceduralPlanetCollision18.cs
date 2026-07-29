using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Collision system with terrain sampling.
    /// </summary>
    public sealed class ProceduralPlanetCollision18 : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float sampleRadius = 5f;
        [SerializeField] int sampleCount = 8;

        [Header("References")]
        [SerializeField] PlanetBody planet;

        void Start()
        {
            if (planet == null)
                planet = FindFirstObjectByType<PlanetBody>();
        }

        /// <summary>
        /// Sample terrain height around a position.
        /// </summary>
        public float SampleAverageHeight(Vector3 position)
        {
            if (planet == null) return 0f;

            float totalHeight = 0f;
            int samples = 0;

            for (int i = 0; i < sampleCount; i++)
            {
                float angle = (float)i / sampleCount * Mathf.PI * 2f;
                Vector3 offset = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * sampleRadius;
                Vector3 samplePos = position + offset;

                Vector3 direction = (samplePos - planet.Center).normalized;
                float distance = Vector3.Distance(samplePos, planet.Center);
                float height = distance - planet.Radius;

                totalHeight += height;
                samples++;
            }

            return samples > 0 ? totalHeight / samples : 0f;
        }
    }
}
