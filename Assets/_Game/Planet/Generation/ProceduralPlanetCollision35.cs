using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Collision system with terrain quantization.
    /// </summary>
    public sealed class ProceduralPlanetCollision35 : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float quantizationStep = 0.5f;

        [Header("References")]
        [SerializeField] PlanetBody planet;

        void Start()
        {
            if (planet == null)
                planet = FindFirstObjectByType<PlanetBody>();
        }

        /// <summary>
        /// Quantize terrain height.
        /// </summary>
        public float QuantizeHeight(float height)
        {
            return Mathf.Round(height / quantizationStep) * quantizationStep;
        }
    }
}
