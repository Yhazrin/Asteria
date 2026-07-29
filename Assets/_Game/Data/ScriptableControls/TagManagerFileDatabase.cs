using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Tag manager file database for the game.
    /// Contains all tags and layers.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Tag Manager File Database")]
    public sealed class TagManagerFileDatabase : ScriptableObject
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
