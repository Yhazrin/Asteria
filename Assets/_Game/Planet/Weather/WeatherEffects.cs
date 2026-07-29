using UnityEngine;

namespace Asteria.Planet.Weather
{
    /// <summary>
    /// Visual effects for weather conditions.
    /// Manages particles, fog, lighting, and screen effects.
    /// </summary>
    public sealed class WeatherEffects : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] WeatherSystem weatherSystem;
        [SerializeField] Light sunLight;
        [SerializeField] Material skyboxMaterial;

        [Header("Rain")]
        [SerializeField] ParticleSystem rainParticles;
        [SerializeField] AudioSource rainAudio;

        [Header("Snow")]
        [SerializeField] ParticleSystem snowParticles;
        [SerializeField] AudioSource snowAudio;

        [Header("Wind")]
        [SerializeField] ParticleSystem windParticles;
        [SerializeField] AudioSource windAudio;

        [Header("Fog")]
        [SerializeField] float fogDensityClear = 0.001f;
        [SerializeField] float fogDensityFog = 0.01f;
        [SerializeField] float fogDensityStorm = 0.02f;

        [Header("Lighting")]
        [SerializeField] float sunIntensityClear = 1.2f;
        [SerializeField] float sunIntensityCloudy = 0.8f;
        [SerializeField] float sunIntensityStorm = 0.4f;

        void Start()
        {
            if (weatherSystem == null)
                weatherSystem = FindFirstObjectByType<WeatherSystem>();
        }

        void Update()
        {
            if (weatherSystem == null) return;

            UpdateParticles();
            UpdateFog();
            UpdateLighting();
            UpdateAudio();
        }

        void UpdateParticles()
        {
            float intensity = weatherSystem.Intensity;

            // Rain
            if (rainParticles != null)
            {
                var emission = rainParticles.emission;
                bool isRaining = weatherSystem.CurrentWeather == WeatherType.Rain ||
                                 weatherSystem.CurrentWeather == WeatherType.Storm;
                emission.rateOverTime = isRaining ? intensity * 200f : 0f;
            }

            // Snow
            if (snowParticles != null)
            {
                var emission = snowParticles.emission;
                bool isSnowing = weatherSystem.CurrentWeather == WeatherType.Snow;
                emission.rateOverTime = isSnowing ? intensity * 100f : 0f;
            }

            // Wind
            if (windParticles != null)
            {
                var emission = windParticles.emission;
                bool isWindy = weatherSystem.CurrentWeather == WeatherType.Wind ||
                               weatherSystem.CurrentWeather == WeatherType.Storm;
                emission.rateOverTime = isWindy ? intensity * 50f : 0f;
            }
        }

        void UpdateFog()
        {
            RenderSettings.fog = true;
            RenderSettings.fogMode = FogMode.ExponentialSquared;

            float targetDensity = weatherSystem.CurrentWeather switch
            {
                WeatherType.Clear => fogDensityClear,
                WeatherType.Cloudy => fogDensityClear * 2f,
                WeatherType.Rain => fogDensityClear * 3f,
                WeatherType.Storm => fogDensityStorm,
                WeatherType.Fog => fogDensityFog,
                WeatherType.Snow => fogDensityClear * 2f,
                WeatherType.Wind => fogDensityClear * 1.5f,
                _ => fogDensityClear
            };

            RenderSettings.fogDensity = Mathf.Lerp(
                RenderSettings.fogDensity,
                targetDensity * weatherSystem.Intensity,
                Time.deltaTime * 2f);
        }

        void UpdateLighting()
        {
            if (sunLight == null) return;

            float targetIntensity = weatherSystem.CurrentWeather switch
            {
                WeatherType.Clear => sunIntensityClear,
                WeatherType.Cloudy => sunIntensityCloudy,
                WeatherType.Rain => sunIntensityCloudy * 0.8f,
                WeatherType.Storm => sunIntensityStorm,
                WeatherType.Fog => sunIntensityCloudy * 0.6f,
                WeatherType.Snow => sunIntensityCloudy * 0.9f,
                WeatherType.Wind => sunIntensityClear * 0.9f,
                _ => sunIntensityClear
            };

            sunLight.intensity = Mathf.Lerp(
                sunLight.intensity,
                targetIntensity,
                Time.deltaTime * 2f);

            // Tint light based on weather
            Color targetColor = weatherSystem.CurrentWeather switch
            {
                WeatherType.Clear => new Color(1f, 0.95f, 0.85f),
                WeatherType.Cloudy => new Color(0.8f, 0.82f, 0.85f),
                WeatherType.Rain => new Color(0.7f, 0.72f, 0.78f),
                WeatherType.Storm => new Color(0.5f, 0.52f, 0.6f),
                WeatherType.Fog => new Color(0.85f, 0.85f, 0.88f),
                WeatherType.Snow => new Color(0.9f, 0.92f, 0.95f),
                WeatherType.Wind => new Color(0.95f, 0.93f, 0.88f),
                _ => Color.white
            };

            sunLight.color = Color.Lerp(sunLight.color, targetColor, Time.deltaTime * 2f);
        }

        void UpdateAudio()
        {
            float intensity = weatherSystem.Intensity;

            // Rain audio
            if (rainAudio != null)
            {
                bool isRaining = weatherSystem.CurrentWeather == WeatherType.Rain ||
                                 weatherSystem.CurrentWeather == WeatherType.Storm;
                rainAudio.volume = Mathf.Lerp(rainAudio.volume,
                    isRaining ? intensity * 0.5f : 0f,
                    Time.deltaTime * 3f);

                if (isRaining && !rainAudio.isPlaying) rainAudio.Play();
                if (!isRaining && rainAudio.volume < 0.01f) rainAudio.Stop();
            }

            // Snow audio
            if (snowAudio != null)
            {
                bool isSnowing = weatherSystem.CurrentWeather == WeatherType.Snow;
                snowAudio.volume = Mathf.Lerp(snowAudio.volume,
                    isSnowing ? intensity * 0.3f : 0f,
                    Time.deltaTime * 3f);

                if (isSnowing && !snowAudio.isPlaying) snowAudio.Play();
                if (!isSnowing && snowAudio.volume < 0.01f) snowAudio.Stop();
            }

            // Wind audio
            if (windAudio != null)
            {
                bool isWindy = weatherSystem.CurrentWeather == WeatherType.Wind ||
                               weatherSystem.CurrentWeather == WeatherType.Storm;
                windAudio.volume = Mathf.Lerp(windAudio.volume,
                    isWindy ? intensity * 0.6f : 0f,
                    Time.deltaTime * 3f);

                if (isWindy && !windAudio.isPlaying) windAudio.Play();
                if (!isWindy && windAudio.volume < 0.01f) windAudio.Stop();
            }
        }
    }
}
