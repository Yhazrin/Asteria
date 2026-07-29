using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Collision system with terrain generation.
    /// </summary>
    public sealed class ProceduralPlanetCollision30 : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float noiseScale = 0.01f;
        [SerializeField] float noiseAmplitude = 10f;

        [Header("References")]
        [SerializeField] PlanetBody planet;

        void Start()
        {
            if (planet == null)
                planet = FindFirstObjectByType<PlanetBody>();
        }

        /// <summary>
        /// Generate terrain height at position.
        /// </summary>
        public float GenerateHeight(Vector3 position)
        {
            float nx = position.x * noiseScale;
            float nz = position.z * noiseScale;

            return Mathf.PerlinNoise(nx, nz) * noiseAmplitude;
        }
    }
}
