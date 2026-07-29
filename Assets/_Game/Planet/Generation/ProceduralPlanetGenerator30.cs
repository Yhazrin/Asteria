using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Planet generator with mesh translation.
    /// </summary>
    public sealed class ProceduralPlanetGenerator30 : MonoBehaviour
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
        /// Translate mesh vertices.
        /// </summary>
        public Mesh TranslateMesh(Mesh mesh, Vector3 offset)
        {
            if (mesh == null) return null;

            var vertices = mesh.vertices;
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i] += offset;
            }

            mesh.vertices = vertices;
            mesh.RecalculateBounds();

            return mesh;
        }
    }
}
