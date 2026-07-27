using System;
using UnityEngine;

namespace Asteria.Interaction
{
    /// <summary>
    /// State machine for long-duration interactions (Restore, Cooperate).
    /// Replaces single Interact() call with multi-phase progress.
    /// </summary>
    public sealed class InteractionInstance
    {
        public enum State { Ready, InProgress, Completing, Completed, Failed, Cancelled }

        readonly ILongInteraction _interaction;
        readonly InteractionContext _context;
        State _state;
        float _progress;
        float _duration;

        public State CurrentState => _state;
        public float Progress => _progress;
        public float Duration => _duration;
        public ILongInteraction Interaction => _interaction;

        public event Action<State> OnStateChanged;
        public event Action<float> OnProgressChanged;

        public InteractionInstance(ILongInteraction interaction, InteractionContext context, float duration)
        {
            _interaction = interaction;
            _context = context;
            _duration = duration;
            _state = State.Ready;
            _progress = 0f;
        }

        public void Start()
        {
            if (_state != State.Ready)
            {
                return;
            }

            _state = State.InProgress;
            _interaction.OnStart(_context);
            OnStateChanged?.Invoke(_state);
        }

        /// <summary>
        /// Tick the interaction. Returns true if still active.
        /// </summary>
        public bool Tick(float deltaTime)
        {
            if (_state != State.InProgress)
            {
                return _state == State.Completing;
            }

            _progress += deltaTime / _duration;
            _progress = Mathf.Clamp01(_progress);
            OnProgressChanged?.Invoke(_progress);

            _interaction.OnTick(_context, _progress);

            if (_progress >= 1f)
            {
                _state = State.Completing;
                _interaction.OnComplete(_context);
                _state = State.Completed;
                OnStateChanged?.Invoke(_state);
                return false;
            }

            return true;
        }

        public void Cancel()
        {
            if (_state == State.InProgress || _state == State.Completing)
            {
                _state = State.Cancelled;
                _interaction.OnCancel(_context);
                OnStateChanged?.Invoke(_state);
            }
        }
    }

    /// <summary>
    /// Interface for interactions that take time (Restore, Cooperate).
    /// </summary>
    public interface ILongInteraction
    {
        string DisplayName { get; }
        float BaseDuration { get; }
        void OnStart(InteractionContext context);
        void OnTick(InteractionContext context, float progress);
        void OnComplete(InteractionContext context);
        void OnCancel(InteractionContext context);
    }
}
