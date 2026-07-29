using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Keybinding definitions for the game.
    /// Contains all configurable key bindings.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Keybinding Database")]
    public sealed class KeybindingDatabase : ScriptableObject
    {
        [Header("Movement")]
        public KeyCode moveForward = KeyCode.W;
        public KeyCode moveBackward = KeyCode.S;
        public KeyCode moveLeft = KeyCode.A;
        public KeyCode moveRight = KeyCode.D;
        public KeyCode run = KeyCode.LeftShift;
        public KeyCode jump = KeyCode.Space;

        [Header("Interaction")]
        public KeyCode interact = KeyCode.E;
        public KeyCode tool1 = KeyCode.Alpha1;
        public KeyCode tool2 = KeyCode.Alpha2;
        public KeyCode tool3 = KeyCode.Alpha3;

        [Header("UI")]
        public KeyCode inventory = KeyCode.I;
        public KeyCode map = KeyCode.M;
        public KeyCode photo = KeyCode.P;
        public KeyCode settings = KeyCode.Escape;
        public KeyCode multiplayer = KeyCode.M;

        [Header("Debug")]
        public KeyCode debugToggle = KeyCode.F1;
        public KeyCode debugFPS = KeyCode.F2;
        public KeyCode debugGodMode = KeyCode.F3;
    }
}
