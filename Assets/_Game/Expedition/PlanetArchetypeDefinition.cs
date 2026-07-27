using UnityEngine;

namespace Asteria.Expedition
{
    /// <summary>
    /// Defines a planet archetype for expedition generation.
    /// Contains biome layout, POI slots, and event deck.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Planet Archetype Definition")]
    public sealed class PlanetArchetypeDefinition : ScriptableObject
    {
        public string archetypeId = "archetype_default";
        public string displayName = "星球原型";
        [TextArea(2, 4)] public string description = "";

        [Header("Planet")]
        public float planetRadius = 300f;

        [Header("Biomes")]
        public BiomeDefinition[] biomes = { };

        [Header("POI Slots")]
        public PoiSlotDefinition[] poiSlots = { };

        [Header("Event Deck")]
        public EventDeckEntry[] eventDeck = { };

        [Header("Requirements")]
        public string[] requiredTools = { };
    }

    [System.Serializable]
    public class PoiSlotDefinition
    {
        public string slotId;
        public string poiType;
        public SerializableVector3 localDirection;
        public string[] contentTags;
    }

    [System.Serializable]
    public class EventDeckEntry
    {
        public string eventId;
        public float weight = 1f;
        public ExpeditionPhase phase;
    }

    [System.Serializable]
    public struct SerializableVector3
    {
        public float x, y, z;
        public Vector3 ToVector3() => new Vector3(x, y, z);
        public static SerializableVector3 From(Vector3 v) => new SerializableVector3 { x = v.x, y = v.y, z = v.z };
    }
}
