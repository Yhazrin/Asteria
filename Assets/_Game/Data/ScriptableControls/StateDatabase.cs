using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Game state database for the game.
    /// Contains all game state data.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/State Database")]
    public sealed class StateDatabase : ScriptableObject
    {
        [Header("Player State")]
        public float playerHealth = 1f;
        public float playerEnergy = 1f;
        public float playerTemperature = 0.5f;
        public string[] playerStates = { };

        [Header("World State")]
        public string currentWeather = "clear";
        public float weatherIntensity = 0f;
        public string currentBiome = "wind_grassland";
        public int worldDay = 1;
        public float timeOfDay = 0.5f;

        [Header("Expedition State")]
        public bool inExpedition = false;
        public string expeditionId = "";
        public string expeditionPhase = "";
        public float expeditionTime = 0f;

        [Header("Social State")]
        public int activeSocialEvents = 0;
        public int activeWishes = 0;
        public int fulfilledWishes = 0;
    }
}
