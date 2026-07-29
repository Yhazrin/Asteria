using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Collision system with terrain warping.
    /// </summary>
    public sealed class ProceduralPlanetCollision39 : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float warpFrequency = 0.01f;
        [SerializeField] float warpAmplitude = 5f;

        [Header("References")]
        [SerializeField] PlanetBody planet;

        void Start()
        {
            if (planet == null)
                planet = FindFirstObjectByType<PlanetBody>();
        }

        /// <summary>
        /// Warp terrain height.
        /// </summary>
        public float WarpHeight(float height, float x, float z)
        {
            float warp = Mathf.PerlinNoise(x * warpFrequency, z * warpFrequency) * warpAmplitude;
            return height + warp;
        }
    }
}
