using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Planet generator with mesh rippling.
    /// </summary>
    public sealed class ProceduralPlanetGenerator41 : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float planetRadius = 300f;
        [SerializeField] int seed = 42;
        [SerializeField] float rippleAmount = 0.1f;
        [SerializeField] float rippleFrequency = 0.5f;

        [Header("References")]
        [SerializeField] PlanetBody planet;

        void Start()
        {
            if (planet == null)
                planet = FindFirstObjectByType<PlanetBody>();
        }

        /// <summary>
        /// Apply ripple effect to mesh.
        /// </summary>
        public Mesh ApplyRipple(Mesh mesh, float time)
        {
            if (mesh == null) return null;

            var vertices = mesh.vertices;
            for (int i = 0; i < vertices.Length; i++)
            {
                float ripple = Mathf.Sin(vertices[i].x * rippleFrequency + time) *
                              Mathf.Cos(vertices[i].z * rippleFrequency + time) * rippleAmount;
                vertices[i].y += ripple;
            }

            mesh.vertices = vertices;
            mesh.RecalculateNormals();

            return mesh;
        }
    }
}
