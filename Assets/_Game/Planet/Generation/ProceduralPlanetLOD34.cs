using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// LOD system with mesh mirroring.
    /// </summary>
    public sealed class ProceduralPlanetLOD34 : MonoBehaviour
    {
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
        /// Mirror mesh along an axis.
        /// </summary>
        public Mesh MirrorMesh(Mesh mesh, Vector3 axis)
        {
            if (mesh == null) return null;

            var vertices = mesh.vertices;
            for (int i = 0; i < vertices.Length; i++)
            {
                if (axis.x > 0) vertices[i].x = -vertices[i].x;
                if (axis.y > 0) vertices[i].y = -vertices[i].y;
                if (axis.z > 0) vertices[i].z = -vertices[i].z;
            }

            mesh.vertices = vertices;
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();

            return mesh;
        }
    }
}
