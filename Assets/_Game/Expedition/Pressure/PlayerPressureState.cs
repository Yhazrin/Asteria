using System.Collections.Generic;
using UnityEngine;

namespace Asteria.Expedition
{
    /// <summary>
    /// Tracks the player's current pressure states during an expedition.
    /// States are temporary and fully recover after the pressure event ends.
    /// </summary>
    public sealed class PlayerPressureState : MonoBehaviour
    {
        readonly Dictionary<string, float> _activeStates = new();
        readonly Dictionary<string, float> _stateTimers = new();
        readonly List<string> _expiredBuffer = new();
        readonly List<string> _keysBuffer = new();

        /// <summary>True if the player is affected by any pressure.</summary>
        public bool IsUnderPressure => _activeStates.Count > 0;

        /// <summary>All currently active pressure state IDs.</summary>
        public IEnumerable<string> ActiveStates => _activeStates.Keys;

        /// <summary>
        /// Apply a pressure state. Duration is how long it lasts.
        /// </summary>
        public void ApplyState(string stateId, float duration, float intensity = 1f)
        {
            _activeStates[stateId] = intensity;
            _stateTimers[stateId] = duration;
            Debug.Log($"[Asteria] Pressure state applied: {stateId} ({duration}s)");
        }

        /// <summary>
        /// Remove a pressure state (e.g., from using a counter tool).
        /// </summary>
        public void RemoveState(string stateId)
        {
            _activeStates.Remove(stateId);
            _stateTimers.Remove(stateId);
            Debug.Log($"[Asteria] Pressure state removed: {stateId}");
        }

        /// <summary>
        /// Get the intensity of a pressure state (0 if not active).
        /// </summary>
        public float GetIntensity(string stateId)
        {
            return _activeStates.TryGetValue(stateId, out float intensity) ? intensity : 0f;
        }

        void Update()
        {
            _expiredBuffer.Clear();
            _keysBuffer.Clear();
            _keysBuffer.AddRange(_stateTimers.Keys);

            foreach (var key in _keysBuffer)
            {
                _stateTimers[key] -= Time.deltaTime;
                if (_stateTimers[key] <= 0f)
                {
                    _expiredBuffer.Add(key);
                }
            }

            foreach (var key in _expiredBuffer)
            {
                RemoveState(key);
            }
        }

        /// <summary>
        /// Apply effects of pressure states to the player's movement.
        /// Called by SphericalMotor or similar.
        /// </summary>
        public void ApplyMovementModifiers(ref float speed, ref float control)
        {
            if (_activeStates.ContainsKey("cold"))
            {
                speed *= 0.7f;
            }

            if (_activeStates.ContainsKey("unbalanced"))
            {
                control *= 0.5f;
            }
        }

        /// <summary>
        /// Check if the player has "fallen" (too much pressure).
        /// </summary>
        public bool HasFallen()
        {
            float totalIntensity = 0f;
            foreach (var intensity in _activeStates.Values)
            {
                totalIntensity += intensity;
            }

            return totalIntensity > AsteriaConstants.PressureFallThreshold;
        }

        /// <summary>
        /// Rescue the player (remove all pressure states).
        /// </summary>
        public void Rescue()
        {
            _activeStates.Clear();
            _stateTimers.Clear();
            Debug.Log("[Asteria] Player rescued. All pressure states cleared.");
        }
    }
}
