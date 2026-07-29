using System.Collections.Generic;
using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Manager with planet transformation.
    /// </summary>
    public sealed class ProceduralPlanetManager27 : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] Transform player;

        void Start()
        {
            if (player == null)
            {
                var playerBody = FindFirstObjectByType<Player.SphericalGravityBody>();
                player = playerBody?.transform;
            }
        }

        /// <summary>
        /// Transform planet mesh.
        /// </summary>
        public Mesh TransformMesh(Mesh mesh, Vector3 scale, Quaternion rotation, Vector3 translation)
        {
            if (mesh == null) return null;

            var vertices = mesh.vertices;
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i] = rotation * Vector3.Scale(vertices[i], scale) + translation;
            }

            mesh.vertices = vertices;
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();

            return mesh;
        }
    }
}
