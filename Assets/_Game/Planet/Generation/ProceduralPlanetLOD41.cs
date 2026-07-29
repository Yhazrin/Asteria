using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// LOD system with mesh breathing.
    /// </summary>
    public sealed class ProceduralPlanetLOD41 : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float breatheAmount = 0.05f;
        [SerializeField] float breatheSpeed = 0.3f;

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
        /// Apply breathing effect to mesh.
        /// </summary>
        public Mesh ApplyBreathing(Mesh mesh, float time)
        {
            if (mesh == null) return null;

            var vertices = mesh.vertices;
            float breathe = Mathf.Sin(time * breatheSpeed) * breatheAmount;

            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i] = vertices[i].normalized * (vertices[i].magnitude + breathe);
            }

            mesh.vertices = vertices;
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();

            return mesh;
        }
    }
}
