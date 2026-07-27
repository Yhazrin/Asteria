using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// A shared memory card generated after an expedition.
    /// Records who participated, what happened, and what was discovered.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Shared Memory Card")]
    public sealed class SharedMemoryCard : ScriptableObject
    {
        public string cardId = "memory_default";
        public string title = "共同回忆";
        [TextArea(3, 6)] public string description = "";

        [Header("Participants")]
        public string[] playerIds;
        public string[] residentIds;

        [Header("Expedition")]
        public string expeditionId;
        public string planetName;
        public string weatherCondition;

        [Header("Story")]
        public string keyDiscovery;
        public string whoHelpedWhom;
        public string chosenEnding;
        public string photoReference;

        [Header("Effects")]
        public string[] triggeredHomeEvents;
        public string[] affectedRelationships;
    }
}
