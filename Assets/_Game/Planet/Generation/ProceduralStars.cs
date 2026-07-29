using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Generates procedural starfield for the skybox.
    /// Stars twinkle and rotate slowly.
    /// </summary>
    public sealed class ProceduralStars : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] int starCount = 500;
        [SerializeField] float sphereRadius = 1000f;
        [SerializeField] float twinkleSpeed = 2f;
        [SerializeField] float rotationSpeed = 0.01f;

        [Header("Visual")]
        [SerializeField] Color starColor = new(1f, 0.98f, 0.9f);
        [SerializeField] float minSize = 0.5f;
        [SerializeField] float maxSize = 2f;
        [SerializeField] float minBrightness = 0.3f;
        [SerializeField] float maxBrightness = 1f;

        Mesh _starMesh;
        Material _starMaterial;
        Matrix4x4[] _matrices;
        float[] _twinkleOffsets;

        void Start()
        {
            CreateStarMesh();
            CreateStarMaterial();
            GenerateStars();
        }

        void Update()
        {
            RenderStars();
        }

        void CreateStarMesh()
        {
            // Simple quad for each star
            var verts = new Vector3[]
            {
                new(-0.5f, -0.5f, 0),
                new(0.5f, -0.5f, 0),
                new(0.5f, 0.5f, 0),
                new(-0.5f, 0.5f, 0)
            };

            var tris = new int[] { 0, 1, 2, 0, 2, 3 };
            var uvs = new Vector2[]
            {
                new(0, 0), new(1, 0), new(1, 1), new(0, 1)
            };

            _starMesh = new Mesh { name = "Star" };
            _starMesh.vertices = verts;
            _starMesh.triangles = tris;
            _starMesh.uv = uvs;
            _starMesh.RecalculateNormals();
            _starMesh.RecalculateBounds();
        }

        void CreateStarMaterial()
        {
            var shader = Shader.Find("Universal Render Pipeline/Unlit")
                ?? Shader.Find("Sprites/Default");

            _starMaterial = new Material(shader);
            _starMaterial.color = starColor;
            _starMaterial.renderQueue = 4000; // Behind everything
        }

        void GenerateStars()
        {
            _matrices = new Matrix4x4[starCount];
            _twinkleOffsets = new float[starCount];

            for (int i = 0; i < starCount; i++)
            {
                // Random position on sphere
                Vector3 direction = Random.onUnitSphere;
                Vector3 position = direction * sphereRadius;

                // Random size
                float size = Random.Range(minSize, maxSize);

                // Random rotation
                Quaternion rotation = Quaternion.LookRotation(direction);

                _matrices[i] = Matrix4x4.TRS(position, rotation, Vector3.one * size);
                _twinkleOffsets[i] = Random.Range(0f, Mathf.PI * 2f);
            }
        }

        void RenderStars()
        {
            if (_starMesh == null || _starMaterial == null) return;

            // Rotate entire starfield slowly
            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);

            // Render with GPU instancing
            Graphics.DrawMeshInstanced(_starMesh, 0, _starMaterial, _matrices);
        }

        /// <summary>
        /// Get the brightness of stars at a given time of day.
        /// </summary>
        public float GetStarBrightness(float timeOfDay)
        {
            // Stars are brightest at night (0.0 = midnight, 0.5 = noon)
            float nightFactor = 1f - Mathf.Abs(timeOfDay - 0.5f) * 2f;
            return Mathf.Pow(Mathf.Clamp01(nightFactor), 2f);
        }

        /// <summary>
        /// Update star visibility based on time of day.
        /// </summary>
        public void UpdateForTimeOfDay(float timeOfDay)
        {
            float brightness = GetStarBrightness(timeOfDay);

            if (_starMaterial != null)
            {
                Color c = starColor;
                c.a = brightness;
                _starMaterial.color = c;
            }
        }
    }
}
