using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Manages post-processing effects for the planet.
    /// Handles color grading, bloom, and atmospheric effects.
    /// </summary>
    public sealed class ProceduralPostProcessing : MonoBehaviour
    {
        [Header("Color Grading")]
        [SerializeField] float saturation = 1.1f;
        [SerializeField] float contrast = 1.05f;
        [SerializeField] float brightness = 1f;
        [SerializeField] Color colorFilter = Color.white;

        [Header("Bloom")]
        [SerializeField] float bloomIntensity = 0.3f;
        [SerializeField] float bloomThreshold = 0.8f;
        [SerializeField] float bloomSoftness = 0.5f;

        [Header("Vignette")]
        [SerializeField] float vignetteIntensity = 0.3f;
        [SerializeField] float vignetteSmoothness = 0.3f;

        [Header("Atmosphere")]
        [SerializeField] float atmosphereDensity = 0.01f;
        [SerializeField] Color atmosphereColor = new(0.5f, 0.7f, 0.9f);

        [Header("References")]
        [SerializeField] Camera mainCamera;

        Material _postProcessMaterial;
        RenderTexture _renderTexture;

        void Start()
        {
            if (mainCamera == null)
                mainCamera = Camera.main;

            CreatePostProcessMaterial();
        }

        void CreatePostProcessMaterial()
        {
            // Create a simple post-processing shader
            var shader = Shader.Find("Hidden/PostProcess");
            if (shader == null)
            {
                // Fallback to unlit
                shader = Shader.Find("Universal Render Pipeline/Unlit");
            }

            _postProcessMaterial = new Material(shader);
        }

        void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            if (_postProcessMaterial == null)
            {
                Graphics.Blit(source, destination);
                return;
            }

            // Apply color grading
            _postProcessMaterial.SetFloat("_Saturation", saturation);
            _postProcessMaterial.SetFloat("_Contrast", contrast);
            _postProcessMaterial.SetFloat("_Brightness", brightness);
            _postProcessMaterial.SetColor("_ColorFilter", colorFilter);

            // Apply bloom
            _postProcessMaterial.SetFloat("_BloomIntensity", bloomIntensity);
            _postProcessMaterial.SetFloat("_BloomThreshold", bloomThreshold);
            _postProcessMaterial.SetFloat("_BloomSoftness", bloomSoftness);

            // Apply vignette
            _postProcessMaterial.SetFloat("_VignetteIntensity", vignetteIntensity);
            _postProcessMaterial.SetFloat("_VignetteSmoothness", vignetteSmoothness);

            // Apply atmosphere
            _postProcessMaterial.SetFloat("_AtmosphereDensity", atmosphereDensity);
            _postProcessMaterial.SetColor("_AtmosphereColor", atmosphereColor);

            Graphics.Blit(source, destination, _postProcessMaterial);
        }

        /// <summary>
        /// Set saturation level.
        /// </summary>
        public void SetSaturation(float value)
        {
            saturation = Mathf.Clamp(value, 0f, 2f);
        }

        /// <summary>
        /// Set contrast level.
        /// </summary>
        public void SetContrast(float value)
        {
            contrast = Mathf.Clamp(value, 0.5f, 2f);
        }

        /// <summary>
        /// Set brightness level.
        /// </summary>
        public void SetBrightness(float value)
        {
            brightness = Mathf.Clamp(value, 0.5f, 2f);
        }

        /// <summary>
        /// Set bloom intensity.
        /// </summary>
        public void SetBloom(float intensity, float threshold, float softness)
        {
            bloomIntensity = intensity;
            bloomThreshold = threshold;
            bloomSoftness = softness;
        }

        /// <summary>
        /// Set vignette intensity.
        /// </summary>
        public void SetVignette(float intensity, float smoothness)
        {
            vignetteIntensity = intensity;
            vignetteSmoothness = smoothness;
        }

        /// <summary>
        /// Set atmosphere density.
        /// </summary>
        public void SetAtmosphere(float density, Color color)
        {
            atmosphereDensity = density;
            atmosphereColor = color;
        }

        /// <summary>
        /// Apply a preset profile.
        /// </summary>
        public void ApplyProfile(string profileName)
        {
            switch (profileName)
            {
                case "default":
                    saturation = 1.1f;
                    contrast = 1.05f;
                    brightness = 1f;
                    bloomIntensity = 0.3f;
                    vignetteIntensity = 0.3f;
                    break;

                case "warm":
                    saturation = 1.2f;
                    contrast = 1.1f;
                    brightness = 1.05f;
                    colorFilter = new Color(1f, 0.95f, 0.9f);
                    break;

                case "cool":
                    saturation = 0.9f;
                    contrast = 1.1f;
                    brightness = 0.95f;
                    colorFilter = new Color(0.9f, 0.95f, 1f);
                    break;

                case "vivid":
                    saturation = 1.4f;
                    contrast = 1.2f;
                    brightness = 1.1f;
                    bloomIntensity = 0.5f;
                    break;

                case "muted":
                    saturation = 0.7f;
                    contrast = 0.9f;
                    brightness = 0.9f;
                    bloomIntensity = 0.1f;
                    break;
            }
        }
    }
}
