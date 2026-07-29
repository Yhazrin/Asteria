using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Collision system with terrain interpolation.
    /// </summary>
    public sealed class ProceduralPlanetCollision25 : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] PlanetBody planet;

        void Start()
        {
            if (planet == null)
                planet = FindFirstObjectByType<PlanetBody>();
        }

        /// <summary>
        /// Interpolate height between points.
        /// </summary>
        public float InterpolateHeight(Vector3 pos1, float height1, Vector3 pos2, float height2, Vector3 target)
        {
            float dist1 = Vector3.Distance(target, pos1);
            float dist2 = Vector3.Distance(target, pos2);
            float total = dist1 + dist2;

            if (total < 0.001f) return height1;

            float t = dist1 / total;
            return Mathf.Lerp(height1, height2, t);
        }
    }
}
