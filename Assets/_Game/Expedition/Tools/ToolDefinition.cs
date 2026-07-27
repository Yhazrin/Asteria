using UnityEngine;

namespace Asteria.Expedition
{
    /// <summary>
    /// Defines a player tool (工具) for expeditions.
    /// Tools have specific interaction capabilities and energy limits.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Tool Definition")]
    public sealed class ToolDefinition : ScriptableObject
    {
        public string toolId = "tool_default";
        public string displayName = "工具";
        [TextArea(1, 3)] public string description = "";
        public ToolSlotType slotType = ToolSlotType.Active1;

        [Header("Energy")]
        public float maxEnergy = 100f;
        public float rechargeRate = 5f;

        [Header("Capabilities")]
        public string[] interactionTags = { };
    }

    public enum ToolSlotType
    {
        Active1,
        Active2,
        SharedBeacon
    }
}
