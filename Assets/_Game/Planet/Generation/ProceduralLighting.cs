using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Manages procedural lighting effects on the planet.
    /// Handles sun position, shadows, and dynamic lighting.
    /// </summary>
    public sealed class ProceduralLighting : MonoBehaviour
    {
        [Header("Sun Settings")]
        [SerializeField] float sunDistance = 500f;
        [SerializeField] float sunIntensity = 1.2f;
        [SerializeField] Color sunColor = new(1f, 0.95f, 0.85f);
        [SerializeField] float rotationSpeed = 0.1f;

        [Header("Ambient")]
        [SerializeField] Color ambientDay = new(0.5f, 0.6f, 0.8f);
        [SerializeField] Color ambientNight = new(0.1f, 0.1f, 0.2f);
        [SerializeField] Color ambientSunset = new(0.8f, 0.5f, 0.3f);

        [Header("Shadows")]
        [SerializeField] float shadowDistance = 200f;
        [SerializeField] float shadowStrength = 0.7f;

        [Header("References")]
        [SerializeField] PlanetBody planet;
        [SerializeField] Light sunLight;

        float _sunAngle;
        float _timeOfDay;

        void Start()
        {
            if (planet == null)
                planet = FindFirstObjectByType<PlanetBody>();

            CreateSunLight();
        }

        void Update()
        {
            UpdateSunPosition();
            UpdateLighting();
        }

        void CreateSunLight()
        {
            if (sunLight == null)
            {
                var sunGo = new GameObject("Sun");
                sunLight = sunGo.AddComponent<Light>();
                sunLight.type = LightType.Directional;
                sunLight.shadows = LightShadows.Soft;
                sunLight.shadowStrength = shadowStrength;
                sunLight.shadowDistance = shadowDistance;
            }

            sunLight.color = sunColor;
            sunLight.intensity = sunIntensity;
        }

        void UpdateSunPosition()
        {
            _sunAngle += rotationSpeed * Time.deltaTime;
            if (_sunAngle >= 360f) _sunAngle -= 360f;

            // Rotate sun around planet
            Vector3 sunDirection = Quaternion.Euler(0, _sunAngle, 0) * Vector3.forward;
            Vector3 sunPosition = planet.Center + sunDirection * sunDistance;

            sunLight.transform.position = sunPosition;
            sunLight.transform.LookAt(planet.Center);

            // Calculate time of day (0 = midnight, 0.5 = noon)
            _timeOfDay = (_sunAngle / 360f + 0.25f) % 1f;
        }

        void UpdateLighting()
        {
            // Sun intensity based on time
            float dayFactor = Mathf.Sin(_timeOfDay * Mathf.PI);
            sunLight.intensity = sunIntensity * Mathf.Clamp01(dayFactor);

            // Sun color based on time
            Color currentSunColor;
            if (_timeOfDay < 0.25f || _timeOfDay > 0.75f)
            {
                // Night
                currentSunColor = Color.Lerp(sunColor, new Color(0.3f, 0.3f, 0.5f), 0.8f);
            }
            else if (_timeOfDay < 0.35f || _timeOfDay > 0.65f)
            {
                // Sunset/Sunrise
                currentSunColor = Color.Lerp(sunColor, ambientSunset, 0.5f);
            }
            else
            {
                // Day
                currentSunColor = sunColor;
            }

            sunLight.color = currentSunColor;

            // Ambient color
            Color currentAmbient;
            if (_timeOfDay < 0.25f || _timeOfDay > 0.75f)
            {
                currentAmbient = ambientNight;
            }
            else if (_timeOfDay < 0.35f || _timeOfDay > 0.65f)
            {
                currentAmbient = ambientSunset;
            }
            else
            {
                currentAmbient = ambientDay;
            }

            RenderSettings.ambientSkyColor = currentAmbient;
            RenderSettings.ambientEquatorColor = currentAmbient * 0.8f;
            RenderSettings.ambientGroundColor = currentAmbient * 0.4f;
        }

        /// <summary>
        /// Get the current time of day (0-1).
        /// </summary>
        public float GetTimeOfDay()
        {
            return _timeOfDay;
        }

        /// <summary>
        /// Set the time of day directly.
        /// </summary>
        public void SetTimeOfDay(float time)
        {
            _timeOfDay = Mathf.Clamp01(time);
            _sunAngle = (_timeOfDay - 0.25f) * 360f;
        }

        /// <summary>
        /// Get the sun direction.
        /// </summary>
        public Vector3 GetSunDirection()
        {
            return sunLight != null ? -sunLight.transform.forward : Vector3.up;
        }

        /// <summary>
        /// Check if a position is in shadow.
        /// </summary>
        public bool IsInShadow(Vector3 position)
        {
            if (sunLight == null) return false;

            Vector3 sunDir = -sunLight.transform.forward;
            Ray ray = new Ray(position, sunDir);

            if (Physics.Raycast(ray, out RaycastHit hit, shadowDistance))
            {
                return true;
            }

            return false;
        }
    }
}
