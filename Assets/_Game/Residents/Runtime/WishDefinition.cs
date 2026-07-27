using UnityEngine;

namespace Asteria.Residents
{
    /// <summary>
    /// A wish template that a resident can express. Connects home to expedition.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Wish Definition")]
    public sealed class WishDefinition : ScriptableObject
    {
        public string wishId = "wish_unnamed";
        public string displayName = "未命名愿望";
        [TextArea(2, 4)] public string description = "";

        [Header("Preconditions")]
        [Range(0f, 1f)] public float minAffinity;
        [Range(0f, 1f)] public float minCuriosity;

        [Header("Expedition Connection")]
        public string requiredExpeditionId;
        public string requiredDiscoveryId;

        [Header("Fulfillment")]
        [TextArea(2, 4)] public string fulfillmentText = "";
        public string followUpEventId;
        public string unlockFacilityId;
    }
}
