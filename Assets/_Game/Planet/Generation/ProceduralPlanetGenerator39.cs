using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Planet generator with mesh twisting.
    /// </summary>
    public sealed class ProceduralPlanetGenerator39 : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float planetRadius = 300f;
        [SerializeField] int seed = 42;
        [SerializeField] float twistAmount = 0.1f;

        [Header("References")]
        [SerializeField] PlanetBody planet;

        void Start()
        {
            if (planet == null)
                planet = FindFirstObjectByType<PlanetBody>();
        }

        /// <summary>
        /// Twist mesh around an axis.
        /// </summary>
        public Mesh TwistMesh(Mesh mesh, Vector3 axis)
        {
            if (mesh == null) return null;

            var vertices = mesh.vertices;
            for (int i = 0; i < vertices.Length; i++)
            {
                float t = Vector3.Dot(vertices[i], axis);
                float angle = t * twistAmount;
                Quaternion rotation = Quaternion.AngleAxis(angle * Mathf.Rad2Deg, axis);
                vertices[i] = rotation * vertices[i];
            }

            mesh.vertices = vertices;
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();

            return mesh;
        }
    }
}
