using UnityEngine;

namespace Asteria.Core
{
    /// <summary>
    /// Default implementation of IGameClock.
    /// Tracks in-game day and time of day.
    /// </summary>
    public sealed class GameClock : MonoBehaviour, IGameClock
    {
        [SerializeField] float secondsPerDay = 720f; // 12 minutes real time = 1 game day

        int _worldDay = 1;
        float _timeOfDay; // 0-1 fraction
        float _elapsed;

        public int WorldDay => _worldDay;
        public float TimeOfDay => _timeOfDay;
        public float ElapsedSeconds => _elapsed;
        public bool IsRunning { get; private set; } = true;

        public void Tick(float deltaTime)
        {
            if (!IsRunning)
            {
                return;
            }

            _elapsed += deltaTime;
            _timeOfDay += deltaTime / secondsPerDay;

            if (_timeOfDay >= 1f)
            {
                _timeOfDay -= 1f;
                _worldDay++;
                Debug.Log($"[Asteria] New day: {_worldDay}");
            }
        }

        public void SetDay(int day)
        {
            _worldDay = Mathf.Max(1, day);
        }

        void Update()
        {
            Tick(Time.deltaTime);
        }
    }
}
