using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// General compass configuration database for the game.
    /// Contains all compass parameters.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Compass Config General Database")]
    public sealed class CompassConfigGeneralDatabase : ScriptableObject
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
