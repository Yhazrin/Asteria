using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Balance parameters database.
    /// Contains all balance values for the game.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Balance Database")]
    public sealed class BalanceDatabase : ScriptableObject
    {
        [Header("Progression")]
        public float discoveryXp = 10f;
        public float expeditionXp = 50f;
        public float socialXp = 20f;
        public float buildXp = 30f;

        [Header("Economy")]
        public int maxInventorySlots = 20;
        public int maxStackSize = 99;
        public float resourceRespawnTime = 300f;

        [Header("Social")]
        public float affinityGainPerInteraction = 0.05f;
        public float trustGainPerExpedition = 0.1f;
        public float tensionDecayRate = 0.01f;
        public float moodRecoveryRate = 0.05f;

        [Header("Survival")]
        public float coldDamageRate = 0.1f;
        public float rescueHealAmount = 0.5f;
        public float shelterRecoveryRate = 0.2f;
        public float pressureEscalationRate = 0.01f;

        [Header("Building")]
        public float buildTimeMultiplier = 1f;
        public float facilityEffectMultiplier = 1f;
        public int maxFacilitiesPerType = 3;

        [Header("Expedition")]
        public float eventFrequencyMultiplier = 1f;
        public float rewardMultiplier = 1f;
        public float difficultyScaling = 1f;
    }
}
