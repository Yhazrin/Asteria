using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// LOD system with mesh warping.
    /// </summary>
    public sealed class ProceduralPlanetLOD40 : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float warpFrequency = 0.01f;
        [SerializeField] float warpAmplitude = 0.1f;

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
        /// Warp mesh vertices.
        /// </summary>
        public Mesh WarpMesh(Mesh mesh, float time)
        {
            if (mesh == null) return null;

            var vertices = mesh.vertices;
            for (int i = 0; i < vertices.Length; i++)
            {
                float warpX = Mathf.Sin(vertices[i].y * warpFrequency + time) * warpAmplitude;
                float warpZ = Mathf.Cos(vertices[i].x * warpFrequency + time) * warpAmplitude;
                vertices[i].x += warpX;
                vertices[i].z += warpZ;
            }

            mesh.vertices = vertices;
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();

            return mesh;
        }
    }
}
