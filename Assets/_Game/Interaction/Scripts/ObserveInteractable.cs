using Asteria.Audio;
using Asteria.Data;
using UnityEngine;

namespace Asteria.Interaction
{
    /// <summary>
    /// Observe point of interest. Press interact nearby to unlock a journal entry.
    /// </summary>
    public sealed class ObserveInteractable : MonoBehaviour, IInteractable
    {
        [SerializeField] ObserveEntry entry;
        [SerializeField] float focusDistance = 4.5f;
        [SerializeField] bool oneShot = true;

        bool _observed;
        bool _cachedHasEntry;
        bool _cacheValid;

        public ObserveEntry Entry
        {
            get => entry;
            set
            {
                entry = value;
                _cacheValid = false;
            }
        }

        public string PromptText
        {
            get
            {
                if (entry == null) return "按 E 观察";
                if (IsDiscovered()) return $"已观察 · {entry.displayName}";
                return string.IsNullOrWhiteSpace(entry.promptText)
                    ? $"按 E 观察 · {entry.displayName}"
                    : entry.promptText;
            }
        }

        public bool CanInteract
        {
            get
            {
                if (entry == null) return false;
                if (oneShot && IsDiscovered()) return false;
                return true;
            }
        }

        public float FocusDistance => focusDistance;

        bool IsDiscovered()
        {
            if (_observed) return true;
            if (!_cacheValid)
            {
                _cachedHasEntry = DiscoveryJournal.Instance.Has(entry);
                _cacheValid = true;
            }
            return _cachedHasEntry;
        }

        public void Interact(InteractionContext context)
        {
            if (!CanInteract) return;

            bool unlocked = DiscoveryJournal.Instance.TryUnlock(entry);
            _observed = true;
            _cacheValid = false;

            if (unlocked)
            {
                GameHud.ShowDiscovery(entry);
                AudioManager.Instance?.PlayDiscovery();
            }
            else
            {
                GameHud.ShowToast($"已记录：{entry.displayName}");
            }
        }
    }
}
