using System.Collections.Generic;
using UnityEngine;

namespace Asteria.Planet.Creatures
{
    /// <summary>
    /// Runtime AI agent for creatures. Handles movement, behavior, and player interaction.
    /// </summary>
    public sealed class CreatureAgent : MonoBehaviour
    {
        [Header("Definition")]
        [SerializeField] CreatureDefinition definition;

        [Header("State")]
        [SerializeField] CreatureState _state = CreatureState.Idle;

        // Runtime
        PlanetBody _planet;
        Transform _player;
        float _stateTimer;
        float _interactionCooldown;
        float _trustLevel;
        Vector3 _homePosition;
        Vector3 _targetPosition;
        readonly List<CreatureAgent> _nearbyCreatures = new();

        public CreatureDefinition Definition => definition;
        public CreatureState CurrentState => _state;
        public float TrustLevel => _trustLevel;

        public void Initialize(CreatureDefinition def, PlanetBody planet)
        {
            definition = def;
            _planet = planet;
            _homePosition = transform.position;
            _trustLevel = 0f;
        }

        void Update()
        {
            if (definition == null || _planet == null) return;

            UpdateNearbyCreatures();
            UpdateBehavior();
            UpdateMovement();
            UpdateInteraction();
        }

        void UpdateNearbyCreatures()
        {
            _nearbyCreatures.Clear();
            var allCreatures = FindObjectsByType<CreatureAgent>(FindObjectsSortMode.None);
            foreach (var creature in allCreatures)
            {
                if (creature == this) continue;
                float dist = Vector3.Distance(transform.position, creature.transform.position);
                if (dist < definition.detectionRadius)
                {
                    _nearbyCreatures.Add(creature);
                }
            }
        }

        void UpdateBehavior()
        {
            _stateTimer -= Time.deltaTime;
            _interactionCooldown -= Time.deltaTime;

            // Find player
            if (_player == null)
            {
                var playerBody = FindFirstObjectByType<Player.SphericalGravityBody>();
                _player = playerBody?.transform;
            }

            float playerDistance = _player != null
                ? Vector3.Distance(transform.position, _player.position)
                : float.MaxValue;

            // State machine
            switch (_state)
            {
                case CreatureState.Idle:
                    if (playerDistance < definition.detectionRadius)
                    {
                        _state = definition.behavior switch
                        {
                            CreatureBehavior.Curious => CreatureState.Approaching,
                            CreatureBehavior.Shy => CreatureState.Fleeing,
                            CreatureBehavior.Guide => CreatureState.Guiding,
                            _ => CreatureState.Idle
                        };
                    }
                    else if (_stateTimer <= 0f)
                    {
                        // Random wander
                        _targetPosition = GetRandomNearbyPosition(5f);
                        _state = CreatureState.Wandering;
                        _stateTimer = Random.Range(3f, 8f);
                    }
                    break;

                case CreatureState.Wandering:
                    if (_stateTimer <= 0f)
                    {
                        _state = CreatureState.Idle;
                        _stateTimer = Random.Range(2f, 5f);
                    }
                    break;

                case CreatureState.Approaching:
                    if (playerDistance > definition.detectionRadius * 1.5f)
                    {
                        _state = CreatureState.Idle;
                    }
                    else if (playerDistance < definition.interactionRadius)
                    {
                        _state = CreatureState.Interacting;
                        _stateTimer = 3f;
                    }
                    break;

                case CreatureState.Fleeing:
                    if (playerDistance > definition.detectionRadius * 2f)
                    {
                        _state = CreatureState.Idle;
                        _stateTimer = Random.Range(3f, 6f);
                    }
                    break;

                case CreatureState.Interacting:
                    if (_stateTimer <= 0f)
                    {
                        _state = CreatureState.Idle;
                        _stateTimer = Random.Range(2f, 4f);
                    }
                    break;

                case CreatureState.Guiding:
                    // Guide creatures lead player to hidden paths
                    if (playerDistance > definition.detectionRadius * 2f)
                    {
                        _state = CreatureState.Idle;
                    }
                    break;
            }
        }

        void UpdateMovement()
        {
            Vector3 up = _planet.GetSurfaceUp(transform.position);
            Vector3 moveTarget = Vector3.zero;

            switch (_state)
            {
                case CreatureState.Wandering:
                    moveTarget = _targetPosition;
                    break;

                case CreatureState.Approaching:
                    if (_player != null)
                    {
                        moveTarget = _player.position;
                    }
                    break;

                case CreatureState.Fleeing:
                    if (_player != null)
                    {
                        Vector3 awayDir = (transform.position - _player.position).normalized;
                        moveTarget = transform.position + awayDir * 10f;
                    }
                    break;

                case CreatureState.Guiding:
                    // Move in a direction, occasionally looking back
                    moveTarget = _homePosition + transform.forward * 20f;
                    break;
            }

            if (moveTarget != Vector3.zero)
            {
                Vector3 direction = Vector3.ProjectOnPlane(moveTarget - transform.position, up).normalized;
                float speed = definition.moveSpeed * Time.deltaTime;

                // Apply movement on sphere surface
                transform.position += direction * speed;
                transform.position = _planet.GetPointOnSurface(
                    _planet.GetSurfaceUp(transform.position), 1f);

                // Rotate to face movement direction
                if (direction.sqrMagnitude > 0.001f)
                {
                    Quaternion targetRot = Quaternion.LookRotation(direction, up);
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRot,
                        5f * Time.deltaTime);
                }
            }
        }

        void UpdateInteraction()
        {
            if (_interactionCooldown > 0f) return;
            if (_player == null) return;

            float playerDistance = Vector3.Distance(transform.position, _player.position);
            if (playerDistance > definition.interactionRadius) return;

            // Check if player is interacting (pressing E)
            if (Input.GetKeyDown(KeyCode.E))
            {
                InteractWithPlayer();
            }
        }

        void InteractWithPlayer()
        {
            _interactionCooldown = 2f;
            _trustLevel += definition.trustGainPerInteraction;
            _trustLevel = Mathf.Clamp01(_trustLevel);

            // Visual feedback
            ShowReaction();

            Debug.Log($"[Creature] {definition.displayName} interacted. Trust: {_trustLevel:F2}");
        }

        void ShowReaction()
        {
            // Could trigger animation, particle, or dialogue bubble
            // For now, just scale bounce
            StartCoroutine(BounceAnimation());
        }

        System.Collections.IEnumerator BounceAnimation()
        {
            Vector3 originalScale = transform.localScale;
            Vector3 bigScale = originalScale * 1.3f;

            float duration = 0.3f;
            float elapsed = 0f;

            // Scale up
            while (elapsed < duration * 0.5f)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / (duration * 0.5f);
                transform.localScale = Vector3.Lerp(originalScale, bigScale, t);
                yield return null;
            }

            // Scale down
            elapsed = 0f;
            while (elapsed < duration * 0.5f)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / (duration * 0.5f);
                transform.localScale = Vector3.Lerp(bigScale, originalScale, t);
                yield return null;
            }

            transform.localScale = originalScale;
        }

        Vector3 GetRandomNearbyPosition(float radius)
        {
            Vector3 randomDir = Random.onUnitSphere;
            return _planet.GetPointOnSurface(
                (_planet.GetSurfaceUp(transform.position) + randomDir * 0.3f).normalized, 1f);
        }

        public enum CreatureState
        {
            Idle,
            Wandering,
            Approaching,
            Fleeing,
            Interacting,
            Guiding,
            Sleeping
        }
    }
}
