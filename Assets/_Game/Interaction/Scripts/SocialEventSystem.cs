using System.Collections.Generic;
using System.Linq;
using Asteria.Residents;
using UnityEngine;

namespace Asteria.Interaction
{
    /// <summary>
    /// Manages social events between residents.
    /// Triggers events based on conditions and handles outcomes.
    /// </summary>
    public sealed class SocialEventSystem : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float evaluationInterval = 5f;
        [SerializeField] int maxActiveEvents = 3;

        [Header("Events")]
        [SerializeField] SocialEventDefinition[] availableEvents;

        readonly List<SocialEventInstance> _activeEvents = new();
        readonly Dictionary<string, float> _cooldowns = new();
        float _evaluationTimer;

        void Update()
        {
            _evaluationTimer -= Time.deltaTime;
            if (_evaluationTimer <= 0f)
            {
                _evaluationTimer = evaluationInterval;
                EvaluateEvents();
            }

            UpdateActiveEvents();
        }

        void EvaluateEvents()
        {
            if (_activeEvents.Count >= maxActiveEvents) return;
            if (availableEvents == null || availableEvents.Length == 0) return;

            var manager = FindFirstObjectByType<ResidentManager>();
            if (manager == null || manager.Agents.Count < 2) return;

            // Find eligible events
            var eligible = new List<SocialEventDefinition>();
            foreach (var evt in availableEvents)
            {
                if (IsEventEligible(evt, manager))
                {
                    eligible.Add(evt);
                }
            }

            if (eligible.Count == 0) return;

            // Select random event
            var selected = eligible[Random.Range(0, eligible.Count)];
            TriggerEvent(selected, manager);
        }

        bool IsEventEligible(SocialEventDefinition evt, ResidentManager manager)
        {
            // Check cooldown
            if (_cooldowns.TryGetValue(evt.eventId, out float lastTime))
            {
                if (Time.time - lastTime < evt.cooldownDays * 60f) // Convert days to seconds for demo
                    return false;
            }

            // Check if already active
            foreach (var active in _activeEvents)
            {
                if (active.definition == evt) return false;
            }

            // Check participant count
            if (manager.Agents.Count < evt.minParticipants) return false;

            // Check personality preconditions
            if (evt.requiredPersonalityTags != null && evt.requiredPersonalityTags.Length > 0)
            {
                bool hasMatch = false;
                foreach (var agent in manager.Agents)
                {
                    if (MatchesPersonality(agent, evt.requiredPersonalityTags))
                    {
                        hasMatch = true;
                        break;
                    }
                }
                if (!hasMatch) return false;
            }

            return true;
        }

        bool MatchesPersonality(ResidentAgent agent, string[] tags)
        {
            if (agent?.Definition == null) return false;

            foreach (var tag in tags)
            {
                switch (tag)
                {
                    case "extroverted" when agent.Definition.Sociability > 0.3f:
                    case "introverted" when agent.Definition.Sociability < -0.3f:
                    case "curious" when agent.Definition.Curiosity > 0.3f:
                    case "warm" when agent.Definition.Warmth > 0.3f:
                    case "bold" when agent.Definition.Boldness > 0.3f:
                        return true;
                }
            }

            return false;
        }

        void TriggerEvent(SocialEventDefinition evt, ResidentManager manager)
        {
            // Select participants
            var participants = SelectParticipants(evt, manager);
            if (participants.Count < evt.minParticipants) return;

            var instance = new SocialEventInstance
            {
                definition = evt,
                participants = participants,
                startTime = Time.time,
                state = EventState.Active
            };

            _activeEvents.Add(instance);
            _cooldowns[evt.eventId] = Time.time;

            // Show event dialogue
            ShowEventDialogue(instance);

            Debug.Log($"[SocialEvent] Triggered: {evt.title} with {participants.Count} participants");
        }

        List<ResidentAgent> SelectParticipants(SocialEventDefinition evt, ResidentManager manager)
        {
            var agents = new List<ResidentAgent>(manager.Agents);
            var selected = new List<ResidentAgent>();

            // Shuffle
            for (int i = agents.Count - 1; i > 0; i--)
            {
                int j = Random.Range(0, i + 1);
                (agents[i], agents[j]) = (agents[j], agents[i]);
            }

            // Select based on personality match
            foreach (var agent in agents)
            {
                if (selected.Count >= evt.maxParticipants) break;

                if (evt.requiredPersonalityTags == null || evt.requiredPersonalityTags.Length == 0 ||
                    MatchesPersonality(agent, evt.requiredPersonalityTags))
                {
                    selected.Add(agent);
                }
            }

            return selected;
        }

        void ShowEventDialogue(SocialEventInstance instance)
        {
            if (instance.participants.Count == 0) return;

            var bubble = instance.participants[0].GetComponentInChildren<ResidentDialogueBubble>();
            if (bubble != null)
            {
                bubble.ShowDialogue(instance.definition.openingBeatDescription);
            }
        }

        void UpdateActiveEvents()
        {
            for (int i = _activeEvents.Count - 1; i >= 0; i--)
            {
                var evt = _activeEvents[i];
                float elapsed = Time.time - evt.startTime;

                // Auto-complete after some time
                if (elapsed > 10f) // 10 seconds for demo
                {
                    CompleteEvent(evt);
                    _activeEvents.RemoveAt(i);
                }
            }
        }

        void CompleteEvent(SocialEventInstance instance)
        {
            instance.state = EventState.Completed;

            // Apply relationship effects
            if (instance.definition.relationshipEffects != null)
            {
                foreach (var effect in instance.definition.relationshipEffects)
                {
                    // Apply to participants
                    if (instance.participants.Count >= 2)
                    {
                        var a = instance.participants[0];
                        var b = instance.participants[1];

                        if (a.State != null && b.State != null)
                        {
                            a.State.affinity += effect.affinityChange;
                            b.State.affinity += effect.affinityChange;
                            a.State.tension += effect.tensionChange;
                            b.State.tension += effect.tensionChange;
                        }
                    }
                }
            }

            // Create memory
            foreach (var participant in instance.participants)
            {
                if (participant.State != null)
                {
                    participant.State.memories.Add(new MemoryRecord
                    {
                        eventId = instance.definition.eventId,
                        timestamp = System.DateTime.UtcNow.ToString("o"),
                        participants = instance.participants.Select(p => p.Definition?.ResidentId).ToArray(),
                        location = "home",
                        emotionalTone = "happy",
                        tags = new[] { "social", instance.definition.category.ToString().ToLower() },
                        importance = 0.5f,
                        isPermanent = false
                    });
                }
            }

            Debug.Log($"[SocialEvent] Completed: {instance.definition.title}");
        }

        public enum EventState { Active, Completed, Failed }

        public class SocialEventInstance
        {
            public SocialEventDefinition definition;
            public List<ResidentAgent> participants;
            public float startTime;
            public EventState state;
        }
    }
}
