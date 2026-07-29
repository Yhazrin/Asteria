using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// General creature configuration database for the game.
    /// Contains all creature parameters.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Creature Config General Database")]
    public sealed class CreatureConfigGeneralDatabase : ScriptableObject
    {
        [Header("Spawning")]
        public int maxCreatures = 30;
        public float spawnRadius = 200f;
        public float despawnDistance = 300f;
        public float spawnInterval = 5f;

        [Header("Movement")]
        public float defaultMoveSpeed = 3f;
        public float fleeSpeedMultiplier = 1.5f;

        [Header("Behavior")]
        public float detectionRadius = 10f;
        public float interactionRadius = 3f;
        public float trustGainPerInteraction = 0.1f;

        [Header("Groups")]
        public int maxGroupSize = 3;
    }
}
