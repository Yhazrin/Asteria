using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Recovery configuration database for the game.
    /// Contains all recovery parameters.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Recovery Config Database")]
    public sealed class RecoveryConfigDatabase : ScriptableObject
    {
        [Header("Recovery")]
        public bool enableRecovery = true;
        public int maxRecoveryAttempts = 3;
        public float recoveryTimeout = 30f;

        [Header("Save Recovery")]
        public bool autoRecoverSave = true;
        public string recoverySaveSuffix = ".recovered";
        public bool validateRecoveredSave = true;

        [Header("Session Recovery")]
        public bool autoRecoverSession = true;
        public float sessionRecoveryTimeout = 60f;
    }
}
