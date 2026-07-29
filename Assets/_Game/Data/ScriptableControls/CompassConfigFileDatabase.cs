using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Compass configuration file database for the game.
    /// Contains all compass parameters.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Compass Config File Database")]
    public sealed class CompassConfigFileDatabase : ScriptableObject
    {
        [Header("Compass")]
        public float compassRadius = 80f;
        public float markerSize = 20f;
        public float updateInterval = 0.2f;

        [Header("Display")]
        public float edgeFadeSpeed = 2f;
        public float edgeFadeMinAlpha = 0.2f;
        public float edgeFadeMaxAlpha = 1f;
    }
}
