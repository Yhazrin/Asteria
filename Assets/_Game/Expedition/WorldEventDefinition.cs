using UnityEngine;

namespace Asteria.Expedition
{
    /// <summary>
    /// Defines a world event that occurs during an expedition.
    /// Selected by the event director based on phase, biome, and conditions.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/World Event Definition")]
    public sealed class WorldEventDefinition : ScriptableObject
    {
        public string eventId = "world_default";
        public string title = "世界事件";
        [TextArea(2, 4)] public string description = "";

        [Header("Tags")]
        public string[] biomeTags = { };
        public string[] moodTags = { };
        public string[] requiredPoiTypes = { };

        [Header("Participants")]
        public int minPlayers = 1;
        public int maxPlayers = 4;
        public string[] requiredResidentTraits = { };

        [Header("Timing")]
        public ExpeditionPhase phase = ExpeditionPhase.Arrival;
        public float durationMinSeconds = 60f;
        public float durationMaxSeconds = 180f;

        [Header("Conditions")]
        public string[] worldStateConditions = { };

        [Header("Setup")]
        public EventSetupAction[] setupActions = { };
        public EventObjective[] runtimeObjectives = { };

        [Header("Outcomes")]
        public EventOutcome successOutcome;
        public EventOutcome partialOutcome;
        public string[] followUpSeeds = { };
        public float cooldownDays = 1f;
    }

    public enum ExpeditionPhase
    {
        Arrival,
        Invitation,
        Complication,
        Pressure,
        Resolution,
        Aftermath
    }

    [System.Serializable]
    public class EventSetupAction
    {
        public string actionType;
        public string targetId;
        public string[] parameters;
    }

    [System.Serializable]
    public class EventObjective
    {
        public string objectiveId;
        public string description;
        public string completionCondition;
    }

    [System.Serializable]
    public class EventOutcome
    {
        public string outcomeId;
        public string description;
        public string[] rewardIds;
        public string[] followUpSeeds;
    }
}
