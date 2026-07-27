using Asteria.Data;
using UnityEngine;

namespace Asteria.Interaction
{
    /// <summary>
    /// A restore interaction (e.g., repairing a wind tower).
    /// Multi-phase: player holds interact key to progress through stages.
    /// </summary>
    public sealed class RestoreInteractable : MonoBehaviour, IInteractable, ILongInteraction
    {
        [SerializeField] string restoreId = "restore_default";
        [SerializeField] string displayName = "修复";
        [TextArea(1, 3)] public string description = "";
        [SerializeField] float baseDuration = 5f;
        [SerializeField] int totalStages = 3;
        [SerializeField] bool oneShot = true;

        int _currentStage;
        bool _restored;
        InteractionInstance _activeInstance;

        public string PromptText => _restored ? $"已修复 · {displayName}" : $"按住 E 修复 · {displayName}";
        public bool CanInteract => !_restored;
        public string DisplayName => displayName;
        public float BaseDuration => baseDuration;
        public string RestoreId => restoreId;
        public bool IsRestored => _restored;

        public void Interact(InteractionContext context)
        {
            if (_restored || _activeInstance != null)
            {
                return;
            }

            _activeInstance = new InteractionInstance(this, context, baseDuration);
            _activeInstance.Start();
        }

        void Update()
        {
            if (_activeInstance == null)
            {
                return;
            }

            // Check if player is still holding interact
            bool holding = Input.GetKey(KeyCode.E);
            if (!holding)
            {
                _activeInstance.Cancel();
                _activeInstance = null;
                return;
            }

            bool active = _activeInstance.Tick(Time.deltaTime);
            if (!active)
            {
                _activeInstance = null;
            }
        }

        public void OnStart(InteractionContext context)
        {
            Debug.Log($"[Asteria] Restoring {displayName}...");
        }

        public void OnTick(InteractionContext context, float progress)
        {
            // Visual feedback: change color or scale based on progress
            var renderer = GetComponent<MeshRenderer>();
            if (renderer != null && renderer.material.HasProperty("_BaseColor"))
            {
                Color baseColor = renderer.material.GetColor("_BaseColor");
                Color targetColor = new(0.4f, 0.9f, 0.5f);
                renderer.material.SetColor("_BaseColor", Color.Lerp(baseColor, targetColor, progress));
            }
        }

        public void OnComplete(InteractionContext context)
        {
            _currentStage++;
            if (_currentStage >= totalStages)
            {
                _restored = true;
                GameHud.ShowToast($"已修复：{displayName}");
                Debug.Log($"[Asteria] {displayName} fully restored!");

                // Change appearance
                var renderer = GetComponent<MeshRenderer>();
                if (renderer != null && renderer.material.HasProperty("_BaseColor"))
                {
                    renderer.material.SetColor("_BaseColor", new Color(0.3f, 0.85f, 0.4f));
                }
            }
            else
            {
                GameHud.ShowToast($"修复进度：{_currentStage}/{totalStages}");
            }
        }

        public void OnCancel(InteractionContext context)
        {
            Debug.Log($"[Asteria] Restore of {displayName} cancelled.");
        }
    }
}
