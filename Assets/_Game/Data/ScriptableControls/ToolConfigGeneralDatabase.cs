using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// General tool configuration database for the game.
    /// Contains all tool parameters.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Tool Config General Database")]
    public sealed class ToolConfigGeneralDatabase : ScriptableObject
    {
        [Header("General")]
        public int maxActiveTools = 2;
        public int maxSharedBeacons = 1;

        [Header("Energy")]
        public float maxEnergy = 100f;
        public float rechargeRate = 5f;
        public float energyPerUse = 10f;

        [Header("Placement")]
        public float placementRadius = 2f;
        public float placementCooldown = 1f;
    }
}
