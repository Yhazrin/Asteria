using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Gamepad mapping database for the game.
    /// Contains all gamepad button mappings.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Gamepad Mapping Database")]
    public sealed class GamepadMappingDatabase : ScriptableObject
    {
        [Header("Face Buttons")]
        public string buttonA = "Jump";
        public string buttonB = "Cancel";
        public string buttonX = "Interact";
        public string buttonY = "Inventory";

        [Header("Shoulder Buttons")]
        public string leftBumper = "Tool1";
        public string rightBumper = "Tool2";
        public string leftTrigger = "Aim";
        public string rightTrigger = "Use Tool";

        [Header("Sticks")]
        public string leftStickPress = "Run";
        public string rightStickPress = "Photo";

        [Header("System")]
        public string startButton = "Settings";
        public string backButton = "Map";

        [Header("Sticks")]
        public string leftStickUp = "Move Forward";
        public string leftStickDown = "Move Back";
        public string leftStickLeft = "Move Left";
        public string leftStickRight = "Move Right";
        public string rightStickUp = "Look Up";
        public string rightStickDown = "Look Down";
        public string rightStickLeft = "Look Left";
        public string rightStickRight = "Look Right";
    }
}
