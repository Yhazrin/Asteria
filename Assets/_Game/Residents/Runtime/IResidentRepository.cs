using System.Collections.Generic;

namespace Asteria.Residents
{
    /// <summary>
    /// Persistent storage for resident states. Backed by save service.
    /// </summary>
    public interface IResidentRepository
    {
        /// <summary>Get a resident state by ID, or null if not found.</summary>
        ResidentState Get(string residentId);

        /// <summary>Get all resident states.</summary>
        IReadOnlyList<ResidentState> GetAll();

        /// <summary>Save a resident state.</summary>
        void Save(ResidentState state);

        /// <summary>Remove a resident state.</summary>
        void Remove(string residentId);

        /// <summary>Total resident count.</summary>
        int Count { get; }
    }
}
