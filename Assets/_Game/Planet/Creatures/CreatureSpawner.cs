using System.Collections.Generic;
using Asteria.Art;
using UnityEngine;

namespace Asteria.Planet.Creatures
{
    /// <summary>
    /// Spawns creatures on the planet surface based on biome and noise.
    /// Similar to Minecraft's mob spawning system.
    /// </summary>
    public sealed class CreatureSpawner : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float spawnRadius = 200f;
        [SerializeField] int maxCreatures = 30;
        [SerializeField] float spawnInterval = 5f;
        [SerializeField] float despawnDistance = 300f;

        [Header("Definitions")]
        [SerializeField] CreatureDefinition[] creatureTypes;

        [Header("References")]
        [SerializeField] PlanetBody planet;
        [SerializeField] Transform player;

        readonly List<CreatureAgent> _activeCreatures = new();
        float _spawnTimer;
        int _seed;

        void Start()
        {
            if (planet == null) planet = FindFirstObjectByType<PlanetBody>();
            _seed = Random.Range(0, 10000);
        }

        void Update()
        {
            _spawnTimer -= Time.deltaTime;
            if (_spawnTimer <= 0f)
            {
                _spawnTimer = spawnInterval;
                TrySpawnCreature();
            }

            CleanupDistantCreatures();
        }

        void TrySpawnCreature()
        {
            if (_activeCreatures.Count >= maxCreatures) return;
            if (creatureTypes == null || creatureTypes.Length == 0) return;
            if (planet == null) return;

            // Get player position for distance check
            Vector3 playerPos = player != null ? player.position : planet.Center;

            // Random position on planet
            Vector3 spawnDir = Random.onUnitSphere;
            Vector3 spawnPos = planet.GetPointOnSurface(spawnDir, 1f);

            // Check distance from player
            float distFromPlayer = Vector3.Distance(spawnPos, playerPos);
            if (distFromPlayer < 50f || distFromPlayer > spawnRadius) return;

            // Select creature type based on biome and weight
            CreatureDefinition selectedDef = SelectCreatureType(spawnDir);
            if (selectedDef == null) return;

            // Check spawn limits for this type
            int typeCount = 0;
            foreach (var c in _activeCreatures)
            {
                if (c.Definition == selectedDef) typeCount++;
            }
            if (typeCount >= selectedDef.maxGroupSize * 3) return;

            // Spawn creature
            SpawnCreature(selectedDef, spawnPos, spawnDir);
        }

        CreatureDefinition SelectCreatureType(Vector3 position)
        {
            // Weighted random selection
            float totalWeight = 0f;
            foreach (var def in creatureTypes)
            {
                totalWeight += def.spawnWeight;
            }

            float roll = Random.Range(0f, totalWeight);
            float cumulative = 0f;

            foreach (var def in creatureTypes)
            {
                cumulative += def.spawnWeight;
                if (roll <= cumulative) return def;
            }

            return creatureTypes[0];
        }

        void SpawnCreature(CreatureDefinition def, Vector3 position, Vector3 surfaceNormal)
        {
            // Use procedural creature mesh
            var go = ProceduralAssets.MakeCreature(position, def.bodyColor, def.scale);
            go.name = $"Creature_{def.displayName}";
            go.transform.up = surfaceNormal;

            // Add trigger for interaction
            var trigger = go.AddComponent<SphereCollider>();
            trigger.isTrigger = true;
            trigger.radius = def.interactionRadius;

            // Add creature agent
            var agent = go.AddComponent<CreatureAgent>();
            agent.Initialize(def, planet);

            // Add rigidbody for physics
            var rb = go.AddComponent<Rigidbody>();
            rb.useGravity = false;
            rb.constraints = RigidbodyConstraints.FreezeRotation;
            rb.interpolation = RigidbodyInterpolation.Interpolate;

            // Add spherical gravity
            var gravity = go.AddComponent<SphericalGravityBody>();
            gravity.Planet = planet;

            _activeCreatures.Add(agent);

            Debug.Log($"[Creature] Spawned {def.displayName} at {position}");
        }

        void CleanupDistantCreatures()
        {
            Vector3 playerPos = player != null ? player.position : Vector3.zero;

            for (int i = _activeCreatures.Count - 1; i >= 0; i--)
            {
                if (_activeCreatures[i] == null)
                {
                    _activeCreatures.RemoveAt(i);
                    continue;
                }

                float dist = Vector3.Distance(_activeCreatures[i].transform.position, playerPos);
                if (dist > despawnDistance)
                {
                    Destroy(_activeCreatures[i].gameObject);
                    _activeCreatures.RemoveAt(i);
                }
            }
        }

        /// <summary>
        /// Get all active creatures.
        /// </summary>
        public IReadOnlyList<CreatureAgent> ActiveCreatures => _activeCreatures;

        /// <summary>
        /// Get creatures within radius of a position.
        /// </summary>
        public List<CreatureAgent> GetCreaturesInRadius(Vector3 position, float radius)
        {
            var result = new List<CreatureAgent>();
            foreach (var creature in _activeCreatures)
            {
                if (creature == null) continue;
                if (Vector3.Distance(creature.transform.position, position) <= radius)
                {
                    result.Add(creature);
                }
            }
            return result;
        }
    }
}
