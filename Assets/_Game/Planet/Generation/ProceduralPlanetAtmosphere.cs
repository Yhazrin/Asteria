using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Atmosphere system for procedural planets.
    /// Handles sky, fog, and atmospheric effects.
    /// </summary>
    public sealed class ProceduralPlanetAtmosphere : MonoBehaviour
    {
        [Header("Atmosphere")]
        [SerializeField] float atmosphereRadius = 1.3f;
        [SerializeField] Color atmosphereColorDay = new(0.4f, 0.6f, 0.9f, 0.3f);
        [SerializeField] Color atmosphereColorSunset = new(0.9f, 0.5f, 0.3f, 0.5f);
        [SerializeField] Color atmosphereColorNight = new(0.1f, 0.1f, 0.3f, 0.2f);

        [Header("Sky")]
        [SerializeField] Color skyZenith = new(0.2f, 0.4f, 0.8f);
        [SerializeField] Color skyHorizon = new(0.6f, 0.7f, 0.9f);

        [Header("Fog")]
        [SerializeField] float fogDensityDay = 0.001f;
        [SerializeField] float fogDensityNight = 0.003f;
        [SerializeField] Color fogColorDay = new(0.55f, 0.68f, 0.82f);
        [SerializeField] Color fogColorNight = new(0.05f, 0.05f, 0.1f);

        [Header("References")]
        [SerializeField] PlanetBody planet;
        [SerializeField] ProceduralLighting lighting;

        Mesh _atmosphereMesh;
        Material _atmosphereMaterial;

        void Start()
        {
            if (planet == null)
                planet = FindFirstObjectByType<PlanetBody>();
            if (lighting == null)
                lighting = FindFirstObjectByType<ProceduralLighting>();

            CreateAtmosphereMesh();
            CreateAtmosphereMaterial();
        }

        void Update()
        {
            UpdateAtmosphere();
        }

        void CreateAtmosphereMesh()
        {
            // Create a slightly larger sphere for atmosphere
            _atmosphereMesh = PlanetMeshFactory.CreateUvSphere(64, 32, 1f);
        }

        void CreateAtmosphereMaterial()
        {
            var shader = Shader.Find("Universal Render Pipeline/Unlit")
                ?? Shader.Find("Sprites/Default");

            _atmosphereMaterial = new Material(shader);
            _atmosphereMaterial.color = atmosphereColorDay;
            _atmosphereMaterial.renderQueue = 3100;
        }

        void UpdateAtmosphere()
        {
            if (planet == null || lighting == null) return;

            float timeOfDay = lighting.GetTimeOfDay();

            // Update atmosphere color
            Color currentColor = CalculateAtmosphereColor(timeOfDay);
            if (_atmosphereMaterial != null)
            {
                _atmosphereMaterial.color = currentColor;
            }

            // Update fog
            UpdateFog(timeOfDay);

            // Update sky
            UpdateSky(timeOfDay);
        }

        Color CalculateAtmosphereColor(float timeOfDay)
        {
            float sunsetFactor = 1f - Mathf.Abs(timeOfDay - 0.5f) * 4f;
            sunsetFactor = Mathf.Clamp01(sunsetFactor);
            sunsetFactor = Mathf.Pow(sunsetFactor, 3f);

            float dayFactor = Mathf.Sin(timeOfDay * Mathf.PI);

            Color dayColor = Color.Lerp(atmosphereColorDay, atmosphereColorSunset, sunsetFactor);
            Color nightColor = atmosphereColorNight;

            return Color.Lerp(nightColor, dayColor, dayFactor);
        }

        void UpdateFog(float timeOfDay)
        {
            float dayFactor = Mathf.Sin(timeOfDay * Mathf.PI);

            RenderSettings.fog = true;
            RenderSettings.fogMode = FogMode.ExponentialSquared;
            RenderSettings.fogColor = Color.Lerp(fogColorNight, fogColorDay, dayFactor);
            RenderSettings.fogDensity = Mathf.Lerp(fogDensityNight, fogDensityDay, dayFactor);
        }

        void UpdateSky(float timeOfDay)
        {
            float dayFactor = Mathf.Sin(timeOfDay * Mathf.PI);

            Color skyColor = Color.Lerp(atmosphereColorNight, skyZenith, dayFactor);
            RenderSettings.ambientSkyColor = skyColor;
            RenderSettings.ambientEquatorColor = skyColor * 0.8f;
            RenderSettings.ambientGroundColor = skyColor * 0.4f;
        }

        /// <summary>
        /// Check if a position is inside the atmosphere.
        /// </summary>
        public bool IsInsideAtmosphere(Vector3 position)
        {
            if (planet == null) return false;

            float distance = Vector3.Distance(position, planet.Center);
            float atmosphereHeight = planet.Radius * atmosphereRadius;
            return distance < atmosphereHeight;
        }

        /// <summary>
        /// Get the atmosphere density at a position.
        /// </summary>
        public float GetAtmosphereDensity(Vector3 position)
        {
            if (planet == null) return 0f;

            float distance = Vector3.Distance(position, planet.Center);
            float atmosphereHeight = planet.Radius * atmosphereRadius;
            float normalizedAlt = Mathf.Clamp01((atmosphereHeight - distance) / atmosphereHeight);

            return Mathf.Exp(-normalizedAlt * 5f) * 0.01f;
        }
    }
}
