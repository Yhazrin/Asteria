using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Planet generator with mesh centering.
    /// </summary>
    public sealed class ProceduralPlanetGenerator27 : MonoBehaviour
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
        /// Center mesh at origin.
        /// </summary>
        public Mesh CenterMesh(Mesh mesh)
        {
            if (mesh == null) return null;

            var vertices = mesh.vertices;
            Vector3 center = Vector3.zero;

            foreach (var v in vertices)
            {
                center += v;
            }

            center /= vertices.Length;

            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i] -= center;
            }

            mesh.vertices = vertices;
            mesh.RecalculateBounds();

            return mesh;
        }
    }
}
