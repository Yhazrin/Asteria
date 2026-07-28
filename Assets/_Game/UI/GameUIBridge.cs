using Asteria.Data;
using Asteria.Interaction;
using UnityEngine;

namespace Asteria.UI
{
    /// <summary>
    /// Bridges game events to the UI system.
    /// Attach to the same GameObject as GameBootstrap.
    /// Replaces the old OnGUI-based GameHud.
    /// </summary>
    public sealed class GameUIBridge : MonoBehaviour
    {
        GameUIRoot _ui;

        void Start()
        {
            _ui = GameUIRoot.Instance;

            // Subscribe to discovery events
            var journal = DiscoveryJournal.Instance;
            if (journal != null)
            {
                journal.DiscoveryUnlocked += OnDiscoveryUnlocked;
            }
        }

        void Update()
        {
            if (_ui == null) return;

            // Update discovery count
            var journal = DiscoveryJournal.Instance;
            if (journal != null && _ui.HUD != null)
            {
                _ui.HUD.UpdateDiscoveryCount(journal.Count);
            }

            // Update day/time
            var clock = Core.GameBootstrap.Instance?.GameClock;
            if (clock != null && _ui.HUD != null)
            {
                _ui.HUD.UpdateDayTime(clock.WorldDay, clock.TimeOfDay);
            }

            // Update interaction prompt
            var detector = FindFirstObjectByType<InteractionDetector>();
            if (detector != null && _ui.InteractionPrompt != null)
            {
                if (detector.Current != null)
                {
                    _ui.InteractionPrompt.Show(detector.Current.PromptText);
                }
                else
                {
                    _ui.InteractionPrompt.Hide();
                }
            }

            // Tick discovery popup
            _ui.DiscoveryPopup?.Tick();
        }

        void OnDiscoveryUnlocked(ObserveEntry entry)
        {
            if (_ui?.DiscoveryPopup != null && entry != null)
            {
                _ui.DiscoveryPopup.Show(entry.displayName, entry.description);
            }
        }

        void OnDestroy()
        {
            var journal = DiscoveryJournal.Instance;
            if (journal != null)
            {
                journal.DiscoveryUnlocked -= OnDiscoveryUnlocked;
            }
        }
    }
}
