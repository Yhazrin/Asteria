using System.Collections.Generic;
using System.Linq;
using Asteria.Planet;
using UnityEngine;

namespace Asteria.Residents
{
    /// <summary>
    /// Manages all residents on the home planet. Handles spawn, schedule ticks,
    /// and autonomous interactions between residents.
    /// </summary>
    public sealed class ResidentManager : MonoBehaviour
    {
        [SerializeField] ResidentDefinition[] residentDefinitions;
        [SerializeField] PlanetBody planet;

        readonly List<ResidentAgent> _agents = new();
        float _interactionCheckTimer;

        public IReadOnlyList<ResidentAgent> Agents => _agents;

        /// <summary>
        /// Initialize with definitions and planet before Start() runs.
        /// Call this from code that creates the manager at runtime.
        /// </summary>
        public void Initialize(ResidentDefinition[] definitions, PlanetBody targetPlanet)
        {
            residentDefinitions = definitions;
            planet = targetPlanet;
        }

        void Start()
        {
            if (planet == null)
            {
                planet = FindFirstObjectByType<PlanetBody>();
            }

            SpawnResidents();
        }

        void SpawnResidents()
        {
            if (residentDefinitions == null || residentDefinitions.Length == 0)
            {
                Debug.Log("[Asteria] No resident definitions assigned to ResidentManager.");
                return;
            }

            foreach (var def in residentDefinitions)
            {
                if (def == null)
                {
                    continue;
                }

                var go = GameObject.CreatePrimitive(PrimitiveType.Capsule);
                go.name = $"Resident_{def.displayName}";
                Destroy(go.GetComponent<CapsuleCollider>());
                CapsuleCollider col = go.AddComponent<CapsuleCollider>();
                col.height = 2f;
                col.radius = 0.35f;

                MaterialHelper.ApplyColor(go.GetComponent<MeshRenderer>(), def.bodyColor);

                // Add agent
                var agent = go.AddComponent<ResidentAgent>();
                agent.Initialize(def, planet);
                _agents.Add(agent);
            }

            Debug.Log($"[Asteria] Spawned {_agents.Count} residents.");
        }

        void Update()
        {
            // Check for autonomous interactions periodically
            _interactionCheckTimer -= Time.deltaTime;
            if (_interactionCheckTimer <= 0f)
            {
                _interactionCheckTimer = 3f; // Check every 3 seconds
                CheckAutonomousInteractions();
            }
        }

        void CheckAutonomousInteractions()
        {
            for (int i = 0; i < _agents.Count; i++)
            {
                for (int j = i + 1; j < _agents.Count; j++)
                {
                    var a = _agents[i];
                    var b = _agents[j];

                    if (a == null || b == null)
                    {
                        continue;
                    }

                    float distance = Vector3.Distance(a.transform.position, b.transform.position);
                    if (distance < 4f)
                    {
                        a.TryInteract(b);
                    }
                }
            }
        }

        /// <summary>
        /// Get a resident by ID.
        /// </summary>
        public ResidentAgent GetResident(string residentId)
        {
            return _agents.FirstOrDefault(a => a.Definition != null && a.Definition.residentId == residentId);
        }

        /// <summary>
        /// Get all resident states for saving.
        /// </summary>
        public ResidentState[] GetStatesForSave()
        {
            return _agents.Where(a => a.State != null).Select(a => a.State).ToArray();
        }
    }
}
