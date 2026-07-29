using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Planet generator with mesh flipping.
    /// </summary>
    public sealed class ProceduralPlanetGenerator26 : MonoBehaviour
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
        /// Flip mesh normals.
        /// </summary>
        public Mesh FlipNormals(Mesh mesh)
        {
            if (mesh == null) return null;

            var normals = mesh.normals;
            for (int i = 0; i < normals.Length; i++)
            {
                normals[i] = -normals[i];
            }

            mesh.normals = normals;
            return mesh;
        }
    }
}
