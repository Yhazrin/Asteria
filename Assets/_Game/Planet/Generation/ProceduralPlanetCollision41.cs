using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Collision system with terrain floating.
    /// </summary>
    public sealed class ProceduralPlanetCollision41 : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float floatAmount = 0.5f;
        [SerializeField] float floatSpeed = 0.2f;

        [Header("References")]
        [SerializeField] PlanetBody planet;

        void Start()
        {
            if (planet == null)
                planet = FindFirstObjectByType<PlanetBody>();
        }

        /// <summary>
        /// Float terrain height.
        /// </summary>
        public float FloatHeight(float height, float time)
        {
            float floating = Mathf.Sin(time * floatSpeed) * floatAmount;
            return height + floating;
        }
    }
}
