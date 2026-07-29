using System.Collections.Generic;
using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Generates and manages procedural vegetation on the planet.
    /// Handles trees, grass, flowers, and other plants.
    /// </summary>
    public sealed class ProceduralVegetation : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] int maxTrees = 100;
        [SerializeField] int maxGrass = 500;
        [SerializeField] int maxFlowers = 50;
        [SerializeField] float spawnRadius = 200f;
        [SerializeField] float despawnDistance = 250f;

        [Header("Density")]
        [SerializeField] float treeDensity = 0.3f;
        [SerializeField] float grassDensity = 0.6f;
        [SerializeField] float flowerDensity = 0.2f;

        [Header("References")]
        [SerializeField] PlanetBody planet;
        [SerializeField] BiomeMapper biomeMapper;
        [SerializeField] Transform player;

        readonly List<VegetationInstance> _trees = new();
        readonly List<VegetationInstance> _grass = new();
        readonly List<VegetationInstance> _flowers = new();

        Mesh _treeMesh;
        Mesh _grassMesh;
        Mesh _flowerMesh;
        Material _treeMaterial;
        Material _grassMaterial;
        Material _flowerMaterial;

        void Start()
        {
            if (planet == null)
                planet = FindFirstObjectByType<PlanetBody>();
            if (biomeMapper == null)
                biomeMapper = FindFirstObjectByType<BiomeMapper>();

            CreateMeshes();
            CreateMaterials();
            SpawnInitialVegetation();
        }

        void Update()
        {
            UpdateVegetation();
        }

        void CreateMeshes()
        {
            // Tree mesh
            _treeMesh = Art.ProceduralAssets.MakeTreeMesh();

            // Grass mesh
            _grassMesh = Art.ProceduralAssets.MakeGrassMesh();

            // Flower mesh
            _flowerMesh = Art.ProceduralAssets.MakeCrystalMesh(); // Reuse crystal as flower
        }

        void CreateMaterials()
        {
            _treeMaterial = Art.MaterialLibrary.TreeLeaves;
            _grassMaterial = Art.MaterialLibrary.TerrainGrass;
            _flowerMaterial = Art.ProceduralAssets.MakeMat(Art.ProceduralAssets.FlowerPink);
        }

        void SpawnInitialVegetation()
        {
            if (player == null)
            {
                var playerBody = FindFirstObjectByType<Player.SphericalGravityBody>();
                player = playerBody?.transform;
            }

            // Spawn trees
            for (int i = 0; i < maxTrees; i++)
            {
                TrySpawnVegetation(VegetationType.Tree);
            }

            // Spawn grass
            for (int i = 0; i < maxGrass; i++)
            {
                TrySpawnVegetation(VegetationType.Grass);
            }

            // Spawn flowers
            for (int i = 0; i < maxFlowers; i++)
            {
                TrySpawnVegetation(VegetationType.Flower);
            }
        }

        bool TrySpawnVegetation(VegetationType type)
        {
            // Random position on planet
            Vector3 direction = Random.onUnitSphere;
            Vector3 position = planet.GetPointOnSurface(direction, 0.5f);

            // Check distance from player
            if (player != null)
            {
                float dist = Vector3.Distance(position, player.position);
                if (dist > spawnRadius) return false;
            }

            // Get biome at position
            var biome = biomeMapper?.GetBiome(direction, planet.Radius) ?? BiomeMapper.BiomeType.Plains;

            // Check density based on biome
            float density = type switch
            {
                VegetationType.Tree => GetTreeDensity(biome),
                VegetationType.Grass => GetGrassDensity(biome),
                VegetationType.Flower => GetFlowerDensity(biome),
                _ => 0.1f
            };

            if (Random.value > density) return false;

            // Create vegetation
            var go = new GameObject($"{type}_{direction}");
            go.transform.SetParent(transform, false);
            go.transform.position = position;
            go.transform.up = direction;
            go.transform.localScale = Vector3.one * Random.Range(0.8f, 1.2f);

            var filter = go.AddComponent<MeshFilter>();
            var renderer = go.AddComponent<MeshRenderer>();

            switch (type)
            {
                case VegetationType.Tree:
                    filter.mesh = _treeMesh;
                    renderer.material = _treeMaterial;
                    _trees.Add(new VegetationInstance { root = go, direction = direction });
                    break;

                case VegetationType.Grass:
                    filter.mesh = _grassMesh;
                    renderer.material = _grassMaterial;
                    go.transform.localScale *= 0.5f;
                    _grass.Add(new VegetationInstance { root = go, direction = direction });
                    break;

                case VegetationType.Flower:
                    filter.mesh = _flowerMesh;
                    renderer.material = _flowerMaterial;
                    go.transform.localScale *= 0.3f;
                    _flowers.Add(new VegetationInstance { root = go, direction = direction });
                    break;
            }

            return true;
        }

        float GetTreeDensity(BiomeMapper.BiomeType biome)
        {
            return biome switch
            {
                BiomeMapper.BiomeType.Forest => 0.8f,
                BiomeMapper.BiomeType.Woodland => 0.6f,
                BiomeMapper.BiomeType.Jungle => 0.9f,
                BiomeMapper.BiomeType.Plains => 0.2f,
                BiomeMapper.BiomeType.Savanna => 0.1f,
                _ => 0.1f
            };
        }

        float GetGrassDensity(BiomeMapper.BiomeType biome)
        {
            return biome switch
            {
                BiomeMapper.BiomeType.Plains => 0.8f,
                BiomeMapper.BiomeType.Forest => 0.5f,
                BiomeMapper.BiomeType.Savanna => 0.6f,
                BiomeMapper.BiomeType.Swamp => 0.4f,
                _ => 0.3f
            };
        }

        float GetFlowerDensity(BiomeMapper.BiomeType biome)
        {
            return biome switch
            {
                BiomeMapper.BiomeType.Plains => 0.5f,
                BiomeMapper.BiomeType.Forest => 0.3f,
                BiomeMapper.BiomeType.Swamp => 0.2f,
                _ => 0.1f
            };
        }

        void UpdateVegetation()
        {
            if (player == null) return;

            Vector3 playerPos = player.position;

            // Remove distant vegetation
            RemoveDistantVegetation(_trees, playerPos);
            RemoveDistantVegetation(_grass, playerPos);
            RemoveDistantVegetation(_flowers, playerPos);

            // Spawn new vegetation near player
            int treesToSpawn = maxTrees - _trees.Count;
            int grassToSpawn = maxGrass - _grass.Count;
            int flowersToSpawn = maxFlowers - _flowers.Count;

            for (int i = 0; i < Mathf.Min(treesToSpawn, 5); i++)
                TrySpawnVegetation(VegetationType.Tree);
            for (int i = 0; i < Mathf.Min(grassToSpawn, 10); i++)
                TrySpawnVegetation(VegetationType.Grass);
            for (int i = 0; i < Mathf.Min(flowersToSpawn, 3); i++)
                TrySpawnVegetation(VegetationType.Flower);
        }

        void RemoveDistantVegetation(List<VegetationInstance> instances, Vector3 playerPos)
        {
            for (int i = instances.Count - 1; i >= 0; i--)
            {
                if (instances[i].root == null)
                {
                    instances.RemoveAt(i);
                    continue;
                }

                float dist = Vector3.Distance(instances[i].root.transform.position, playerPos);
                if (dist > despawnDistance)
                {
                    Destroy(instances[i].root);
                    instances.RemoveAt(i);
                }
            }
        }

        void OnDestroy()
        {
            foreach (var v in _trees) if (v.root != null) Destroy(v.root);
            foreach (var v in _grass) if (v.root != null) Destroy(v.root);
            foreach (var v in _flowers) if (v.root != null) Destroy(v.root);
        }

        enum VegetationType { Tree, Grass, Flower }

        class VegetationInstance
        {
            public GameObject root;
            public Vector3 direction;
        }
    }
}
