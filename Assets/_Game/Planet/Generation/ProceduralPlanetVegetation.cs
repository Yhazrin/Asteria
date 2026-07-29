using System.Collections.Generic;
using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Vegetation system for procedural planets.
    /// Handles trees, grass, flowers, and other plants.
    /// </summary>
    public sealed class ProceduralPlanetVegetation : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] int maxTrees = 100;
        [SerializeField] int maxGrass = 500;
        [SerializeField] int maxFlowers = 50;
        [SerializeField] float spawnRadius = 200f;
        [SerializeField] float despawnDistance = 250f;

        [Header("References")]
        [SerializeField] PlanetBody planet;
        [SerializeField] BiomeMapper biomeMapper;
        [SerializeField] Transform player;

        readonly List<VegetationInstance> _trees = new();
        readonly list<VegetationInstance> _grass = new();
        readonly list<VegetationInstance> _flowers = new();

        void Start()
        {
            if (planet == null)
                planet = FindFirstObjectByType<PlanetBody>();
            if (biomeMapper == null)
                biomeMapper = FindFirstObjectByType<BiomeMapper>();

            if (player == null)
            {
                var playerBody = FindFirstObjectByType<Player.SphericalGravityBody>();
                player = playerBody?.transform;
            }
        }

        void Update()
        {
            UpdateVegetation();
        }

        void UpdateVegetation()
        {
            if (player == null) return;

            // Remove distant vegetation
            RemoveDistantVegetation(_trees, player.position);
            RemoveDistantVegetation(_grass, player.position);
            RemoveDistantVegetation(_flowers, player.position);

            // Spawn new vegetation
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

        bool TrySpawnVegetation(VegetationType type)
        {
            if (planet == null) return false;

            Vector3 direction = Random.onUnitSphere;
            Vector3 position = planet.GetPointOnSurface(direction, 0.5f);

            // Check distance from player
            if (player != null)
            {
                float dist = Vector3.Distance(position, player.position);
                if (dist > spawnRadius) return false;
            }

            // Get biome
            var biome = biomeMapper?.GetBiome(direction, planet.Radius) ?? BiomeMapper.BiomeType.Plains;

            // Check density
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

            // Add mesh
            var filter = go.AddComponent<MeshFilter>();
            var renderer = go.AddComponent<MeshRenderer>();

            switch (type)
            {
                case VegetationType.Tree:
                    filter.mesh = Art.ProceduralAssets.MakeTreeMesh();
                    renderer.material = Art.MaterialLibrary.TreeLeaves;
                    _trees.Add(new VegetationInstance { root = go, direction = direction });
                    break;
                case VegetationType.Grass:
                    filter.mesh = Art.ProceduralAssets.MakeGrassMesh();
                    renderer.material = Art.MaterialLibrary.TerrainGrass;
                    go.transform.localScale *= 0.5f;
                    _grass.Add(new VegetationInstance { root = go, direction = direction });
                    break;
                case VegetationType.Flower:
                    filter.mesh = Art.ProceduralAssets.MakeCrystalMesh();
                    renderer.material = Art.ProceduralAssets.MakeMat(Art.ProceduralAssets.FlowerPink);
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
                _ => 0.1f
            };
        }

        float GetGrassDensity(BiomeMapper.BiomeType biome)
        {
            return biome switch
            {
                BiomeMapper.BiomeType.Plains => 0.8f,
                BiomeMapper.BiomeType.Forest => 0.5f,
                _ => 0.3f
            };
        }

        float GetFlowerDensity(BiomeMapper.BiomeType biome)
        {
            return biome switch
            {
                BiomeMapper.BiomeType.Plains => 0.5f,
                BiomeMapper.BiomeType.Forest => 0.3f,
                _ => 0.1f
            };
        }

        void RemoveDistantVegetation(list<VegetationInstance> instances, Vector3 playerPos)
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
