using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// LOD system with mesh bounds.
    /// </summary>
    public sealed class ProceduralPlanetLOD29 : MonoBehaviour
    {
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
        /// Get mesh bounds in world space.
        /// </summary>
        public Bounds GetWorldBounds(Mesh mesh, Transform transform)
        {
            if (mesh == null || transform == null) return new Bounds();

            Bounds localBounds = mesh.bounds;
            Vector3 center = transform.TransformPoint(localBounds.center);
            Vector3 size = transform.TransformVector(localBounds.size);

            return new Bounds(center, size);
        }
    }
}
