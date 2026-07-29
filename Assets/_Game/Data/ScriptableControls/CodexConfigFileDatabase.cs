using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Codex configuration file database for the game.
    /// Contains all codex parameters.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Codex Config File Database")]
    public sealed class CodexConfigFileDatabase : ScriptableObject
    {
        [Header("Codex")]
        public int totalEntries = 7;
        public int defaultEntries = 7;

        [Header("Display")]
        public float entryDuration = 4f;
        public float entryFadeSpeed = 2f;

        [Header("Progress")]
        public bool showCompletionPercentage = true;
        public bool showDiscoveryOrder = true;
    }
}
