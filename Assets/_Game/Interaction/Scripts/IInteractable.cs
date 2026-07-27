namespace Asteria.Interaction
{
    public interface IInteractable
    {
        string PromptText { get; }
        bool CanInteract { get; }
        void Interact(InteractionContext context);
    }

    public readonly struct InteractionContext
    {
        public readonly UnityEngine.Transform Actor;

        public InteractionContext(UnityEngine.Transform actor)
        {
            Actor = actor;
        }
    }
}
