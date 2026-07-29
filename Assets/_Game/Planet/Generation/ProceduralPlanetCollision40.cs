using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Collision system with terrain oscillation.
    /// </summary>
    public sealed class ProceduralPlanetCollision40 : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float oscillationFrequency = 0.1f;
        [SerializeField] float oscillationAmplitude = 2f;

        [Header("References")]
        [SerializeField] PlanetBody planet;

        void Start()
        {
            if (planet == null)
                planet = FindFirstObjectByType<PlanetBody>();
        }

        /// <summary>
        /// Oscillate terrain height.
        /// </summary>
        public float OscillateHeight(float height, float time)
        {
            float oscillation = Mathf.Sin(time * oscillationFrequency) * oscillationAmplitude;
            return height + oscillation;
        }
    }
}
