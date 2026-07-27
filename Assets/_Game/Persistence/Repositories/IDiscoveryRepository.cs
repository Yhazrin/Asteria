using System;
using System.Collections.Generic;

namespace Asteria.Persistence
{
    /// <summary>
    /// Persistent discovery storage. Backed by SaveService.
    /// </summary>
    public interface IDiscoveryRepository
    {
        /// <summary>True if the given discovery ID has been recorded.</summary>
        bool Has(string discoveryId);

        /// <summary>Record a new discovery. Returns true if newly added.</summary>
        bool Record(string discoveryId, string displayName);

        /// <summary>All recorded discoveries.</summary>
        IReadOnlyList<DiscoveryRecordDTO> GetAll();

        /// <summary>Total discovery count.</summary>
        int Count { get; }

        /// <summary>Fires when a new discovery is recorded.</summary>
        event Action<DiscoveryRecordDTO> OnRecorded;
    }
}
