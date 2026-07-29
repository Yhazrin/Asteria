using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Level design parameters database.
    /// Contains all level design values for the game.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Level Design Database")]
    public sealed class LevelDesignDatabase : ScriptableObject
    {
        [Header("Home Planet")]
        public float homePlanetRadius = 180f;
        public int homeResidentCount = 6;
        public int homeFacilityCount = 8;
        public int homeAnchorCount = 12;

        [Header("Expedition")]
        public float expeditionRadius = 300f;
        public int expeditionPoiCount = 8;
        public int expeditionEventCount = 8;
        public float expeditionDuration = 25f;

        [Header("Progression")]
        public int discoveryThreshold10 = 10;
        public int discoveryThreshold50 = 50;
        public int expeditionThreshold10 = 10;
        public int residentThreshold6 = 6;

        [Header("Economy")]
        public int maxInventorySlots = 20;
        public int maxToolEnergy = 100;
        public float toolRechargeRate = 5f;

        [Header("Social")]
        public float interactionDistance = 4f;
        public float interactionCooldown = 15f;
        public float scheduleDurationMin = 30f;
        public float scheduleDurationMax = 90f;
    }
}
