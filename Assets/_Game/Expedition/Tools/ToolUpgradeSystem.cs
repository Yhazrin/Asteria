using System.Collections.Generic;
using UnityEngine;

namespace Asteria.Expedition
{
    /// <summary>
    /// Manages tool upgrades and progression.
    /// Tools can be improved through discoveries and achievements.
    /// </summary>
    public sealed class ToolUpgradeSystem : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] ToolUpgradeDefinition[] availableUpgrades;

        readonly Dictionary<string, ToolState> _toolStates = new();
        readonly Dictionary<string, List<string>> _appliedUpgrades = new();

        void Awake()
        {
            InitializeTools();
        }

        void InitializeTools()
        {
            // Initialize default tools
            var defaultTools = new[]
            {
                new ToolState { toolId = "resonance_mirror", level = 1, energy = 100, maxEnergy = 100 },
                new ToolState { toolId = "warm_light", level = 1, energy = 100, maxEnergy = 100 },
                new ToolState { toolId = "beacon", level = 1, energy = 100, maxEnergy = 100 },
                new ToolState { toolId = "repair_beam", level = 1, energy = 100, maxEnergy = 100 },
                new ToolState { toolId = "tether_rope", level = 1, energy = 100, maxEnergy = 100 },
                new ToolState { toolId = "eco_jar", level = 1, energy = 100, maxEnergy = 100 },
            };

            foreach (var tool in defaultTools)
            {
                _toolStates[tool.toolId] = tool;
                _appliedUpgrades[tool.toolId] = new List<string>();
            }
        }

        /// <summary>
        /// Get the state of a tool.
        /// </summary>
        public ToolState GetToolState(string toolId)
        {
            return _toolStates.TryGetValue(toolId, out var state) ? state : null;
        }

        /// <summary>
        /// Use a tool (consume energy).
        /// </summary>
        public bool UseTool(string toolId, float energyCost)
        {
            if (!_toolStates.TryGetValue(toolId, out var state)) return false;
            if (state.energy < energyCost) return false;

            state.energy -= energyCost;
            return true;
        }

        /// <summary>
        /// Recharge a tool.
        /// </summary>
        public void RechargeTool(string toolId, float amount)
        {
            if (_toolStates.TryGetValue(toolId, out var state))
            {
                state.energy = Mathf.Min(state.maxEnergy, state.energy + amount);
            }
        }

        /// <summary>
        /// Apply an upgrade to a tool.
        /// </summary>
        public bool ApplyUpgrade(string toolId, string upgradeId)
        {
            if (!_toolStates.TryGetValue(toolId, out var state)) return false;
            if (_appliedUpgrades[toolId].Contains(upgradeId)) return false;

            // Find upgrade definition
            ToolUpgradeDefinition upgrade = null;
            if (availableUpgrades != null)
            {
                foreach (var u in availableUpgrades)
                {
                    if (u.upgradeId == upgradeId)
                    {
                        upgrade = u;
                        break;
                    }
                }
            }

            if (upgrade == null) return false;

            // Apply effects
            state.level++;
            state.maxEnergy += upgrade.energyBonus;
            state.energy = state.maxEnergy;

            _appliedUpgrades[toolId].Add(upgradeId);

            Debug.Log($"[Tool] Upgraded {toolId} to level {state.level} with {upgradeId}");
            return true;
        }

        /// <summary>
        /// Check if a tool has a specific upgrade.
        /// </summary>
        public bool HasUpgrade(string toolId, string upgradeId)
        {
            return _appliedUpgrades.TryGetValue(toolId, out var upgrades) && upgrades.Contains(upgradeId);
        }

        /// <summary>
        /// Get all tool states for saving.
        /// </summary>
        public Dictionary<string, ToolState> GetAllStates()
        {
            return new Dictionary<string, ToolState>(_toolStates);
        }

        /// <summary>
        /// Restore tool states from save data.
        /// </summary>
        public void RestoreStates(Dictionary<string, ToolState> states)
        {
            _toolStates.Clear();
            foreach (var kvp in states)
            {
                _toolStates[kvp.Key] = kvp.Value;
            }
        }

        [System.Serializable]
        public class ToolState
        {
            public string toolId;
            public int level;
            public float energy;
            public float maxEnergy;
        }
    }

    [CreateAssetMenu(menuName = "Asteria/Tool Upgrade Definition")]
    public sealed class ToolUpgradeDefinition : ScriptableObject
    {
        public string upgradeId;
        public string displayName;
        [TextArea(1, 3)] public string description;
        public string targetToolId;
        public int requiredLevel = 1;
        public float energyBonus = 20f;
        public string[] requiredDiscoveries;
        public string effectDescription;
    }
}
