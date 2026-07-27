using System;
using System.Collections.Generic;
using System.Linq;

namespace Asteria.Persistence
{
    /// <summary>
    /// Discovery repository backed by SaveService.
    /// </summary>
    public sealed class DiscoveryRepository : IDiscoveryRepository
    {
        readonly SaveService _saveService;
        readonly HashSet<string> _idSet;

        public DiscoveryRepository(SaveService saveService)
        {
            _saveService = saveService ?? throw new ArgumentNullException(nameof(saveService));
            _idSet = new HashSet<string>(
                _saveService.Current.discoveries.Select(d => d.id));
        }

        public int Count => _saveService.Current.discoveries.Count;

        public event Action<DiscoveryRecordDTO> OnRecorded;

        public bool Has(string discoveryId)
        {
            return !string.IsNullOrWhiteSpace(discoveryId) && _idSet.Contains(discoveryId);
        }

        public bool Record(string discoveryId, string displayName)
        {
            if (string.IsNullOrWhiteSpace(discoveryId))
            {
                return false;
            }

            if (_idSet.Contains(discoveryId))
            {
                return false;
            }

            var record = new DiscoveryRecordDTO
            {
                id = discoveryId,
                displayName = displayName ?? discoveryId,
                timestamp = DateTime.UtcNow.ToString("o"),
                isDisplayed = false,
                displayAnchorId = null
            };

            _saveService.Current.discoveries.Add(record);
            _idSet.Add(discoveryId);
            OnRecorded?.Invoke(record);
            return true;
        }

        public IReadOnlyList<DiscoveryRecordDTO> GetAll()
        {
            return _saveService.Current.discoveries.AsReadOnly();
        }
    }
}
