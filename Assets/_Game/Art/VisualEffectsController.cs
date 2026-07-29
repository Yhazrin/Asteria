using UnityEngine;

namespace Asteria.Art
{
    /// <summary>
    /// Controls all visual effects based on game state.
    /// Integrates with day/night cycle, weather, and biome.
    /// </summary>
    public sealed class VisualEffectsController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] Light sunLight;
        [SerializeField] Material skyboxMaterial;

        [Header("Day/Night")]
        [SerializeField] Color dayAmbient = new(0.55f, 0.7f, 0.9f);
        [SerializeField] Color sunsetAmbient = new(0.8f, 0.5f, 0.3f);
        [SerializeField] Color nightAmbient = new(0.1f, 0.1f, 0.2f);

        [Header("Weather")]
        [SerializeField] float fogDensityClear = 0.001f;
        [SerializeField] float fogDensityFog = 0.01f;
        [SerializeField] float fogDensityStorm = 0.02f;

        Core.IGameClock _clock;
        Planet.Weather.WeatherSystem _weather;

        void Start()
        {
            _clock = Core.GameBootstrap.Instance?.GameClock;
            _weather = FindFirstObjectByType<Planet.Weather.WeatherSystem>();

            if (sunLight == null)
            {
                var sun = FindFirstObjectByType<Light>();
                if (sun != null && sun.type == LightType.Directional)
                    sunLight = sun;
            }
        }

        void Update()
        {
            UpdateDayNight();
            UpdateWeatherEffects();
        }

        void UpdateDayNight()
        {
            if (_clock == null) return;

            float timeOfDay = _clock.TimeOfDay;
            float dayFactor = Mathf.Sin(timeOfDay * Mathf.PI);

            // Sun intensity
            if (sunLight != null)
            {
                sunLight.intensity = Mathf.Lerp(0.1f, 1.2f, dayFactor);

                // Sun color
                float sunsetFactor = 1f - Mathf.Abs(timeOfDay - 0.5f) * 4f;
                sunsetFactor = Mathf.Clamp01(sunsetFactor);
                sunsetFactor = Mathf.Pow(sunsetFactor, 3f);

                Color sunColor = Color.Lerp(
                    new Color(0.2f, 0.2f, 0.4f),
                    Color.Lerp(new Color(1f, 0.95f, 0.85f), new Color(1f, 0.6f, 0.3f), sunsetFactor),
                    dayFactor);
                sunLight.color = sunColor;
            }

            // Ambient
            Color ambient;
            if (timeOfDay < 0.3f || timeOfDay > 0.7f)
                ambient = Color.Lerp(nightAmbient, sunsetAmbient, dayFactor);
            else
                ambient = Color.Lerp(sunsetAmbient, dayAmbient, dayFactor);

            RenderSettings.ambientSkyColor = ambient;
            RenderSettings.ambientEquatorColor = ambient * 0.8f;
            RenderSettings.ambientGroundColor = ambient * 0.4f;
        }

        void UpdateWeatherEffects()
        {
            if (_weather == null) return;

            float targetFog = _weather.CurrentWeather switch
            {
                Planet.Weather.WeatherType.Clear => fogDensityClear,
                Planet.Weather.WeatherType.Cloudy => fogDensityClear * 2f,
                Planet.Weather.WeatherType.Rain => fogDensityClear * 3f,
                Planet.Weather.WeatherType.Storm => fogDensityStorm,
                Planet.Weather.WeatherType.Fog => fogDensityFog,
                Planet.Weather.WeatherType.Snow => fogDensityClear * 2f,
                Planet.Weather.WeatherType.Wind => fogDensityClear * 1.5f,
                _ => fogDensityClear
            };

            RenderSettings.fog = true;
            RenderSettings.fogMode = FogMode.ExponentialSquared;
            RenderSettings.fogDensity = Mathf.Lerp(RenderSettings.fogDensity,
                targetFog * _weather.Intensity, Time.deltaTime * 2f);

            // Fog color based on time and weather
            float dayFactor = _clock != null ? Mathf.Sin(_clock.TimeOfDay * Mathf.PI) : 1f;
            Color fogColor = Color.Lerp(new Color(0.05f, 0.05f, 0.1f), new Color(0.55f, 0.68f, 0.82f), dayFactor);
            RenderSettings.fogColor = fogColor;
        }
    }
}
