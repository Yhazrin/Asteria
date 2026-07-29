using System.Collections.Generic;
using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Manager with planet scheduling.
    /// </summary>
    public sealed class ProceduralPlanetManager12 : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float scheduleInterval = 1f;

        [Header("References")]
        [SerializeField] Transform player;

        readonly Queue<PlanetTask> _taskQueue = new();
        float _scheduleTimer;

        void Start()
        {
            if (player == null)
            {
                var playerBody = FindFirstObjectByType<Player.SphericalGravityBody>();
                player = playerBody?.transform;
            }
        }

        void Update()
        {
            _scheduleTimer -= Time.deltaTime;
            if (_scheduleTimer <= 0f)
            {
                _scheduleTimer = scheduleInterval;
                ProcessTasks();
            }
        }

        void ProcessTasks()
        {
            if (_taskQueue.Count == 0) return;

            var task = _taskQueue.Dequeue();
            ExecuteTask(task);
        }

        void ExecuteTask(PlanetTask task)
        {
            Debug.Log($"[ProceduralPlanetManager12] Executing task: {task.type}");
        }

        /// <summary>
        /// Schedule a planet task.
        /// </summary>
        public void ScheduleTask(string type, string planetName, float priority = 0f)
        {
            _taskQueue.Enqueue(new PlanetTask
            {
                type = type,
                planetName = planetName,
                priority = priority
            });
        }

        class PlanetTask
        {
            public string type;
            public string planetName;
            public float priority;
        }
    }
}
