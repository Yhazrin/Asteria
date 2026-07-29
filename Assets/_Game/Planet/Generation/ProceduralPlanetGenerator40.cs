using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Planet generator with mesh pulsing.
    /// </summary>
    public sealed class ProceduralPlanetGenerator40 : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float planetRadius = 300f;
        [SerializeField] int seed = 42;
        [SerializeField] float pulseAmount = 0.1f;
        [SerializeField] float pulseFrequency = 0.5f;

        [Header("References")]
        [SerializeField] PlanetBody planet;

        void Start()
        {
            if (planet == null)
                planet = FindFirstObjectByType<PlanetBody>();
        }

        /// <summary>
        /// Pulse mesh vertices.
        /// </summary>
        public Mesh PulseMesh(Mesh mesh, float time)
        {
            if (mesh == null) return null;

            var vertices = mesh.vertices;
            float pulse = Mathf.Sin(time * pulseFrequency) * pulseAmount;

            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i] = vertices[i].normalized * (vertices[i].magnitude + pulse);
            }

            mesh.vertices = vertices;
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();

            return mesh;
        }
    }
}
