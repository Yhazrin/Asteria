using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Asteria.Expedition
{
    /// <summary>
    /// Minimal event director for Milestone E. Manages pressure events
    /// during an expedition based on simple timing and conditions.
    /// </summary>
    public sealed class EventDirectorMinimal : MonoBehaviour
    {
        [SerializeField] PressureDefinition[] availablePressures;

        readonly List<PressureDefinition> _cooldownRegistry = new();
        float _evaluationTimer;
        float _expeditionTime;
        bool _pressureActive;

        public bool IsPressureActive => _pressureActive;

        void Update()
        {
            _expeditionTime += Time.deltaTime;

            _evaluationTimer -= Time.deltaTime;
            if (_evaluationTimer <= 0f)
            {
                _evaluationTimer = 2f; // Evaluate every 2 seconds
                Evaluate();
            }
        }

        void Evaluate()
        {
            if (_pressureActive || availablePressures == null || availablePressures.Length == 0)
            {
                return;
            }

            if (_expeditionTime < AsteriaConstants.PressureTriggerDelay)
            {
                return;
            }

            // Find a pressure that's not on cooldown
            var candidate = availablePressures.FirstOrDefault(p => !_cooldownRegistry.Contains(p));
            if (candidate == null)
            {
                return;
            }

            TriggerPressure(candidate);
        }

        void TriggerPressure(PressureDefinition pressure)
        {
            _pressureActive = true;
            _cooldownRegistry.Add(pressure);

            Debug.Log($"[Asteria] Pressure event: {pressure.displayName} ({pressure.activeDurationSeconds}s)");

            // Apply to all players in the scene
            var players = FindObjectsByType<PlayerPressureState>(FindObjectsSortMode.None);
            foreach (var player in players)
            {
                foreach (var state in pressure.affectedStates)
                {
                    player.ApplyState(state, pressure.activeDurationSeconds);
                }
            }

            // Auto-end after duration
            Invoke(nameof(EndPressure), pressure.activeDurationSeconds);
        }

        void EndPressure()
        {
            _pressureActive = false;

            // Remove all pressure states from players
            var players = FindObjectsByType<PlayerPressureState>(FindObjectsSortMode.None);
            foreach (var player in players)
            {
                player.Rescue();
            }

            Debug.Log("[Asteria] Pressure event ended. All players recovered.");
        }
    }
}
