using UnityEngine;

namespace Asteria.Planet.Creatures
{
    /// <summary>
    /// Defines a creature type with behavior, appearance, and interaction rules.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Creature Definition")]
    public sealed class CreatureDefinition : ScriptableObject
    {
        public string creatureId = "creature_default";
        public string displayName = "生物";
        [TextArea(1, 3)] public string description = "";

        [Header("Behavior")]
        public CreatureBehavior behavior = CreatureBehavior.Curious;
        public float moveSpeed = 3f;
        public float detectionRadius = 10f;
        public float fleeRadius = 5f;
        public float interactionRadius = 3f;

        [Header("Appearance")]
        public Color bodyColor = new(0.8f, 0.8f, 0.7f);
        public float scale = 1f;
        public string[] idleAnimations = { "idle", "look_around" };
        public string[] interactionAnimations = { "curious", "happy" };

        [Header("Spawning")]
        public string[] preferredBiomes = { };
        public float spawnWeight = 1f;
        public int maxGroupSize = 3;
        public float minSpawnAltitude = 0f;
        public float maxSpawnAltitude = 100f;

        [Header("Interaction")]
        public bool canBeFed = false;
        public bool canBePetted = false;
        public bool canBePhotographed = true;
        public string[] favoriteFoods = { };
        public float trustGainPerInteraction = 0.1f;
    }

    public enum CreatureBehavior
    {
        Curious,    // Approaches player
        Shy,        // Flees from player
        Group,      // Moves with group
        Symbiotic,  // Tied to plants/facilities
        Guide,      // Knows hidden paths
        Disturbing  // Changes tools/weather
    }
}
