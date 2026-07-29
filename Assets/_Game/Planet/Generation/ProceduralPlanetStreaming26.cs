using System.Collections.Generic;
using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Streaming system with chunk throttling.
    /// </summary>
    public sealed class ProceduralPlanetStreaming26 : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float throttleInterval = 0.1f;

        [Header("References")]
        [SerializeField] ProceduralPlanetGenerator planetGenerator;

        float _throttleTimer;

        void Start()
        {
            if (planetGenerator == null)
                planetGenerator = FindFirstObjectByType<ProceduralPlanetGenerator>();
        }

        /// <summary>
        /// Check if loading is throttled.
        /// </summary>
        public bool IsThrottled()
        {
            if (Time.time - _throttleTimer < throttleInterval) return true;
            _throttleTimer = Time.time;
            return false;
        }
    }
}
