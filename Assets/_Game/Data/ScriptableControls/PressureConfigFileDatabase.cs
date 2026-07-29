using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Pressure configuration file database for the game.
    /// Contains all pressure parameters.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Pressure Config File Database")]
    public sealed class PressureConfigFileDatabase : ScriptableObject
    {
        [Header("Timing")]
        public float warningDuration = 10f;
        public float activeDuration = 180f;
        public float triggerDelay = 180f;

        [Header("Effects")]
        public float coldSpeedMultiplier = 0.7f;
        public float unbalancedControlMultiplier = 0.5f;
        public float fallThreshold = 2f;

        [Header("Recovery")]
        public float recoveryTime = 5f;
        public string recoveryMethod = "move_to_shelter";
    }
}
