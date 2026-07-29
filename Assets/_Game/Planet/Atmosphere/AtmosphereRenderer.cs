using UnityEngine;

namespace Asteria.Planet.Atmosphere
{
    /// <summary>
    /// Renders atmospheric effects around the planet:
    /// - Glow halo when viewed from space
    /// - Sky gradient when on surface
    /// - Fog/haze during descent
    /// - Cloud layer visualization
    /// </summary>
    public sealed class AtmosphereRenderer : MonoBehaviour
    {
        [Header("Atmosphere")]
        [SerializeField] float atmosphereRadius = 1.3f; // Multiplier of planet radius
        [SerializeField] Color atmosphereColorDay = new(0.4f, 0.6f, 0.9f, 0.3f);
        [SerializeField] Color atmosphereColorSunset = new(0.9f, 0.5f, 0.3f, 0.5f);
        [SerializeField] Color atmosphereColorNight = new(0.1f, 0.1f, 0.3f, 0.2f);

        [Header("Sky Gradient")]
        [SerializeField] Color skyZenith = new(0.2f, 0.4f, 0.8f);
        [SerializeField] Color skyHorizon = new(0.6f, 0.7f, 0.9f);
        [SerializeField] float gradientExponent = 2f;

        [Header("Clouds")]
        [SerializeField] float cloudAltitude = 50f;
        [SerializeField] float cloudDensity = 0.5f;
        [SerializeField] float cloudSpeed = 5f;
        [SerializeField] float cloudScale = 0.002f;

        [Header("References")]
        [SerializeField] PlanetBody planet;
        [SerializeField] Material atmosphereMaterial;
        [SerializeField] MeshRenderer atmosphereMesh;

        Mesh _atmosphereMesh;
        float _time;

        void Start()
        {
            if (planet == null) planet = FindFirstObjectByType<PlanetBody>();
            CreateAtmosphereMesh();
        }

        void Update()
        {
            _time += Time.deltaTime;
            UpdateAtmosphereVisuals();
        }

        void CreateAtmosphereMesh()
        {
            // Create a slightly larger sphere for the atmosphere glow
            _atmosphereMesh = PlanetMeshFactory.CreateUvSphere(64, 32, 1f);

            if (atmosphereMesh == null)
            {
                var go = new GameObject("AtmosphereGlow");
                go.transform.SetParent(transform, false);
                go.transform.localPosition = Vector3.zero;

                atmosphereMesh = go.AddComponent<MeshRenderer>();
                var filter = go.AddComponent<MeshFilter>();
                filter.mesh = _atmosphereMesh;

                // Create atmosphere material
                if (atmosphereMaterial == null)
                {
                    atmosphereMaterial = CreateAtmosphereMaterial();
                }
                atmosphereMesh.material = atmosphereMaterial;
            }

            // Scale to atmosphere radius
            float scale = planet != null ? planet.Radius * atmosphereRadius * 2f : 400f;
            atmosphereMesh.transform.localScale = Vector3.one * scale;
        }

        Material CreateAtmosphereMaterial()
        {
            // Use a simple additive blended material for atmosphere glow
            Shader shader = Shader.Find("Universal Render Pipeline/Unlit");
            if (shader == null) shader = Shader.Find("Sprites/Default");

            var mat = new Material(shader);
            mat.color = atmosphereColorDay;
            mat.renderQueue = 3000; // Render after opaque

            // Enable transparency
            mat.SetFloat("_Surface", 1); // Transparent
            mat.SetFloat("_Blend", 0);   // Alpha blend

            return mat;
        }

        void UpdateAtmosphereVisuals()
        {
            if (atmosphereMesh == null || planet == null) return;

            // Get time of day from game clock
            var clock = GameBootstrap.Instance?.GameClock;
            float timeOfDay = clock?.TimeOfDay ?? 0.5f;

            // Calculate atmosphere color based on time
            Color currentColor = CalculateAtmosphereColor(timeOfDay);

            // Apply to material
            if (atmosphereMaterial != null)
            {
                atmosphereMaterial.color = currentColor;
            }

            // Rotate atmosphere slightly for visual interest
            atmosphereMesh.transform.Rotate(Vector3.up, cloudSpeed * 0.1f * Time.deltaTime);
        }

        Color CalculateAtmosphereColor(float timeOfDay)
        {
            // Sunset at 0.25 and 0.75
            float sunsetFactor = 1f - Mathf.Abs(timeOfDay - 0.5f) * 4f;
            sunsetFactor = Mathf.Clamp01(sunsetFactor);
            sunsetFactor = Mathf.Pow(sunsetFactor, 3f); // Sharper sunset

            // Day/night blend
            float dayFactor = Mathf.Sin(timeOfDay * Mathf.PI);
            dayFactor = Mathf.Pow(dayFactor, 0.5f);

            Color dayColor = Color.Lerp(atmosphereColorDay, atmosphereColorSunset, sunsetFactor);
            Color nightColor = atmosphereColorNight;

            return Color.Lerp(nightColor, dayColor, dayFactor);
        }

        /// <summary>
        /// Get the sky color at a specific altitude.
        /// Used by the camera for skybox blending.
        /// </summary>
        public Color GetSkyColorAtAltitude(float altitude)
        {
            float normalizedAlt = Mathf.Clamp01(altitude / (planet.Radius * 0.5f));
            return Color.Lerp(skyHorizon, skyZenith, Mathf.Pow(normalizedAlt, gradientExponent));
        }

        /// <summary>
        /// Get the atmosphere fog density at a specific altitude.
        /// </summary>
        public float GetFogDensity(float altitude)
        {
            float atmosphereHeight = planet.Radius * (atmosphereRadius - 1f);
            float normalizedAlt = Mathf.Clamp01(altitude / atmosphereHeight);

            // Exponential falloff
            return Mathf.Exp(-normalizedAlt * 5f) * 0.01f;
        }

        /// <summary>
        /// Check if a position is inside the atmosphere.
        /// </summary>
        public bool IsInsideAtmosphere(Vector3 position)
        {
            if (planet == null) return false;
            float altitude = (position - planet.Center).magnitude - planet.Radius;
            float atmosphereHeight = planet.Radius * (atmosphereRadius - 1f);
            return altitude < atmosphereHeight;
        }
    }
}
