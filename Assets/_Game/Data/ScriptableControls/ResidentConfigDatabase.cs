using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Resident configuration database for the game.
    /// Contains all resident parameters.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Resident Config Database")]
    public sealed class ResidentConfigDatabase : ScriptableObject
    {
        [Header("Movement")]
        public float moveSpeed = 4f;
        public float rotationSpeed = 8f;

        [Header("Schedule")]
        public float scheduleDurationMin = 30f;
        public float scheduleDurationMax = 90f;

        [Header("Interaction")]
        public float interactionDistance = 4f;
        public float interactionCooldown = 15f;
        public float interactionCheckInterval = 3f;

        [Header("Needs")]
        public float needDecayRate = 0.01f;
        public float needRecoveryRate = 0.05f;

        [Header("Memory")]
        public int maxMemories = 50;
        public float memoryDecayRate = 0.001f;
    }
}
