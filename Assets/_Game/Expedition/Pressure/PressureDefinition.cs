using UnityEngine;

namespace Asteria.Expedition
{
    /// <summary>
    /// Defines a pressure event (e.g., strong wind, cold, darkness).
    /// These are temporary environmental hazards, not permanent stat debuffs.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Pressure Definition")]
    public sealed class PressureDefinition : ScriptableObject
    {
        public string pressureId = "pressure_default";
        public string displayName = "压力";
        public PressureType pressureType;

        [Header("Timing")]
        public float warningDurationSeconds = 10f;
        public float activeDurationSeconds = 180f;

        [Header("Effects")]
        public string[] affectedStates = { "cold", "unbalanced" };
        public string[] counterTools = { "warm_light", "beacon" };

        [Header("Recovery")]
        public string recoveryMethod = "move_to_shelter";
        public float recoveryTimeSeconds = 5f;
    }

    public enum PressureType
    {
        Wind,
        Cold,
        Dark,
        Spores,
        Instability
    }
}
