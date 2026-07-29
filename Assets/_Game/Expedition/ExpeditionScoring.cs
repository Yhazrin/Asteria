using System;
using System.Collections.Generic;
using UnityEngine;

namespace Asteria.Expedition
{
    /// <summary>
    /// Calculates expedition score and generates summary.
    /// Tracks discoveries, rescues, time, and cooperation.
    /// </summary>
    public sealed class ExpeditionScoring : MonoBehaviour
    {
        [Header("Scoring Weights")]
        [SerializeField] float discoveryWeight = 10f;
        [SerializeField] float restoreWeight = 20f;
        [SerializeField] float cooperateWeight = 30f;
        [SerializeField] float rescueWeight = 15f;
        [SerializeField] float timeBonus = 5f;
        [SerializeField] float timePenalty = -2f;

        // Tracking
        int _discoveries;
        int _restores;
        int _cooperates;
        int _rescues;
        float _startTime;
        bool _isExpeditionActive;

        // Detailed log
        readonly List<ExpeditionEvent> _events = new();

        public int Discoveries => _discoveries;
        public int Restores => _restores;
        public int Cooperates => _cooperates;
        public int Rescues => _rescues;

        /// <summary>
        /// Start tracking a new expedition.
        /// </summary>
        public void StartExpedition()
        {
            _discoveries = 0;
            _restores = 0;
            _cooperates = 0;
            _rescues = 0;
            _startTime = Time.time;
            _isExpeditionActive = true;
            _events.Clear();

            Debug.Log("[Scoring] Expedition scoring started.");
        }

        /// <summary>
        /// Record a discovery.
        /// </summary>
        public void RecordDiscovery(string discoveryId, string name)
        {
            if (!_isExpeditionActive) return;

            _discoveries++;
            _events.Add(new ExpeditionEvent
            {
                type = "discovery",
                name = name,
                time = Time.time - _startTime,
                points = discoveryWeight
            });

            Debug.Log($"[Scoring] Discovery: {name} (+{discoveryWeight} points)");
        }

        /// <summary>
        /// Record a restore.
        /// </summary>
        public void RecordRestore(string restoreId, string name)
        {
            if (!_isExpeditionActive) return;

            _restores++;
            _events.Add(new ExpeditionEvent
            {
                type = "restore",
                name = name,
                time = Time.time - _startTime,
                points = restoreWeight
            });

            Debug.Log($"[Scoring] Restore: {name} (+{restoreWeight} points)");
        }

        /// <summary>
        /// Record a cooperate.
        /// </summary>
        public void RecordCooperate(string cooperateId, string name)
        {
            if (!_isExpeditionActive) return;

            _cooperates++;
            _events.Add(new ExpeditionEvent
            {
                type = "cooperate",
                name = name,
                time = Time.time - _startTime,
                points = cooperateWeight
            });

            Debug.Log($"[Scoring] Cooperate: {name} (+{cooperateWeight} points)");
        }

        /// <summary>
        /// Record a rescue.
        /// </summary>
        public void RecordRescue(string rescuerId, string rescuedId)
        {
            if (!_isExpeditionActive) return;

            _rescues++;
            _events.Add(new ExpeditionEvent
            {
                type = "rescue",
                name = $"{rescuerId} rescued {rescuedId}",
                time = Time.time - _startTime,
                points = rescueWeight
            });

            Debug.Log($"[Scoring] Rescue: {rescuerId} -> {rescuedId} (+{rescueWeight} points)");
        }

        /// <summary>
        /// End the expedition and calculate final score.
        /// </summary>
        public ExpeditionResult EndExpedition()
        {
            if (!_isExpeditionActive) return null;

            _isExpeditionActive = false;
            float duration = Time.time - _startTime;

            // Calculate score
            float baseScore = _discoveries * discoveryWeight
                            + _restores * restoreWeight
                            + _cooperates * cooperateWeight
                            + _rescues * rescueWeight;

            // Time bonus/penalty
            float timeScore = 0f;
            if (duration < 1200f) // Under 20 minutes
                timeScore = timeBonus;
            else if (duration > 2400f) // Over 40 minutes
                timeScore = timePenalty;

            float totalScore = baseScore + timeScore;

            // Determine outcome
            string outcome = totalScore switch
            {
                >= 100 => "perfect",
                >= 60 => "success",
                >= 30 => "partial",
                _ => "minimal"
            };

            // Create result
            var result = new ExpeditionResult
            {
                expeditionId = Guid.NewGuid().ToString("N")[..8],
                durationSeconds = duration,
                discoveredIds = new List<string>(),
                restoredIds = new List<string>(),
                cooperatedIds = new List<string>(),
                rescueCount = _rescues,
                outcomeType = outcome
            };

            // Populate IDs from events
            foreach (var evt in _events)
            {
                switch (evt.type)
                {
                    case "discovery": result.discoveredIds.Add(evt.name); break;
                    case "restore": result.restoredIds.Add(evt.name); break;
                    case "cooperate": result.cooperatedIds.Add(evt.name); break;
                }
            }

            Debug.Log($"[Scoring] Expedition ended. Score: {totalScore:F0}, Outcome: {outcome}");
            return result;
        }

        /// <summary>
        /// Get the current score summary.
        /// </summary>
        public ScoreSummary GetCurrentSummary()
        {
            float duration = _isExpeditionActive ? Time.time - _startTime : 0f;

            return new ScoreSummary
            {
                discoveries = _discoveries,
                restores = _restores,
                cooperates = _cooperates,
                rescues = _rescues,
                duration = duration,
                events = new List<ExpeditionEvent>(_events)
            };
        }

        public struct ExpeditionEvent
        {
            public string type;
            public string name;
            public float time;
            public float points;
        }

        public struct ScoreSummary
        {
            public int discoveries;
            public int restores;
            public int cooperates;
            public int rescues;
            public float duration;
            public List<ExpeditionEvent> events;
        }
    }
}
