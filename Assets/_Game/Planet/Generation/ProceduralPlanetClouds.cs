using System.Collections.Generic;
using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Cloud system for procedural planets.
    /// Handles cloud generation and animation.
    /// </summary>
    public sealed class ProceduralPlanetClouds : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] int cloudCount = 20;
        [SerializeField] float cloudAltitude = 50f;
        [SerializeField] float cloudScale = 20f;
        [SerializeField] float moveSpeed = 2f;

        [Header("Visual")]
        [SerializeField] Color cloudColor = new(0.95f, 0.95f, 0.98f, 0.6f);

        [Header("References")]
        [SerializeField] PlanetBody planet;

        readonly List<CloudInstance> _clouds = new();
        Material _cloudMaterial;

        void Start()
        {
            if (planet == null)
                planet = FindFirstObjectByType<PlanetBody>();

            CreateCloudMaterial();
            GenerateClouds();
        }

        void Update()
        {
            MoveClouds();
        }

        void CreateCloudMaterial()
        {
            var shader = Shader.Find("Universal Render Pipeline/Unlit")
                ?? Shader.Find("Sprites/Default");

            _cloudMaterial = new Material(shader);
            _cloudMaterial.color = cloudColor;
            _cloudMaterial.renderQueue = 3000;
        }

        void GenerateClouds()
        {
            for (int i = 0; i < cloudCount; i++)
            {
                Vector3 direction = Random.onUnitSphere;
                Vector3 position = direction * (planet.Radius + cloudAltitude);

                var go = new GameObject($"Cloud_{i}");
                go.transform.SetParent(transform, false);
                go.transform.position = position;
                go.transform.rotation = Quaternion.LookRotation(
                    Vector3.Cross(direction, Vector3.up).normalized, direction);

                var filter = go.AddComponent<MeshFilter>();
                filter.mesh = CreateCloudMesh();

                var renderer = go.AddComponent<MeshRenderer>();
                renderer.material = _cloudMaterial;

                _clouds.Add(new CloudInstance
                {
                    root = go,
                    direction = direction,
                    speed = Random.Range(0.5f, 1.5f) * moveSpeed
                });
            }
        }

        Mesh CreateCloudMesh()
        {
            var combined = new System.Collections.Generic.List<Vector3>();
            var combinedTris = new System.Collections.Generic.List<int>();

            int blobCount = Random.Range(3, 6);
            int vertexOffset = 0;

            for (int i = 0; i < blobCount; i++)
            {
                Vector3 offset = new Vector3(
                    Random.Range(-0.5f, 0.5f),
                    Random.Range(-0.1f, 0.1f),
                    Random.Range(-0.3f, 0.3f)) * cloudScale;

                float radius = Random.Range(0.3f, 0.6f) * cloudScale;

                var sphere = CreateSphere(1, radius);
                var verts = sphere.vertices;
                var tris = sphere.triangles;

                for (int v = 0; v < verts.Length; v++)
                {
                    combined.Add(verts[v] + offset);
                }

                for (int t = 0; t < tris.Length; t++)
                {
                    combinedTris.Add(tris[t] + vertexOffset);
                }

                vertexOffset += verts.Length;
            }

            var mesh = new Mesh { name = "Cloud" };
            mesh.SetVertices(combined);
            mesh.SetTriangles(combinedTris, 0);
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            return mesh;
        }

        Mesh CreateSphere(int subdivisions, float radius)
        {
            float t = (1f + Mathf.Sqrt(5f)) / 2f;
            var verts = new System.Collections.Generic.List<Vector3>
            {
                new(-1, t, 0).normalized * radius,
                new(1, t, 0).normalized * radius,
                new(-1, -t, 0).normalized * radius,
                new(1, -t, 0).normalized * radius,
                new(0, -1, t).normalized * radius,
                new(0, 1, t).normalized * radius,
                new(0, -1, -t).normalized * radius,
                new(0, 1, -t).normalized * radius,
                new(t, 0, -1).normalized * radius,
                new(t, 0, 1).normalized * radius,
                new(-t, 0, -1).normalized * radius,
                new(-t, 0, 1).normalized * radius,
            };

            var tris = new System.Collections.Generic.List<int>
            {
                0,11,5, 0,5,1, 0,1,7, 0,7,10, 0,10,11,
                1,5,9, 5,11,4, 11,10,2, 10,7,6, 7,1,8,
                3,9,4, 3,4,2, 3,2,6, 3,6,8, 3,8,9,
                4,9,5, 2,4,11, 6,2,10, 8,6,7, 9,8,1,
            };

            var mesh = new Mesh();
            mesh.SetVertices(verts);
            mesh.SetTriangles(tris, 0);
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            return mesh;
        }

        void MoveClouds()
        {
            foreach (var cloud in _clouds)
            {
                if (cloud.root == null) continue;

                cloud.root.transform.RotateAround(
                    planet.Center,
                    planet.GetSurfaceUp(cloud.root.transform.position),
                    cloud.speed * Time.deltaTime);
            }
        }

        void OnDestroy()
        {
            foreach (var cloud in _clouds)
            {
                if (cloud.root != null) Destroy(cloud.root);
            }
        }

        class CloudInstance
        {
            public GameObject root;
            public Vector3 direction;
            public float speed;
        }
    }
}
