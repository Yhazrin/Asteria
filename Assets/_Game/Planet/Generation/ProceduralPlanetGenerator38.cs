using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Planet generator with mesh bending.
    /// </summary>
    public sealed class ProceduralPlanetGenerator38 : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float planetRadius = 300f;
        [SerializeField] int seed = 42;
        [SerializeField] float bendAmount = 0.1f;

        [Header("References")]
        [SerializeField] PlanetBody planet;

        void Start()
        {
            if (planet == null)
                planet = FindFirstObjectByType<PlanetBody>();
        }

        /// <summary>
        /// Bend mesh along a curve.
        /// </summary>
        public Mesh BendMesh(Mesh mesh, Vector3 axis)
        {
            if (mesh == null) return null;

            var vertices = mesh.vertices;
            for (int i = 0; i < vertices.Length; i++)
            {
                float t = Vector3.Dot(vertices[i], axis);
                float bend = Mathf.Sin(t * bendAmount) * bendAmount;
                vertices[i] += axis * bend;
            }

            mesh.vertices = vertices;
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();

            return mesh;
        }
    }
}
