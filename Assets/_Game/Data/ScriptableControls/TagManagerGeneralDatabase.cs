using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// General tag manager database for the game.
    /// Contains all tags and layers.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Tag Manager General Database")]
    public sealed class TagManagerGeneralDatabase : ScriptableObject
    {
        [Header("Tags")]
        public string[] tags = {
            "Player",
            "Planet",
            "POI",
            "Resident",
            "Creature",
            "Facility",
            "Tool",
            "Beacon",
            "Shelter"
        };

        [Header("Layers")]
        public string[] layers = {
            "Default",
            "TransparentFX",
            "Ignore Raycast",
            "",
            "Water",
            "UI",
            "Planet",
            "Player",
            "Resident",
            "Creature",
            "POI",
            "Tool"
        };
    }
}
