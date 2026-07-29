using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Post-processing system for procedural planets.
    /// Handles color grading, bloom, and effects.
    /// </summary>
    public sealed class ProceduralPlanetPostProcessing : MonoBehaviour
    {
        [Header("Color Grading")]
        [SerializeField] float saturation = 1.1f;
        [SerializeField] float contrast = 1.05f;
        [SerializeField] float brightness = 1f;

        [Header("Bloom")]
        [SerializeField] float bloomIntensity = 0.3f;
        [SerializeField] float bloomThreshold = 0.8f;

        [Header("Vignette")]
        [SerializeField] float vignetteIntensity = 0.3f;

        [Header("References")]
        [SerializeField] Camera mainCamera;

        Material _postProcessMaterial;

        void Start()
        {
            if (mainCamera == null)
                mainCamera = Camera.main;

            CreatePostProcessMaterial();
        }

        void CreatePostProcessMaterial()
        {
            var shader = Shader.Find("Universal Render Pipeline/Unlit")
                ?? Shader.Find("Sprites/Default");

            _postProcessMaterial = new Material(shader);
        }

        void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            if (_postProcessMaterial == null)
            {
                Graphics.Blit(source, destination);
                return;
            }

            _postProcessMaterial.SetFloat("_Saturation", saturation);
            _postProcessMaterial.SetFloat("_Contrast", contrast);
            _postProcessMaterial.SetFloat("_Brightness", brightness);

            Graphics.Blit(source, destination, _postProcessMaterial);
        }

        public void SetSaturation(float value) => saturation = Mathf.Clamp(value, 0f, 2f);
        public void SetContrast(float value) => contrast = Mathf.Clamp(value, 0.5f, 2f);
        public void SetBrightness(float value) => brightness = Mathf.Clamp(value, 0.5f, 2f);
        public void SetBloom(float intensity) => bloomIntensity = intensity;
        public void SetVignette(float intensity) => vignetteIntensity = intensity;
    }
}
