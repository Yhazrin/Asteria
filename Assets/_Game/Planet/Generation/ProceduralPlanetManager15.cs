using System.Collections.Generic;
using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Manager with planet logging.
    /// </summary>
    public sealed class ProceduralPlanetManager15 : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] bool enableLogging = true;

        [Header("References")]
        [SerializeField] Transform player;

        readonly List<string> _logs = new();

        void Start()
        {
            if (player == null)
            {
                var playerBody = FindFirstObjectByType<Player.SphericalGravityBody>();
                player = playerBody?.transform;
            }
        }

        /// <summary>
        /// Log a message.
        /// </summary>
        public void Log(string message)
        {
            if (!enableLogging) return;

            string logEntry = $"[{Time.time:F2}] {message}";
            _logs.Add(logEntry);
            Debug.Log($"[ProceduralPlanetManager15] {message}");
        }

        /// <summary>
        /// Get all logs.
        /// </summary>
        public List<string> GetLogs()
        {
            return new List<string>(_logs);
        }

        /// <summary>
        /// Clear logs.
        /// </summary>
        public void ClearLogs()
        {
            _logs.Clear();
        }
    }
}
