using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// LOD system with mesh pooling.
    /// </summary>
    public sealed class ProceduralPlanetLOD19 : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] int poolSize = 10;

        [Header("References")]
        [SerializeField] Camera mainCamera;
        [SerializeField] PlanetBody planet;

        readonly System.Collections.Generic.Queue<Mesh> _meshPool = new();

        void Start()
        {
            if (mainCamera == null)
                mainCamera = Camera.main;
            if (planet == null)
                planet = FindFirstObjectByType<PlanetBody>();
        }

        /// <summary>
        /// Get a mesh from the pool.
        /// </summary>
        public Mesh GetPooledMesh()
        {
            return _meshPool.Count > 0 ? _meshPool.Dequeue() : null;
        }

        /// <summary>
        /// Return a mesh to the pool.
        /// </summary>
        public void ReturnMesh(Mesh mesh)
        {
            if (mesh != null && _meshPool.Count < poolSize)
            {
                _meshPool.Enqueue(mesh);
            }
        }

        /// <summary>
        /// Get pool size.
        /// </summary>
        public int GetPoolSize()
        {
            return _meshPool.Count;
        }
    }
}
