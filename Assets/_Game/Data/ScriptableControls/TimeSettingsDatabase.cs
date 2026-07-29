using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Time settings database for the game.
    /// Contains all Unity Time settings.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Time Settings Database")]
    public sealed class TimeSettingsDatabase : ScriptableObject
    {
        [Header("Time")]
        public float fixedTimestep = 0.02f; // 50 Hz
        public float maximumTimestep = 0.1f;
        public float timeScale = 1f;
        public float maximumParticleDeltaTime = 0.03f;

        [Header("Game")]
        public float secondsPerDay = 720f; // 12 minutes = 1 game day
        public float weatherCycleDuration = 3600f; // 1 hour
    }
}
