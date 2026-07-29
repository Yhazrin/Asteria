using System;
using UnityEngine;

namespace Asteria.Planet.Weather
{
    /// <summary>
    /// Dynamic weather system for planets.
    /// Manages weather transitions, effects, and gameplay impact.
    /// </summary>
    public sealed class WeatherSystem : MonoBehaviour
    {
        [Header("Current Weather")]
        [SerializeField] WeatherType currentWeather = WeatherType.Clear;
        [SerializeField] float weatherIntensity = 0f;

        [Header("Transition")]
        [SerializeField] float transitionDuration = 5f;
        [SerializeField] float minWeatherDuration = 30f;
        [SerializeField] float maxWeatherDuration = 120f;

        [Header("Effects")]
        [SerializeField] ParticleSystem rainParticles;
        [SerializeField] ParticleSystem snowParticles;
        [SerializeField] ParticleSystem fogParticles;
        [SerializeField] ParticleSystem windParticles;

        [Header("Audio")]
        [SerializeField] AudioSource weatherAudioSource;
        [SerializeField] AudioClip rainSound;
        [SerializeField] AudioClip windSound;
        [SerializeField] AudioClip thunderSound;

        // State
        WeatherType _targetWeather;
        float _weatherTimer;
        float _transitionTimer;
        bool _isTransitioning;

        // Events
        public event Action<WeatherType> OnWeatherChanged;
        public event Action<float> OnIntensityChanged;

        public WeatherType CurrentWeather => currentWeather;
        public float Intensity => weatherIntensity;

        void Start()
        {
            // Start with random weather
            SetWeather(WeatherType.Clear, 0f);
        }

        void Update()
        {
            _weatherTimer -= Time.deltaTime;

            if (_isTransitioning)
            {
                UpdateTransition();
            }

            // Auto-change weather
            if (_weatherTimer <= 0f && !_isTransitioning)
            {
                ChooseNextWeather();
            }

            UpdateEffects();
        }

        void ChooseNextWeather()
        {
            // Weighted random weather selection
            var candidates = new[]
            {
                (WeatherType.Clear, 40f),
                (WeatherType.Cloudy, 25f),
                (WeatherType.Rain, 15f),
                (WeatherType.Storm, 5f),
                (WeatherType.Fog, 10f),
                (WeatherType.Snow, 5f),
            };

            float totalWeight = 0f;
            foreach (var (_, weight) in candidates) totalWeight += weight;

            float roll = UnityEngine.Random.Range(0f, totalWeight);
            float cumulative = 0f;

            foreach (var (type, weight) in candidates)
            {
                cumulative += weight;
                if (roll <= cumulative)
                {
                    TransitionTo(type);
                    return;
                }
            }

            TransitionTo(WeatherType.Clear);
        }

        /// <summary>
        /// Transition to a new weather type.
        /// </summary>
        public void TransitionTo(WeatherType target)
        {
            if (target == currentWeather && weatherIntensity >= 0.8f) return;

            _targetWeather = target;
            _isTransitioning = true;
            _transitionTimer = 0f;

            Debug.Log($"[Weather] Transitioning to {target}");
        }

        /// <summary>
        /// Set weather immediately (no transition).
        /// </summary>
        public void SetWeather(WeatherType type, float intensity)
        {
            currentWeather = type;
            weatherIntensity = intensity;
            _isTransitioning = false;
            _weatherTimer = UnityEngine.Random.Range(minWeatherDuration, maxWeatherDuration);

            OnWeatherChanged?.Invoke(currentWeather);
            OnIntensityChanged?.Invoke(weatherIntensity);
        }

        void UpdateTransition()
        {
            _transitionTimer += Time.deltaTime;
            float t = Mathf.Clamp01(_transitionTimer / transitionDuration);

            // Fade out current weather
            if (t < 0.5f)
            {
                weatherIntensity = Mathf.Lerp(weatherIntensity, 0f, t * 2f);
            }
            // Switch and fade in new weather
            else
            {
                if (currentWeather != _targetWeather)
                {
                    currentWeather = _targetWeather;
                    OnWeatherChanged?.Invoke(currentWeather);
                }
                weatherIntensity = Mathf.Lerp(0f, 1f, (t - 0.5f) * 2f);
            }

            OnIntensityChanged?.Invoke(weatherIntensity);

            if (t >= 1f)
            {
                _isTransitioning = false;
                weatherIntensity = 1f;
                _weatherTimer = UnityEngine.Random.Range(minWeatherDuration, maxWeatherDuration);
            }
        }

        void UpdateEffects()
        {
            // Update particle systems
            SetParticleSystem(rainParticles, currentWeather == WeatherType.Rain || currentWeather == WeatherType.Storm);
            SetParticleSystem(snowParticles, currentWeather == WeatherType.Snow);
            SetParticleSystem(fogParticles, currentWeather == WeatherType.Fog);
            SetParticleSystem(windParticles, currentWeather == WeatherType.Wind || currentWeather == WeatherType.Storm);

            // Update audio
            if (weatherAudioSource != null)
            {
                AudioClip targetClip = currentWeather switch
                {
                    WeatherType.Rain => rainSound,
                    WeatherType.Storm => rainSound,
                    WeatherType.Wind => windSound,
                    _ => null
                };

                if (targetClip != null && weatherAudioSource.clip != targetClip)
                {
                    weatherAudioSource.clip = targetClip;
                    weatherAudioSource.Play();
                }
                else if (targetClip == null && weatherAudioSource.isPlaying)
                {
                    weatherAudioSource.Stop();
                }

                weatherAudioSource.volume = weatherIntensity * 0.5f;
            }
        }

        void SetParticleSystem(ParticleSystem ps, bool active)
        {
            if (ps == null) return;

            var emission = ps.emission;
            emission.rateOverTime = active ? weatherIntensity * 100f : 0f;
        }

        /// <summary>
        /// Get the current wind strength for gameplay effects.
        /// </summary>
        public float GetWindStrength()
        {
            return currentWeather switch
            {
                WeatherType.Clear => 0.1f,
                WeatherType.Cloudy => 0.3f,
                WeatherType.Rain => 0.5f,
                WeatherType.Storm => 1f,
                WeatherType.Fog => 0.1f,
                WeatherType.Snow => 0.2f,
                WeatherType.Wind => 0.8f,
                _ => 0f
            } * weatherIntensity;
        }

        /// <summary>
        /// Get the current visibility multiplier.
        /// </summary>
        public float GetVisibility()
        {
            return currentWeather switch
            {
                WeatherType.Clear => 1f,
                WeatherType.Cloudy => 0.8f,
                WeatherType.Rain => 0.6f,
                WeatherType.Storm => 0.3f,
                WeatherType.Fog => 0.2f,
                WeatherType.Snow => 0.5f,
                WeatherType.Wind => 0.7f,
                _ => 1f
            };
        }
    }

    public enum WeatherType
    {
        Clear,
        Cloudy,
        Rain,
        Storm,
        Fog,
        Snow,
        Wind
    }
}
