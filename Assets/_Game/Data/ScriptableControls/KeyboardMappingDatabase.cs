using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Keyboard mapping database for the game.
    /// Contains all keyboard key mappings.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Keyboard Mapping Database")]
    public sealed class KeyboardMappingDatabase : ScriptableObject
    {
        [Header("Movement")]
        public string keyForward = "W";
        public string keyBackward = "S";
        public string keyLeft = "A";
        public string keyRight = "D";
        public string keyJump = "Space";
        public string keyRun = "LeftShift";

        [Header("Interaction")]
        public string keyInteract = "E";
        public string keyTool1 = "1";
        public string keyTool2 = "2";
        public string keyTool3 = "3";

        [Header("UI")]
        public string keyInventory = "I";
        public string keyMap = "M";
        public string keyPhoto = "P";
        public string keySettings = "Escape";
        public string keyMultiplayer = "M";

        [Header("Debug")]
        public string keyDebugToggle = "F1";
        public string keyDebugFPS = "F2";
        public string keyDebugGodMode = "F3";
    }
}
