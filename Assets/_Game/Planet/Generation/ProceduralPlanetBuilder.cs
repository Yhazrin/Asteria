using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Integrates procedural terrain generation with the existing planet system.
    /// Replaces the static sphere mesh with procedurally generated terrain.
    /// </summary>
    public sealed class ProceduralPlanetBuilder : MonoBehaviour
    {
        [Header("Planet Settings")]
        [SerializeField] float planetRadius = 300f;
        [SerializeField] int seed = 42;

        [Header("Generation")]
        [SerializeField] int meshResolution = 128;
        [SerializeField] int meshRings = 64;
        [SerializeField] float terrainAmplitude = 20f;
        [SerializeField] bool generateOnStart = true;

        [Header("Features")]
        [SerializeField] bool spawnCreatures = true;
        [SerializeField] bool spawnDecorations = true;

        [Header("References")]
        [SerializeField] PlanetBody planetBody;

        SphericalTerrainGenerator _terrainGenerator;
        BiomeMapper _biomeMapper;
        CreatureSpawner _creatureSpawner;

        void Start()
        {
            if (generateOnStart)
            {
                BuildProceduralPlanet();
            }
        }

        /// <summary>
        /// Build the entire procedural planet.
        /// </summary>
        public void BuildProceduralPlanet()
        {
            Debug.Log($"[ProceduralPlanet] Building planet with seed {seed}...");

            // Ensure PlanetBody exists
            if (planetBody == null)
            {
                planetBody = FindFirstObjectByType<PlanetBody>();
                if (planetBody == null)
                {
                    var go = new GameObject("ProceduralPlanet");
                    planetBody = go.AddComponent<PlanetBody>();
                    planetBody.Configure(planetRadius, 9.81f);
                }
            }

            // Generate terrain mesh
            GenerateTerrainMesh();

            // Spawn creatures
            if (spawnCreatures)
            {
                SpawnCreatures();
            }

            Debug.Log("[ProceduralPlanet] Planet generation complete.");
        }

        void GenerateTerrainMesh()
        {
            // Create terrain generator
            _terrainGenerator = gameObject.AddComponent<SphericalTerrainGenerator>();

            // Generate mesh
            Mesh terrainMesh = _terrainGenerator.GeneratePlanetMesh();

            // Apply to planet
            var meshFilter = planetBody.GetComponent<MeshFilter>();
            if (meshFilter == null)
            {
                meshFilter = planetBody.gameObject.AddComponent<MeshFilter>();
            }
            meshFilter.mesh = terrainMesh;

            // Update planet radius for terrain height
            planetBody.Configure(planetRadius, 9.81f);
        }

        void SpawnCreatures()
        {
            // Add creature spawner
            _creatureSpawner = gameObject.AddComponent<CreatureSpawner>();

            // Create default creature definitions
            var definitions = CreateDefaultCreatureDefinitions();

            // Use reflection to set the serialized field
            var field = typeof(CreatureSpawner).GetField("creatureTypes",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            field?.SetValue(_creatureSpawner, definitions);

            var planetField = typeof(CreatureSpawner).GetField("planet",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            planetField?.SetValue(_creatureSpawner, planetBody);

            var playerField = typeof(CreatureSpawner).GetField("player",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var player = FindFirstObjectByType<Player.SphericalGravityBody>();
            if (player != null) playerField?.SetValue(_creatureSpawner, player.transform);
        }

        CreatureDefinition[] CreateDefaultCreatureDefinitions()
        {
            return new[]
            {
                CreateCreature("curious_creature", "好奇生物", CreatureBehavior.Curious,
                    new Color(0.9f, 0.8f, 0.5f), 0.8f, 8f),
                CreateCreature("shy_creature", "胆小生物", CreatureBehavior.Shy,
                    new Color(0.6f, 0.7f, 0.9f), 0.6f, 5f),
                CreateCreature("guide_creature", "引路生物", CreatureBehavior.Guide,
                    new Color(0.5f, 0.9f, 0.7f), 1.2f, 12f),
            };
        }

        CreatureDefinition CreateCreature(string id, string name, CreatureBehavior behavior,
            Color color, float scale, float detection)
        {
            var def = ScriptableObject.CreateInstance<CreatureDefinition>();
            def.creatureId = id;
            def.displayName = name;
            def.behavior = behavior;
            def.bodyColor = color;
            def.scale = scale;
            def.detectionRadius = detection;
            def.moveSpeed = 3f;
            def.maxGroupSize = 3;
            def.spawnWeight = 1f;
            def.canBePhotographed = true;
            return def;
        }

        /// <summary>
        /// Regenerate the planet with a new seed.
        /// </summary>
        public void RegenerateWithSeed(int newSeed)
        {
            seed = newSeed;

            // Clean up old creatures
            if (_creatureSpawner != null)
            {
                foreach (var creature in _creatureSpawner.ActiveCreatures)
                {
                    if (creature != null) Destroy(creature.gameObject);
                }
            }

            // Rebuild
            BuildProceduralPlanet();
        }
    }
}
