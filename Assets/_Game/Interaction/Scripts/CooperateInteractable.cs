using UnityEngine;

namespace Asteria.Interaction
{
    /// <summary>
    /// A cooperate interaction that requires multiple players to complete.
    /// Example: bipolar resonance mechanism (two players on opposite sides).
    /// </summary>
    public sealed class CooperateInteractable : MonoBehaviour, IInteractable, ILongInteraction
    {
        [SerializeField] string cooperateId = "cooperate_default";
        [SerializeField] string displayName = "共鸣";
        [TextArea(1, 3)] public string description = "";
        [SerializeField] float baseDuration = 8f;
        [SerializeField] int requiredPlayers = 2;
        [SerializeField] bool oneShot = true;

        bool _completed;
        InteractionInstance _activeInstance;
        int _currentParticipants;

        public string PromptText => _completed ? $"已完成 · {displayName}" : $"需要 {requiredPlayers} 人 · {displayName}";
        public bool CanInteract => !_completed && _currentParticipants < requiredPlayers;
        public string DisplayName => displayName;
        public float BaseDuration => baseDuration;
        public string CooperateId => cooperateId;
        public bool IsCompleted => _completed;

        public void Interact(InteractionContext context)
        {
            if (_completed)
            {
                return;
            }

            _currentParticipants++;

            if (_currentParticipants >= requiredPlayers)
            {
                // All participants ready, start the interaction
                _activeInstance = new InteractionInstance(this, context, baseDuration);
                _activeInstance.Start();
            }
            else
            {
                GameHud.ShowToast($"等待其他玩家... ({_currentParticipants}/{requiredPlayers})");
            }
        }

        void Update()
        {
            if (_activeInstance == null)
            {
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
            Debug.Log($"[Asteria] Cooperate started: {displayName}");
        }

        public void OnTick(InteractionContext context, float progress)
        {
            // Visual feedback: glow increases with progress
            var renderer = GetComponent<MeshRenderer>();
            if (renderer != null && renderer.material.HasProperty("_EmissionColor"))
            {
                Color emission = Color.Lerp(Color.black, new Color(0.5f, 0.8f, 1f), progress);
                renderer.material.SetColor("_EmissionColor", emission);
            }
        }

        public void OnComplete(InteractionContext context)
        {
            _completed = true;
            GameHud.ShowToast($"共鸣完成：{displayName}");
            Debug.Log($"[Asteria] Cooperate completed: {displayName}");

            // Trigger global effect (e.g., aurora, unlock path)
            OnCooperateCompleted();
        }

        public void OnCancel(InteractionContext context)
        {
            _currentParticipants = Mathf.Max(0, _currentParticipants - 1);
            Debug.Log($"[Asteria] Cooperate cancelled: {displayName}");
        }

        void OnCooperateCompleted()
        {
            // Example: change the skybox color to simulate aurora
            RenderSettings.ambientSkyColor = new Color(0.3f, 0.6f, 0.8f);
        }
    }
}
