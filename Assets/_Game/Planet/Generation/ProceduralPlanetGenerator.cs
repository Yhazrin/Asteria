using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// High-level procedural planet generator.
    /// Coordinates all sub-systems to create a complete planet.
    /// </summary>
    public sealed class ProceduralPlanetGenerator : MonoBehaviour
    {
        [Header("Planet Settings")]
        [SerializeField] float planetRadius = 300f;
        [SerializeField] int seed = 42;
        [SerializeField] string planetName = "New Planet";

        [Header("Generation Flags")]
        [SerializeField] bool generateOnStart = true;
        [SerializeField] bool enableLOD = true;
        [SerializeField] bool enableDeformation = true;

        [Header("Sub-Systems")]
        [SerializeField] ProceduralPlanetRenderer renderer;
        [SerializeField] BiomeMapper biomeMapper;
        [SerializeField] ChunkManager chunkManager;
        [SerializeField] LODSystem lodSystem;
        [SerializeField] TerrainDeformation deformationSystem;

        // State
        bool _isGenerated;
        PlanetData _planetData;

        void Start()
        {
            if (generateOnStart)
            {
                Generate();
            }
        }

        /// <summary>
        /// Generate the planet.
        /// </summary>
        public void Generate()
        {
            if (_isGenerated)
            {
                Debug.LogWarning("[ProceduralPlanetGenerator] Planet already generated.");
                return;
            }

            Debug.Log($"[ProceduralPlanetGenerator] Generating planet: {planetName} (seed: {seed})");

            // Create planet body
            var planetGo = new GameObject(planetName);
            planetGo.transform.SetParent(transform, false);
            var planetBody = planetGo.AddComponent<PlanetBody>();
            planetBody.Configure(planetRadius, 9.81f);

            // Create renderer
            if (renderer == null)
            {
                renderer = planetGo.AddComponent<ProceduralPlanetRenderer>();
            }

            // Create biome mapper
            if (biomeMapper == null)
            {
                biomeMapper = planetGo.AddComponent<BiomeMapper>();
            }

            // Create chunk manager
            if (enableLOD && chunkManager == null)
            {
                chunkManager = planetGo.AddComponent<ChunkManager>();
            }

            // Create LOD system
            if (enableLOD && lodSystem == null)
            {
                lodSystem = planetGo.AddComponent<LODSystem>();
            }

            // Create deformation system
            if (enableDeformation && deformationSystem == null)
            {
                deformationSystem = planetGo.AddComponent<TerrainDeformation>();
            }

            // Store planet data
            _planetData = new PlanetData
            {
                name = planetName,
                radius = planetRadius,
                seed = seed,
                planetBody = planetBody,
                renderer = renderer,
                biomeMapper = biomeMapper,
                chunkManager = chunkManager,
                lodSystem = lodSystem,
                deformationSystem = deformationSystem
            };

            _isGenerated = true;
            Debug.Log($"[ProceduralPlanetGenerator] Planet generation complete: {planetName}");
        }

        /// <summary>
        /// Regenerate the planet with a new seed.
        /// </summary>
        public void Regenerate(int newSeed)
        {
            seed = newSeed;
            _isGenerated = false;

            // Destroy old planet
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }

            // Regenerate
            Generate();
        }

        /// <summary>
        /// Get the planet data.
        /// </summary>
        public PlanetData GetPlanetData()
        {
            return _planetData;
        }

        /// <summary>
        /// Check if the planet is generated.
        /// </summary>
        public bool IsGenerated()
        {
            return _isGenerated;
        }

        [System.Serializable]
        public class PlanetData
        {
            public string name;
            public float radius;
            public int seed;
            public PlanetBody planetBody;
            public ProceduralPlanetRenderer renderer;
            public BiomeMapper biomeMapper;
            public ChunkManager chunkManager;
            public LODSystem lodSystem;
            public TerrainDeformation deformationSystem;
        }
    }
}
