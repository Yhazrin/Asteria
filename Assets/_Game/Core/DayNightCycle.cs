using UnityEngine;

namespace Asteria.Core
{
    /// <summary>
    /// Manages the visual day/night cycle.
    /// Controls sun position, ambient light, fog, and skybox colors.
    /// </summary>
    public sealed class DayNightCycle : MonoBehaviour
    {
        [Header("Sun")]
        [SerializeField] Light sunLight;
        [SerializeField] float sunIntensityDay = 1.2f;
        [SerializeField] float sunIntensityNight = 0.1f;
        [SerializeField] Color sunColorDay = new(1f, 0.95f, 0.85f);
        [SerializeField] Color sunColorSunset = new(1f, 0.6f, 0.3f);
        [SerializeField] Color sunColorNight = new(0.2f, 0.2f, 0.4f);

        [Header("Ambient")]
        [SerializeField] Color ambientDay = new(0.55f, 0.7f, 0.9f);
        [SerializeField] Color ambientSunset = new(0.8f, 0.5f, 0.3f);
        [SerializeField] Color ambientNight = new(0.1f, 0.1f, 0.2f);

        [Header("Fog")]
        [SerializeField] Color fogDay = new(0.55f, 0.68f, 0.82f);
        [SerializeField] Color fogNight = new(0.05f, 0.05f, 0.1f);
        [SerializeField] float fogDensityDay = 0.0008f;
        [SerializeField] float fogDensityNight = 0.002f;

        [Header("Stars")]
        [SerializeField] ParticleSystem starsParticles;
        [SerializeField] float starsAlphaDay = 0f;
        [SerializeField] float starsAlphaNight = 0.8f;

        IGameClock _clock;

        void Start()
        {
            _clock = GameBootstrap.Instance?.GameClock;

            // If no sun assigned, find or create one
            if (sunLight == null)
            {
                var existing = FindFirstObjectByType<Light>();
                if (existing != null && existing.type == LightType.Directional)
                {
                    sunLight = existing;
                }
                else
                {
                    var sunGo = new GameObject("Sun");
                    sunLight = sunGo.AddComponent<Light>();
                    sunLight.type = LightType.Directional;
                    sunLight.shadows = LightShadows.Soft;
                }
            }
        }

        void Update()
        {
            if (_clock == null) return;

            float timeOfDay = _clock.TimeOfDay;
            UpdateSun(timeOfDay);
            UpdateAmbient(timeOfDay);
            UpdateFog(timeOfDay);
            UpdateStars(timeOfDay);
        }

        void UpdateSun(float time)
        {
            if (sunLight == null) return;

            // Sun rotation (0=midnight, 0.25=sunrise, 0.5=noon, 0.75=sunset)
            float sunAngle = (time - 0.25f) * 360f;
            sunLight.transform.rotation = Quaternion.Euler(sunAngle, -30f, 0f);

            // Intensity curve (peak at noon)
            float intensityCurve = Mathf.Sin(time * Mathf.PI);
            sunLight.intensity = Mathf.Lerp(sunIntensityNight, sunIntensityDay, intensityCurve);

            // Color (sunrise/sunset warmth)
            float sunsetFactor = 1f - Mathf.Abs(time - 0.5f) * 4f; // Peak at 0.5
            sunsetFactor = Mathf.Clamp01(sunsetFactor);

            Color sunColor;
            if (time < 0.3f || time > 0.7f)
            {
                sunColor = Color.Lerp(sunColorNight, sunColorSunset, intensityCurve);
            }
            else
            {
                sunColor = Color.Lerp(sunColorSunset, sunColorDay, sunsetFactor);
            }
            sunLight.color = sunColor;
        }

        void UpdateAmbient(float time)
        {
            float dayFactor = Mathf.Sin(time * Mathf.PI);
            Color ambient;

            if (time < 0.3f || time > 0.7f)
            {
                ambient = Color.Lerp(ambientNight, ambientSunset, dayFactor);
            }
            else
            {
                ambient = Color.Lerp(ambientSunset, ambientDay, dayFactor);
            }

            RenderSettings.ambientSkyColor = ambient;
            RenderSettings.ambientEquatorColor = ambient * 0.8f;
            RenderSettings.ambientGroundColor = ambient * 0.4f;
        }

        void UpdateFog(float time)
        {
            float dayFactor = Mathf.Sin(time * Mathf.PI);
            RenderSettings.fogColor = Color.Lerp(fogNight, fogDay, dayFactor);
            RenderSettings.fogDensity = Mathf.Lerp(fogDensityNight, fogDensityDay, dayFactor);
        }

        void UpdateStars(float time)
        {
            if (starsParticles == null) return;

            var main = starsParticles.main;
            float nightFactor = 1f - Mathf.Sin(time * Mathf.PI);
            nightFactor = Mathf.Pow(nightFactor, 2f); // Sharper transition

            var color = main.startColor;
            color.color = new Color(1, 1, 1, Mathf.Lerp(starsAlphaDay, starsAlphaNight, nightFactor));
            main.startColor = color;
        }

        /// <summary>
        /// Set the clock reference (called by GameBootstrap).
        /// </summary>
        public void SetClock(IGameClock clock)
        {
            _clock = clock;
        }
    }
}
