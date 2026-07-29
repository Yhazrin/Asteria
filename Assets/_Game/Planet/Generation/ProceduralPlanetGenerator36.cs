using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Planet generator with mesh flattening.
    /// </summary>
    public sealed class ProceduralPlanetGenerator36 : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float planetRadius = 300f;
        [SerializeField] int seed = 42;
        [SerializeField] float flattenFactor = 0.5f;

        [Header("References")]
        [SerializeField] PlanetBody planet;

        void Start()
        {
            if (planet == null)
                planet = FindFirstObjectByType<PlanetBody>();
        }

        /// <summary>
        /// Flatten mesh along an axis.
        /// </summary>
        public Mesh FlattenMesh(Mesh mesh, Vector3 axis)
        {
            if (mesh == null) return null;

            var vertices = mesh.vertices;
            for (int i = 0; i < vertices.Length; i++)
            {
                float dot = Vector3.Dot(vertices[i], axis);
                vertices[i] -= axis * dot * flattenFactor;
            }

            mesh.vertices = vertices;
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();

            return mesh;
        }
    }
}
