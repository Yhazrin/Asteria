using UnityEngine;

namespace Asteria.Interaction
{
    /// <summary>
    /// Defines a social event that can occur between residents on the home planet.
    /// Events are data-driven and selected by the event director based on conditions.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Social Event Definition")]
    public sealed class SocialEventDefinition : ScriptableObject
    {
        public string eventId = "social_default";
        public string title = "日常事件";
        [TextArea(2, 4)] public string description = "";
        public EventCategory category = EventCategory.Daily;

        [Header("Preconditions")]
        public int minParticipants = 2;
        public int maxParticipants = 2;
        public string[] requiredPersonalityTags = { };
        public string[] requiredRelationshipTags = { };
        public string[] requiredMemoryTags = { };
        public string[] requiredLocationTags = { };
        public string[] requiredWeatherTags = { };

        [Header("Content")]
        [TextArea(2, 4)] public string openingBeatDescription = "";
        public PlayerInterventionOption[] playerOptions = { };
        public AutonomousOutcome[] autonomousOutcomes = { };

        [Header("Effects")]
        public RelationshipEffect[] relationshipEffects = { };
        public string[] followUpSeedIds = { };

        [Header("Constraints")]
        public float cooldownDays = 1f;
        public bool isUnique = false;
    }

    public enum EventCategory
    {
        Daily,
        Relationship,
        Conflict,
        Community,
        ExpeditionFollowUp,
        Surprise
    }

    [System.Serializable]
    public class PlayerInterventionOption
    {
        public string optionId;
        public string displayText;
        public string[] effects;
    }

    [System.Serializable]
    public class AutonomousOutcome
    {
        public string outcomeId;
        public string description;
        public float weight = 1f;
    }

    [System.Serializable]
    public class RelationshipEffect
    {
        public string participantA;
        public string participantB;
        public float affinityChange;
        public float trustChange;
        public float tensionChange;
    }
}
