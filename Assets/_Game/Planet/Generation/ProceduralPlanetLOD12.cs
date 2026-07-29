using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// LOD system with distance-based mesh swapping.
    /// </summary>
    public sealed class ProceduralPlanetLOD12 : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float[] lodDistances = { 100f, 200f, 400f, 800f };

        [Header("References")]
        [SerializeField] Camera mainCamera;
        [SerializeField] PlanetBody planet;

        void Start()
        {
            if (mainCamera == null)
                mainCamera = Camera.main;
            if (planet == null)
                planet = FindFirstObjectByType<PlanetBody>();
        }

        /// <summary>
        /// Select LOD based on distance.
        /// </summary>
        public int SelectLOD(float distance)
        {
            for (int i = 0; i < lodDistances.Length; i++)
            {
                if (distance < lodDistances[i])
                    return i;
            }
            return lodDistances.Length - 1;
        }
    }
}
