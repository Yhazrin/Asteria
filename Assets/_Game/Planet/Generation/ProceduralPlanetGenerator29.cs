using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Planet generator with mesh bounds calculation.
    /// </summary>
    public sealed class ProceduralPlanetGenerator29 : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float planetRadius = 300f;
        [SerializeField] int seed = 42;

        [Header("References")]
        [SerializeField] PlanetBody planet;

        void Start()
        {
            if (planet == null)
                planet = FindFirstObjectByType<PlanetBody>();
        }

        /// <summary>
        /// Calculate mesh bounds.
        /// </summary>
        public Bounds CalculateBounds(Mesh mesh)
        {
            if (mesh == null) return new Bounds();

            var vertices = mesh.vertices;
            if (vertices.Length == 0) return new Bounds();

            Vector3 min = vertices[0];
            Vector3 max = vertices[0];

            foreach (var v in vertices)
            {
                min = Vector3.Min(min, v);
                max = Vector3.Max(max, v);
            }

            Vector3 center = (min + max) * 0.5f;
            Vector3 size = max - min;

            return new Bounds(center, size);
        }
    }
}
