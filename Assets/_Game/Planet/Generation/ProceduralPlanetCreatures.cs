using System.Collections.Generic;
using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Creature system for procedural planets.
    /// Handles creature spawning, behavior, and interactions.
    /// </summary>
    public sealed class ProceduralPlanetCreatures : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] int maxCreatures = 30;
        [SerializeField] float spawnRadius = 200f;
        [SerializeField] float despawnDistance = 300f;
        [SerializeField] float spawnInterval = 5f;

        [Header("References")]
        [SerializeField] PlanetBody planet;
        [SerializeField] Creatures.CreatureDefinition[] creatureDefinitions;
        [SerializeField] Transform player;

        readonly List<Creatures.CreatureAgent> _activeCreatures = new();
        float _spawnTimer;

        void Start()
        {
            if (planet == null)
                planet = FindFirstObjectByType<PlanetBody>();

            if (player == null)
            {
                var playerBody = FindFirstObjectByType<Player.SphericalGravityBody>();
                player = playerBody?.transform;
            }
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
            if (creatureDefinitions == null || creatureDefinitions.Length == 0) return;
            if (planet == null) return;

            // Random position
            Vector3 direction = Random.onUnitSphere;
            Vector3 position = planet.GetPointOnSurface(direction, 1f);

            // Check distance from player
            if (player != null)
            {
                float dist = Vector3.Distance(position, player.position);
                if (dist < 50f || dist > spawnRadius) return;
            }

            // Select creature type
            var definition = creatureDefinitions[Random.Range(0, creatureDefinitions.Length)];

            // Spawn creature
            var go = new GameObject($"Creature_{definition.displayName}");
            go.transform.position = position;
            go.transform.up = direction;

            var agent = go.AddComponent<Creatures.CreatureAgent>();
            agent.Initialize(definition, planet);

            _activeCreatures.Add(agent);
        }

        void CleanupDistantCreatures()
        {
            if (player == null) return;

            for (int i = _activeCreatures.Count - 1; i >= 0; i--)
            {
                if (_activeCreatures[i] == null)
                {
                    _activeCreatures.RemoveAt(i);
                    continue;
                }

                float dist = Vector3.Distance(_activeCreatures[i].transform.position, player.position);
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
        public IReadOnlyList<Creatures.CreatureAgent> GetActiveCreatures()
        {
            return _activeCreatures;
        }

        /// <summary>
        /// Get creatures within radius.
        /// </summary>
        public List<Creatures.CreatureAgent> GetCreaturesInRadius(Vector3 position, float radius)
        {
            var result = new List<Creatures.CreatureAgent>();
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
