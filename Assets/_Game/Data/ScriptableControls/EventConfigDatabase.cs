using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Event configuration database for the game.
    /// Contains all event parameters.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Event Config Database")]
    public sealed class EventConfigDatabase : ScriptableObject
    {
        [Header("Event Director")]
        public float evaluationInterval = 2f;
        public int maxActiveEvents = 3;

        [Header("Cooldowns")]
        public float dailyCooldown = 0.5f;
        public float relationshipCooldown = 2f;
        public float conflictCooldown = 3f;
        public float communityCooldown = 5f;
        public float expeditionCooldown = 7f;

        [Header("Scoring")]
        public float moodWeight = 20f;
        public float diversityWeight = 15f;
        public float dwellTimeWeight = 15f;
        public float accessibilityWeight = 10f;
    }
}
