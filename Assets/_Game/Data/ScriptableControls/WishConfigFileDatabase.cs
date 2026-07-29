using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Wish configuration file database for the game.
    /// Contains all wish parameters.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Wish Config File Database")]
    public sealed class WishConfigFileDatabase : ScriptableObject
    {
        [Header("Wishes")]
        public int maxActiveWishes = 6;
        public float wishCheckInterval = 10f;
        public float wishCooldownDays = 5f;

        [Header("Preconditions")]
        public float minAffinityForWish = 0.3f;
        public float minCuriosityForWish = 0.4f;

        [Header("Fulfillment")]
        public float affinityGainOnFulfill = 0.15f;
        public float trustGainOnFulfill = 0.1f;
    }
}
