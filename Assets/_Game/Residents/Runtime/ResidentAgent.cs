using System.Collections.Generic;
using Asteria.Planet;
using UnityEngine;

namespace Asteria.Residents
{
    /// <summary>
    /// Runtime agent for a resident. Handles movement, schedule execution,
    /// and autonomous behavior on the spherical home planet.
    /// </summary>
    public sealed class ResidentAgent : MonoBehaviour
    {
        [SerializeField] ResidentDefinition definition;

        ResidentState _state;
        PlanetBody _planet;
        Transform _currentTarget;
        float _moveSpeed = 4f;
        float _rotationSpeed = 8f;
        float _scheduleTimer;
        float _interactionCooldown;
        int _currentScheduleIndex;

        public ResidentDefinition Definition => definition;
        public ResidentState State => _state;

        // Schedule: list of (activity name, destination tag)
        static readonly (string activity, string destTag)[] DefaultSchedule =
        {
            ("rest", "home"),
            ("social", "plaza"),
            ("explore", "observatory"),
            ("rest", "home"),
            ("social", "plaza"),
            ("solitary", "observatory"),
        };

        public void Initialize(ResidentDefinition def, PlanetBody planet, ResidentState savedState = null)
        {
            definition = def;
            _planet = planet;

            _state = savedState ?? new ResidentState
            {
                residentId = def.residentId,
                familiarity = 0.5f,
                affinity = 0.5f,
                trust = 0.5f,
                tension = 0f
            };

            // Start at a random position on the planet
            if (savedState == null)
            {
                Vector3 randomDir = Random.onUnitSphere;
                transform.position = planet.GetPointOnSurface(randomDir, 1.05f);
                planet.AlignTransformToSurface(transform, Random.onUnitSphere);
            }

            _scheduleTimer = 0f;
            _currentScheduleIndex = 0;
            AdvanceSchedule();
        }

        void Update()
        {
            if (_planet == null || definition == null)
            {
                return;
            }

            // Move toward current destination
            if (_currentTarget != null)
            {
                MoveToward(_currentTarget.position);
            }

            // Schedule timer
            _scheduleTimer -= Time.deltaTime;
            if (_scheduleTimer <= 0f)
            {
                AdvanceSchedule();
            }

            // Interaction cooldown
            if (_interactionCooldown > 0f)
            {
                _interactionCooldown -= Time.deltaTime;
            }
        }

        void MoveToward(Vector3 targetPos)
        {
            Vector3 up = _planet.GetSurfaceUp(transform.position);
            Vector3 toTarget = Vector3.ProjectOnPlane(targetPos - transform.position, up);

            if (toTarget.sqrMagnitude < 1f)
            {
                // Arrived
                return;
            }

            Vector3 moveDir = toTarget.normalized;
            transform.position += moveDir * (_moveSpeed * Time.deltaTime);

            // Re-project onto sphere surface
            Vector3 surfaceUp = _planet.GetSurfaceUp(transform.position);
            Vector3 surfacePos = _planet.GetPointOnSurface(surfaceUp, 1.05f);
            transform.position = Vector3.Lerp(transform.position, surfacePos, 10f * Time.deltaTime);

            // Rotate to face movement direction
            Quaternion targetRot = Quaternion.LookRotation(moveDir, surfaceUp);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, _rotationSpeed * Time.deltaTime);
        }

        void AdvanceSchedule()
        {
            if (DefaultSchedule.Length == 0)
            {
                return;
            }

            var (activity, destTag) = DefaultSchedule[_currentScheduleIndex % DefaultSchedule.Length];
            _currentScheduleIndex++;

            _state.currentActivity = activity;
            _state.currentDestination = destTag;

            // Find destination in scene
            _currentTarget = FindDestination(destTag);

            // Schedule duration: 30-90 seconds per activity
            _scheduleTimer = Random.Range(30f, 90f);

            // Update needs based on activity
            switch (activity)
            {
                case "rest":
                    _state.safety = Mathf.Min(1f, _state.safety + 0.1f);
                    _state.solitude = Mathf.Min(1f, _state.solitude + 0.05f);
                    break;
                case "social":
                    _state.social = Mathf.Min(1f, _state.social + 0.1f);
                    _state.expression = Mathf.Min(1f, _state.expression + 0.05f);
                    break;
                case "explore":
                    _state.exploration = Mathf.Min(1f, _state.exploration + 0.1f);
                    break;
                case "solitary":
                    _state.solitude = Mathf.Min(1f, _state.solitude + 0.15f);
                    break;
            }
        }

        // Cached destination lookups to avoid per-schedule GameObject.Find calls.
        static readonly Dictionary<string, string[]> DestinationSearchNames = new()
        {
            { "home", new[] { "ResidentialArea", "Home", "Residence" } },
            { "plaza", new[] { "Plaza", "Square", "CentralPlaza" } },
            { "observatory", new[] { "Observatory", "ObservationDeck" } },
        };

        Transform _fallbackDest;

        Transform FindDestination(string tag)
        {
            if (DestinationSearchNames.TryGetValue(tag, out var searchNames))
            {
                foreach (string name in searchNames)
                {
                    var go = GameObject.Find(name);
                    if (go != null)
                    {
                        return go.transform;
                    }
                }
            }
            else
            {
                var go = GameObject.Find(tag);
                if (go != null)
                {
                    return go.transform;
                }
            }

            // Reuse a single fallback transform instead of leaking GameObjects.
            if (_planet != null)
            {
                if (_fallbackDest == null)
                {
                    var fallbackGo = new GameObject($"Resident_{definition?.displayName}_FallbackDest");
                    _fallbackDest = fallbackGo.transform;
                }

                Vector3 dir = Random.onUnitSphere;
                _fallbackDest.position = _planet.GetPointOnSurface(dir, 1.05f);
                return _fallbackDest;
            }

            return null;
        }

        /// <summary>
        /// Attempt social interaction with another resident.
        /// </summary>
        public bool TryInteract(ResidentAgent other)
        {
            if (_interactionCooldown > 0f || other == null || other == this)
            {
                return false;
            }

            float distance = Vector3.Distance(transform.position, other.transform.position);
            if (distance > 5f)
            {
                return false;
            }

            // Determine interaction type based on personality
            string eventType = DetermineEventType(other);

            // Create memory
            var memory = new MemoryRecord
            {
                eventId = $"interact_{eventType}_{System.Guid.NewGuid().ToString("N")[..8]}",
                timestamp = System.DateTime.UtcNow.ToString("o"),
                participants = new[] { definition.residentId, other.definition.residentId },
                location = _state.currentDestination,
                emotionalTone = eventType == "conflict" ? "tense" : "happy",
                tags = new[] { eventType, _state.currentActivity },
                importance = 0.5f,
                isPermanent = false
            };

            _state.memories.Add(memory);
            other.State.memories.Add(memory);

            // Adjust relationship
            if (eventType == "friendly")
            {
                _state.affinity = Mathf.Min(1f, _state.affinity + 0.05f);
                other.State.affinity = Mathf.Min(1f, other.State.affinity + 0.05f);
                _state.tension = Mathf.Max(0f, _state.tension - 0.02f);
            }
            else if (eventType == "conflict")
            {
                _state.tension = Mathf.Min(1f, _state.tension + 0.1f);
                other.State.tension = Mathf.Min(1f, other.State.tension + 0.1f);
            }

            _interactionCooldown = 15f;
            other._interactionCooldown = 15f;

            Debug.Log($"[Asteria] {definition.displayName} and {other.definition.displayName}: {eventType}");
            return true;
        }

        string DetermineEventType(ResidentAgent other)
        {
            if (definition == null || other.definition == null)
            {
                return "neutral";
            }

            // High warmth + high sociability = friendly
            float friendliness = (definition.warmth + definition.sociability +
                                  other.definition.warmth + other.definition.sociability) / 4f;

            // High tension + different order = conflict prone
            float conflictProne = (_state.tension + other.State.tension) / 2f +
                                  Mathf.Abs(definition.order - other.definition.order) * 0.3f;

            if (conflictProne > 0.6f && Random.value < 0.3f)
            {
                return "conflict";
            }

            if (friendliness > 0.3f)
            {
                return "friendly";
            }

            return "neutral";
        }

        /// <summary>
        /// Record a wish (player-triggered suggestion).
        /// </summary>
        public void RecordWish(string wishDescription)
        {
            var memory = new MemoryRecord
            {
                eventId = $"wish_{System.Guid.NewGuid().ToString("N")[..8]}",
                timestamp = System.DateTime.UtcNow.ToString("o"),
                participants = new[] { definition.residentId },
                location = _state.currentDestination,
                emotionalTone = "curious",
                tags = new[] { "wish", "player_suggestion" },
                importance = 0.8f,
                isPermanent = true
            };

            _state.memories.Add(memory);
            Debug.Log($"[Asteria] {definition.displayName} received wish: {wishDescription}");
        }
    }
}
