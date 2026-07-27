using System;
using System.Collections.Generic;
using Asteria.Data;
using UnityEngine;

namespace Asteria.Interaction
{
    /// <summary>
    /// Minimal in-memory discovery journal for Phase 1.
    /// </summary>
    public sealed class DiscoveryJournal : MonoBehaviour
    {
        static DiscoveryJournal _instance;

        readonly HashSet<string> _unlocked = new HashSet<string>();

        public static DiscoveryJournal Instance
        {
            get
            {
                if (_instance != null)
                {
                    return _instance;
                }

                _instance = FindFirstObjectByType<DiscoveryJournal>();
                if (_instance != null)
                {
                    return _instance;
                }

                var go = new GameObject("DiscoveryJournal");
                _instance = go.AddComponent<DiscoveryJournal>();
                return _instance;
            }
        }

        public event Action<ObserveEntry> DiscoveryUnlocked;

        public int Count => _unlocked.Count;

        public bool Has(ObserveEntry entry)
        {
            return entry != null && _unlocked.Contains(entry.id);
        }

        public bool TryUnlock(ObserveEntry entry)
        {
            if (entry == null || string.IsNullOrWhiteSpace(entry.id))
            {
                return false;
            }

            if (!entry.IsIdValid())
            {
                Debug.LogWarning($"[Asteria] Rejecting ObserveEntry with invalid ID: '{entry.id}'");
                return false;
            }

            if (!_unlocked.Add(entry.id))
            {
                return false;
            }

            DiscoveryUnlocked?.Invoke(entry);
            Debug.Log($"[Asteria] Discovered: {entry.displayName}");
            return true;
        }

        void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
        }
    }
}
