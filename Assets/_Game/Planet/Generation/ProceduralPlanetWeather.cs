using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Weather system for procedural planets.
    /// Handles weather transitions and effects.
    /// </summary>
    public sealed class ProceduralPlanetWeather : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float weatherTransitionSpeed = 0.5f;
        [SerializeField] float minWeatherDuration = 30f;
        [SerializeField] float maxWeatherDuration = 120f;

        [Header("References")]
        [SerializeField] Weather.WeatherSystem weatherSystem;
        [SerializeField] PlanetBody planet;

        float _weatherTimer;
        Weather.WeatherType _currentWeather;

        void Start()
        {
            if (weatherSystem == null)
                weatherSystem = FindFirstObjectByType<Weather.WeatherSystem>();
            if (planet == null)
                planet = FindFirstObjectByType<PlanetBody>();

            InitializeWeather();
        }

        void Update()
        {
            UpdateWeather();
        }

        void InitializeWeather()
        {
            _currentWeather = Weather.WeatherType.Clear;
            _weatherTimer = Random.Range(minWeatherDuration, maxWeatherDuration);
        }

        void UpdateWeather()
        {
            _weatherTimer -= Time.deltaTime;

            if (_weatherTimer <= 0f)
            {
                ChangeWeather();
                _weatherTimer = Random.Range(minWeatherDuration, maxWeatherDuration);
            }
        }

        void ChangeWeather()
        {
            // Select new weather based on biome and time
            var newWeather = SelectWeather();

            if (newWeather != _currentWeather)
            {
                _currentWeather = newWeather;
                weatherSystem?.TransitionTo(newWeather);
                Debug.Log($"[ProceduralPlanetWeather] Weather changed to: {newWeather}");
            }
        }

        Weather.WeatherType SelectWeather()
        {
            // Weighted random selection
            var candidates = new[]
            {
                (Weather.WeatherType.Clear, 40f),
                (Weather.WeatherType.Cloudy, 25f),
                (Weather.WeatherType.Rain, 15f),
                (Weather.WeatherType.Storm, 5f),
                (Weather.WeatherType.Fog, 10f),
                (Weather.WeatherType.Snow, 5f),
            };

            float totalWeight = 0f;
            foreach (var (_, weight) in candidates) totalWeight += weight;

            float roll = Random.Range(0f, totalWeight);
            float cumulative = 0f;

            foreach (var (type, weight) in candidates)
            {
                cumulative += weight;
                if (roll <= cumulative) return type;
            }

            return Weather.WeatherType.Clear;
        }

        /// <summary>
        /// Get the current weather.
        /// </summary>
        public Weather.WeatherType GetCurrentWeather()
        {
            return _currentWeather;
        }

        /// <summary>
        /// Force a weather change.
        /// </summary>
        public void SetWeather(Weather.WeatherType weather)
        {
            _currentWeather = weather;
            weatherSystem?.TransitionTo(weather);
        }
    }
}
