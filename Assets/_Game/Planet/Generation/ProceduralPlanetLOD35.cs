using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// LOD system with mesh welding.
    /// </summary>
    public sealed class ProceduralPlanetLOD35 : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float weldThreshold = 0.001f;

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
        /// Weld nearby vertices.
        /// </summary>
        public Mesh WeldVertices(Mesh mesh)
        {
            if (mesh == null) return null;

            var vertices = mesh.vertices;
            var welded = new System.Collections.Generic.List<Vector3>();
            var indexMap = new int[vertices.Length];

            for (int i = 0; i < vertices.Length; i++)
            {
                int existingIndex = -1;
                for (int j = 0; j < welded.Count; j++)
                {
                    if (Vector3.Distance(vertices[i], welded[j]) < weldThreshold)
                    {
                        existingIndex = j;
                        break;
                    }
                }

                if (existingIndex >= 0)
                {
                    indexMap[i] = existingIndex;
                }
                else
                {
                    indexMap[i] = welded.Count;
                    welded.Add(vertices[i]);
                }
            }

            var triangles = mesh.triangles;
            for (int i = 0; i < triangles.Length; i++)
            {
                triangles[i] = indexMap[triangles[i]];
            }

            var newMesh = new Mesh();
            newMesh.vertices = welded.ToArray();
            newMesh.triangles = triangles;
            newMesh.RecalculateNormals();
            newMesh.RecalculateBounds();

            return newMesh;
        }
    }
}
