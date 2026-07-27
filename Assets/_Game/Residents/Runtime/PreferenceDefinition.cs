using UnityEngine;

namespace Asteria.Residents
{
    /// <summary>
    /// Defines a resident's preferences for biomes, weather, activities, gifts, and creatures.
    /// Used by the event director and schedule system.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Preference Definition")]
    public sealed class PreferenceDefinition : ScriptableObject
    {
        public string preferenceId = "pref_default";

        [Header("Biomes")]
        public string[] likedBiomes = { };
        public string[] dislikedBiomes = { };

        [Header("Weather")]
        public string[] likedWeather = { };
        public string[] dislikedWeather = { };

        [Header("Activities")]
        public string[] likedActivities = { };

        [Header("Gifts")]
        public string[] giftPreferences = { };

        [Header("Creatures")]
        public string[] creaturePreferences = { };
    }
}
