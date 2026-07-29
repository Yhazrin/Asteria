using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Generates and manages water bodies on the planet surface.
    /// Supports oceans, lakes, and rivers with wave animation.
    /// </summary>
    public sealed class WaterSystem : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float waterLevel = 0.4f; // Normalized height (0-1)
        [SerializeField] int resolution = 64;
        [SerializeField] float waveSpeed = 1f;
        [SerializeField] float waveHeight = 0.5f;
        [SerializeField] float waveFrequency = 0.1f;

        [Header("Visual")]
        [SerializeField] Color shallowColor = new(0.3f, 0.6f, 0.8f, 0.6f);
        [SerializeField] Color deepColor = new(0.1f, 0.2f, 0.4f, 0.8f);
        [SerializeField] float foamThreshold = 0.7f;

        [Header("References")]
        [SerializeField] PlanetBody planet;
        [SerializeField] SphericalTerrainGenerator terrainGenerator;

        Mesh _waterMesh;
        Material _waterMaterial;
        Vector3[] _baseVertices;
        Vector3[] _animatedVertices;

        void Start()
        {
            if (planet == null)
                planet = FindFirstObjectByType<PlanetBody>();

            CreateWaterMaterial();
            GenerateWaterMesh();
        }

        void Update()
        {
            AnimateWaves();
        }

        void CreateWaterMaterial()
        {
            var shader = Shader.Find("Universal Render Pipeline/Lit")
                ?? Shader.Find("Universal Render Pipeline/Simple Lit")
                ?? Shader.Find("Standard");

            _waterMaterial = new Material(shader);
            _waterMaterial.color = shallowColor;
            _waterMaterial.renderQueue = 3000; // Transparent

            if (_waterMaterial.HasProperty("_Smoothness"))
                _waterMaterial.SetFloat("_Smoothness", 0.8f);
            if (_waterMaterial.HasProperty("_Metallic"))
                _waterMaterial.SetFloat("_Metallic", 0.1f);
        }

        void GenerateWaterMesh()
        {
            int vertCount = (resolution + 1) * (resolution + 1);
            _baseVertices = new Vector3[vertCount];
            _animatedVertices = new Vector3[vertCount];
            var normals = new Vector3[vertCount];
            var uvs = new Vector2[vertCount];

            // Generate sphere vertices at water level
            for (int lat = 0; lat <= resolution; lat++)
            {
                float phi = (float)lat / resolution * Mathf.PI;
                for (int lon = 0; lon <= resolution; lon++)
                {
                    float theta = (float)lon / resolution * Mathf.PI * 2f;

                    int idx = lat * (resolution + 1) + lon;

                    float x = Mathf.Sin(phi) * Mathf.Cos(theta);
                    float y = Mathf.Cos(phi);
                    float z = Mathf.Sin(phi) * Mathf.Sin(theta);

                    Vector3 dir = new Vector3(x, y, z).normalized;
                    float radius = planet.Radius * waterLevel;
                    _baseVertices[idx] = dir * radius;
                    _animatedVertices[idx] = _baseVertices[idx];
                    normals[idx] = dir;
                    uvs[idx] = new Vector2((float)lon / resolution, (float)lat / resolution);
                }
            }

            // Generate triangles
            int triCount = resolution * resolution * 6;
            var triangles = new int[triCount];
            int t = 0;

            for (int lat = 0; lat < resolution; lat++)
            {
                for (int lon = 0; lon < resolution; lon++)
                {
                    int current = lat * (resolution + 1) + lon;
                    int next = current + resolution + 1;

                    triangles[t++] = current;
                    triangles[t++] = next;
                    triangles[t++] = current + 1;

                    triangles[t++] = current + 1;
                    triangles[t++] = next;
                    triangles[t++] = next + 1;
                }
            }

            _waterMesh = new Mesh { name = "Water" };
            _waterMesh.vertices = _animatedVertices;
            _waterMesh.normals = normals;
            _waterMesh.uv = uvs;
            _waterMesh.triangles = triangles;
            _waterMesh.RecalculateBounds();

            var go = new GameObject("WaterSurface");
            go.transform.SetParent(transform, false);

            var filter = go.AddComponent<MeshFilter>();
            filter.mesh = _waterMesh;

            var renderer = go.AddComponent<MeshRenderer>();
            renderer.material = _waterMaterial;
        }

        void AnimateWaves()
        {
            if (_waterMesh == null || _baseVertices == null) return;

            float time = Time.time * waveSpeed;

            for (int i = 0; i < _baseVertices.Length; i++)
            {
                Vector3 basePos = _baseVertices[i];
                Vector3 dir = basePos.normalized;

                // Wave displacement along surface normal
                float wave = Mathf.Sin(
                    basePos.x * waveFrequency + time) *
                    Mathf.Sin(basePos.z * waveFrequency + time * 0.7f) *
                    waveHeight;

                _animatedVertices[i] = basePos + dir * wave;
            }

            _waterMesh.vertices = _animatedVertices;
            _waterMesh.RecalculateNormals();
        }

        /// <summary>
        /// Check if a position is underwater.
        /// </summary>
        public bool IsUnderwater(Vector3 position)
        {
            float distance = (position - planet.Center).magnitude;
            return distance < planet.Radius * waterLevel;
        }

        /// <summary>
        /// Get the water depth at a position.
        /// </summary>
        public float GetWaterDepth(Vector3 position)
        {
            float distance = (position - planet.Center).magnitude;
            float waterRadius = planet.Radius * waterLevel;
            return Mathf.Max(0, waterRadius - distance);
        }

        /// <summary>
        /// Get the wave height at a position.
        /// </summary>
        public float GetWaveHeight(Vector3 position)
        {
            float time = Time.time * waveSpeed;
            return Mathf.Sin(
                position.x * waveFrequency + time) *
                Mathf.Sin(position.z * waveFrequency + time * 0.7f) *
                waveHeight;
        }
    }
}
