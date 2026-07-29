using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Lighting system for procedural planets.
    /// Handles sun, shadows, and dynamic lighting.
    /// </summary>
    public sealed class ProceduralPlanetLighting : MonoBehaviour
    {
        [Header("Sun")]
        [SerializeField] float sunDistance = 500f;
        [SerializeField] float sunIntensity = 1.2f;
        [SerializeField] Color sunColor = new(1f, 0.95f, 0.85f);
        [SerializeField] float rotationSpeed = 0.1f;

        [Header("Ambient")]
        [SerializeField] Color ambientDay = new(0.5f, 0.6f, 0.8f);
        [SerializeField] Color ambientNight = new(0.1f, 0.1f, 0.2f);
        [SerializeField] Color ambientSunset = new(0.8f, 0.5f, 0.3f);

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
            }

            sunLight.color = sunColor;
            sunLight.intensity = sunIntensity;
        }

        void UpdateSunPosition()
        {
            _sunAngle += rotationSpeed * Time.deltaTime;
            if (_sunAngle >= 360f) _sunAngle -= 360f;

            Vector3 sunDirection = Quaternion.Euler(0, _sunAngle, 0) * Vector3.forward;
            Vector3 sunPosition = planet.Center + sunDirection * sunDistance;

            sunLight.transform.position = sunPosition;
            sunLight.transform.LookAt(planet.Center);

            _timeOfDay = (_sunAngle / 360f + 0.25f) % 1f;
        }

        void UpdateLighting()
        {
            float dayFactor = Mathf.Sin(_timeOfDay * Mathf.PI);
            sunLight.intensity = sunIntensity * Mathf.Clamp01(dayFactor);

            Color currentSunColor;
            if (_timeOfDay < 0.25f || _timeOfDay > 0.75f)
            {
                currentSunColor = Color.Lerp(sunColor, new Color(0.3f, 0.3f, 0.5f), 0.8f);
            }
            else if (_timeOfDay < 0.35f || _timeOfDay > 0.65f)
            {
                currentSunColor = Color.Lerp(sunColor, ambientSunset, 0.5f);
            }
            else
            {
                currentSunColor = sunColor;
            }

            sunLight.color = currentSunColor;

            Color currentAmbient;
            if (_timeOfDay < 0.25f || _timeOfDay > 0.75f)
                currentAmbient = ambientNight;
            else if (_timeOfDay < 0.35f || _timeOfDay > 0.65f)
                currentAmbient = ambientSunset;
            else
                currentAmbient = ambientDay;

            RenderSettings.ambientSkyColor = currentAmbient;
            RenderSettings.ambientEquatorColor = currentAmbient * 0.8f;
            RenderSettings.ambientGroundColor = currentAmbient * 0.4f;
        }

        public float GetTimeOfDay() => _timeOfDay;
        public Vector3 GetSunDirection() => sunLight != null ? -sunLight.transform.forward : Vector3.up;
    }
}
