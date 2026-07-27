namespace Asteria.Interaction
{
    /// <summary>
    /// Abstracts the event director. Home and expedition use different implementations.
    /// </summary>
    public interface IEventDirector
    {
        /// <summary>Evaluate and potentially trigger the next event.</summary>
        void Evaluate();

        /// <summary>Force-trigger a specific event by ID.</summary>
        bool TriggerEvent(string eventId);

        /// <summary>True if an event is currently active.</summary>
        bool HasActiveEvent { get; }

        /// <summary>The currently active event ID, or null.</summary>
        string ActiveEventId { get; }

        /// <summary>Register a follow-up seed for later evaluation.</summary>
        void EnqueueFollowUp(string seedId, float delayDays);
    }
}
