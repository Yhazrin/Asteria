using UnityEngine;

namespace Asteria.Core
{
    /// <summary>
    /// Default implementation of IWorldStateService.
    /// Tracks current weather, biome, and pressure state.
    /// </summary>
    public sealed class WorldStateService : MonoBehaviour, IWorldStateService
    {
        string _currentWeather = "clear";
        string _currentBiome = "wind_grassland";
        int _activePlayerCount = 1;
        bool _isPressureActive;

        public string CurrentWeather => _currentWeather;
        public string CurrentBiome => _currentBiome;
        public int ActivePlayerCount => _activePlayerCount;
        public bool IsPressureActive => _isPressureActive;

        public void SetWeather(string weatherId)
        {
            _currentWeather = weatherId ?? "clear";
            Debug.Log($"[Asteria] Weather changed to: {_currentWeather}");
        }

        public void SetBiome(string biomeId)
        {
            _currentBiome = biomeId ?? "wind_grassland";
        }

        public void SetPressureActive(bool active)
        {
            _isPressureActive = active;
        }

        public void SetPlayerCount(int count)
        {
            _activePlayerCount = Mathf.Max(1, count);
        }
    }
}
