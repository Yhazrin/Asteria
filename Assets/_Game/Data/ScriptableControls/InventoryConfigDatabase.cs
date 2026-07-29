using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Inventory configuration database for the game.
    /// Contains all inventory parameters.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Inventory Config Database")]
    public sealed class InventoryConfigDatabase : ScriptableObject
    {
        [Header("Inventory")]
        public int maxSlots = 20;
        public int maxStackSize = 99;

        [Header("Categories")]
        public string[] categories = { "seed", "photo", "souvenir", "material" };

        [Header("UI")]
        public int columns = 4;
        public float slotSize = 80f;
        public float slotSpacing = 10f;
    }
}
