using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Photo mode configuration file database for the game.
    /// Contains all photo mode parameters.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Photo Mode Config File Database")]
    public sealed class PhotoModeConfigFileDatabase : ScriptableObject
    {
        [Header("Camera")]
        public float cameraSensitivity = 2f;
        public float moveSpeed = 10f;
        public float fastMoveSpeed = 30f;

        [Header("UI")]
        public KeyCode toggleKey = KeyCode.P;
        public KeyCode captureKey = KeyCode.Space;

        [Header("Output")]
        public int captureResolution = 2;
        public string captureFolder = "Photos";
    }
}
