using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Memory configuration file database for the game.
    /// Contains all memory parameters.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Memory Config File Database")]
    public sealed class MemoryConfigFileDatabase : ScriptableObject
    {
        [Header("Memory")]
        public int maxMemoriesPerResident = 50;
        public float memoryDecayRate = 0.001f;
        public float permanentMemoryThreshold = 0.8f;

        [Header("Types")]
        public string[] memoryTypes = {
            "daily",
            "relationship",
            "conflict",
            "community",
            "expedition",
            "wish",
            "surprise"
        };

        [Header("Effects")]
        public float memoryInfluenceOnWish = 0.3f;
        public float memoryInfluenceOnEvent = 0.2f;
    }
}
