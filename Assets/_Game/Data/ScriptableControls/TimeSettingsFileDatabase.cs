using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Time settings file database for the game.
    /// Contains all Unity Time settings.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Time Settings File Database")]
    public sealed class TimeSettingsFileDatabase : ScriptableObject
    {
        [Header("Time")]
        public float fixedTimestep = 0.02f;
        public float maximumTimestep = 0.1f;
        public float timeScale = 1f;
        public float maximumParticleDeltaTime = 0.03f;

        [Header("Game")]
        public float secondsPerDay = 720f;
        public float weatherCycleDuration = 3600f;
    }
}
