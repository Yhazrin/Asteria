using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// UI test configuration database for the game.
    /// Contains all UI test parameters.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/UI Test Config Database")]
    public sealed class UITestConfigDatabase : ScriptableObject
    {
        [Header("UI Test")]
        public bool enableUITest = false;
        public float testTimeout = 60f;

        [Header("Scenarios")]
        public bool testMainMenu = true;
        public bool testSettings = true;
        public bool testInventory = true;
        public bool testCodex = true;
        public bool testMultiplayer = true;
    }
}
