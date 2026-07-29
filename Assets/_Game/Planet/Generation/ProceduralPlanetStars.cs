using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Star system for procedural planets.
    /// Handles starfield rendering and effects.
    /// </summary>
    public sealed class ProceduralPlanetStars : MonoBehaviour
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

        [Header("References")]
        [SerializeField] ProceduralLighting lighting;

        Mesh _starMesh;
        Material _starMaterial;
        Matrix4x4[] _matrices;
        float[] _twinkleOffsets;

        void Start()
        {
            if (lighting == null)
                lighting = FindFirstObjectByType<ProceduralLighting>();

            CreateStarMesh();
            CreateStarMaterial();
            GenerateStars();
        }

        void Update()
        {
            UpdateStars();
        }

        void CreateStarMesh()
        {
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
            _starMaterial.renderQueue = 4000;
        }

        void GenerateStars()
        {
            _matrices = new Matrix4x4[starCount];
            _twinkleOffsets = new float[starCount];

            for (int i = 0; i < starCount; i++)
            {
                Vector3 direction = Random.onUnitSphere;
                Vector3 position = direction * sphereRadius;
                float size = Random.Range(minSize, maxSize);
                Quaternion rotation = Quaternion.LookRotation(direction);

                _matrices[i] = Matrix4x4.TRS(position, rotation, Vector3.one * size);
                _twinkleOffsets[i] = Random.Range(0f, Mathf.PI * 2f);
            }
        }

        void UpdateStars()
        {
            if (_starMesh == null || _starMaterial == null) return;

            // Rotate starfield
            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);

            // Update brightness based on time
            if (lighting != null)
            {
                float brightness = GetStarBrightness(lighting.GetTimeOfDay());
                Color c = starColor;
                c.a = brightness;
                _starMaterial.color = c;
            }

            // Render
            Graphics.DrawMeshInstanced(_starMesh, 0, _starMaterial, _matrices);
        }

        float GetStarBrightness(float timeOfDay)
        {
            float nightFactor = 1f - Mathf.Abs(timeOfDay - 0.5f) * 2f;
            return Mathf.Pow(Mathf.Clamp01(nightFactor), 2f);
        }
    }
}
