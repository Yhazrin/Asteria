using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Asteria.Residents
{
    /// <summary>
    /// Manages resident wishes and their fulfillment.
    /// Wishes connect home life to expedition goals.
    /// </summary>
    public sealed class WishSystem : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float wishCheckInterval = 10f;
        [SerializeField] int maxActiveWishes = 6;

        [Header("Wishes")]
        [SerializeField] WishDefinition[] availableWishes;

        readonly List<WishInstance> _activeWishes = new();
        readonly List<WishInstance> _fulfilledWishes = new();
        readonly Dictionary<string, float> _wishCooldowns = new();
        float _checkTimer;

        void Update()
        {
            _checkTimer -= Time.deltaTime;
            if (_checkTimer <= 0f)
            {
                _checkTimer = wishCheckInterval;
                CheckForNewWishes();
            }
        }

        void CheckForNewWishes()
        {
            if (_activeWishes.Count >= maxActiveWishes) return;
            if (availableWishes == null || availableWishes.Length == 0) return;

            var manager = FindFirstObjectByType<ResidentManager>();
            if (manager == null) return;

            foreach (var agent in manager.Agents)
            {
                if (agent?.Definition == null || agent.State == null) continue;

                // Check if resident can have a new wish
                if (_activeWishes.Any(w => w.residentId == agent.Definition.ResidentId)) continue;

                // Find eligible wishes
                foreach (var wishDef in availableWishes)
                {
                    if (IsWishEligible(wishDef, agent))
                    {
                        CreateWish(wishDef, agent);
                        break; // One wish per resident per check
                    }
                }
            }
        }

        bool IsWishEligible(WishDefinition wishDef, ResidentAgent agent)
        {
            // Check cooldown
            string key = $"{agent.Definition.ResidentId}_{wishDef.wishId}";
            if (_wishCooldowns.TryGetValue(key, out float lastTime))
            {
                if (Time.time - lastTime < 300f) // 5 minute cooldown
                    return false;
            }

            // Check preconditions
            if (agent.State.affinity < wishDef.minAffinity) return false;
            if (agent.Definition.Curiosity < wishDef.minCuriosity) return false;

            // Check if already fulfilled
            if (_fulfilledWishes.Any(w => w.wishId == wishDef.wishId &&
                                          w.residentId == agent.Definition.ResidentId))
                return false;

            return true;
        }

        void CreateWish(WishDefinition wishDef, ResidentAgent agent)
        {
            var wish = new WishInstance
            {
                wishId = wishDef.wishId,
                residentId = agent.Definition.ResidentId,
                definition = wishDef,
                status = WishStatus.Active,
                createdTime = Time.time
            };

            _activeWishes.Add(wish);

            // Show wish dialogue
            var bubble = agent.GetComponentInChildren<ResidentDialogueBubble>();
            if (bubble != null)
            {
                bubble.ShowThought(wishDef.description);
            }

            Debug.Log($"[Wish] {agent.Definition.DisplayName} expressed wish: {wishDef.displayName}");
        }

        /// <summary>
        /// Check if a discovery fulfills any active wishes.
        /// </summary>
        public List<WishInstance> CheckFulfillment(string discoveryId)
        {
            var fulfilled = new List<WishInstance>();

            foreach (var wish in _activeWishes)
            {
                if (wish.status != WishStatus.Active) continue;

                if (wish.definition.requiredDiscoveryId == discoveryId)
                {
                    wish.status = WishStatus.Fulfilled;
                    fulfilled.Add(wish);

                    // Notify resident
                    var manager = FindFirstObjectByType<ResidentManager>();
                    if (manager != null)
                    {
                        var agent = manager.GetResident(wish.residentId);
                        if (agent != null)
                        {
                            var bubble = agent.GetComponentInChildren<ResidentDialogueBubble>();
                            if (bubble != null)
                            {
                                bubble.ShowDialogue(wish.definition.fulfillmentText);
                            }

                            // Improve relationship
                            agent.State.affinity += 0.15f;
                            agent.State.trust += 0.1f;
                        }
                    }

                    Debug.Log($"[Wish] Fulfilled: {wish.definition.displayName}");
                }
            }

            // Move fulfilled wishes
            foreach (var wish in fulfilled)
            {
                _activeWishes.Remove(wish);
                _fulfilledWishes.Add(wish);
                _wishCooldowns[$"{wish.residentId}_{wish.wishId}"] = Time.time;
            }

            return fulfilled;
        }

        /// <summary>
        /// Get all active wishes.
        /// </summary>
        public IReadOnlyList<WishInstance> GetActiveWishes()
        {
            return _activeWishes.AsReadOnly();
        }

        /// <summary>
        /// Get all fulfilled wishes.
        /// </summary>
        public IReadOnlyList<WishInstance> GetFulfilledWishes()
        {
            return _fulfilledWishes.AsReadOnly();
        }

        public enum WishStatus { Active, Fulfilled, Expired }

        public class WishInstance
        {
            public string wishId;
            public string residentId;
            public WishDefinition definition;
            public WishStatus status;
            public float createdTime;
        }
    }
}
