using UnityEngine;

namespace Asteria.Expedition
{
    /// <summary>
    /// Defines an expedition that players can embark on from home.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Expedition Definition")]
    public sealed class ExpeditionDefinition : ScriptableObject
    {
        public string expeditionId = "exp_default";
        public string displayName = "远征";
        [TextArea(2, 4)] public string description = "";

        [Header("Planet")]
        public string planetArchetypeId = "wind_grassland";
        public string sceneName = "SphereMoveDemo";

        [Header("Content")]
        public string[] availableDiscoveries;
        public float targetDurationMinutes = 25f;

        [Header("Rewards")]
        public string[] rewardSeedIds;
        public string[] followUpEventIds;
    }
}
