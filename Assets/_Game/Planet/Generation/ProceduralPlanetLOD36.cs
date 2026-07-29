using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// LOD system with mesh quantization.
    /// </summary>
    public sealed class ProceduralPlanetLOD36 : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float quantizationStep = 0.01f;

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
        /// Quantize mesh vertex positions.
        /// </summary>
        public Mesh QuantizeMesh(Mesh mesh)
        {
            if (mesh == null) return null;

            var vertices = mesh.vertices;
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i].x = Mathf.Round(vertices[i].x / quantizationStep) * quantizationStep;
                vertices[i].y = Mathf.Round(vertices[i].y / quantizationStep) * quantizationStep;
                vertices[i].z = Mathf.Round(vertices[i].z / quantizationStep) * quantizationStep;
            }

            mesh.vertices = vertices;
            mesh.RecalculateBounds();

            return mesh;
        }
    }
}
