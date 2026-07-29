using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Control scheme database for the game.
    /// Contains all control schemes and bindings.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Control Scheme Database")]
    public sealed class ControlSchemeDatabase : ScriptableObject
    {
        [Header("Keyboard")]
        public string keyboardMoveForward = "W";
        public string keyboardMoveBack = "S";
        public string keyboardMoveLeft = "A";
        public string keyboardMoveRight = "D";
        public string keyboardJump = "Space";
        public string keyboardRun = "LeftShift";
        public string keyboardInteract = "E";
        public string keyboardTool1 = "1";
        public string keyboardTool2 = "2";
        public string keyboardInventory = "I";
        public string keyboardMap = "M";
        public string keyboardPhoto = "P";
        public string keyboardSettings = "Escape";

        [Header("Gamepad")]
        public string gamepadMove = "Left Stick";
        public string gamepadLook = "Right Stick";
        public string gamepadJump = "A";
        public string gamepadRun = "Left Stick Press";
        public string gamepadInteract = "X";
        public string gamepadTool1 = "LB";
        public string gamepadTool2 = "RB";
        public string gamepadInventory = "Y";
        public string gamepadMap = "Back";
        public string gamepadPhoto = "Right Stick Press";
        public string gamepadSettings = "Start";
    }
}
