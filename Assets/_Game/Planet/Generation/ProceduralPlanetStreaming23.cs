using System.Collections.Generic;
using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Streaming system with chunk statistics.
    /// </summary>
    public sealed class ProceduralPlanetStreaming23 : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] ProceduralPlanetGenerator planetGenerator;

        int _totalLoaded;
        int _totalUnloaded;
        float _averageLoadTime;

        void Start()
        {
            if (planetGenerator == null)
                planetGenerator = FindFirstObjectByType<ProceduralPlanetGenerator>();
        }

        /// <summary>
        /// Record a chunk load.
        /// </summary>
        public void RecordLoad(float loadTime)
        {
            _totalLoaded++;
            _averageLoadTime = (_averageLoadTime * (_totalLoaded - 1) + loadTime) / _totalLoaded;
        }

        /// <summary>
        /// Record a chunk unload.
        /// </summary>
        public void RecordUnload()
        {
            _totalUnloaded++;
        }

        /// <summary>
        /// Get statistics.
        /// </summary>
        public string GetStats()
        {
            return $"Loaded: {_totalLoaded}, Unloaded: {_totalUnloaded}, Avg Load Time: {_averageLoadTime:F2}ms";
        }
    }
}
