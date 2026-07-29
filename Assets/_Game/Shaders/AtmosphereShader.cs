using UnityEngine;

namespace Asteria.Shaders
{
    /// <summary>
    /// Runtime atmosphere shader generator.
    /// Creates a procedural atmosphere effect using Unity's rendering.
    /// </summary>
    public static class AtmosphereShader
    {
        /// <summary>
        /// Create an atmosphere material with the given parameters.
        /// </summary>
        public static Material CreateAtmosphereMaterial(Color color, float intensity, float radius)
        {
            // Use URP Unlit with transparency for atmosphere
            var shader = Shader.Find("Universal Render Pipeline/Unlit")
                ?? Shader.Find("Sprites/Default");

            var mat = new Material(shader);
            mat.name = "M_Atmosphere_Procedural";

            // Set color with transparency
            Color atmosColor = color;
            atmosColor.a = intensity * 0.3f;
            mat.color = atmosColor;

            // Enable transparency
            if (mat.HasProperty("_Surface"))
            {
                mat.SetFloat("_Surface", 1); // Transparent
            }
            if (mat.HasProperty("_Blend"))
            {
                mat.SetFloat("_Blend", 0); // Alpha blend
            }

            mat.renderQueue = 3100; // After opaque, before UI
            mat.enableInstancing = true;

            return mat;
        }

        /// <summary>
        /// Create a skybox material with procedural gradient.
        /// </summary>
        public static Material CreateSkyboxMaterial(Color zenith, Color horizon)
        {
            var shader = Shader.Find("Skybox/Procedural")
                ?? Shader.Find("Universal Render Pipeline/Unlit");

            var mat = new Material(shader);
            mat.name = "M_Skybox_Procedural";

            if (mat.HasProperty("_SkyTint"))
            {
                mat.SetColor("_SkyTint", zenith);
            }
            if (mat.HasProperty("_GroundColor"))
            {
                mat.SetColor("_GroundColor", horizon);
            }
            if (mat.HasProperty("_Exposure"))
            {
                mat.SetFloat("_Exposure", 1.2f);
            }

            return mat;
        }

        /// <summary>
        /// Create a water surface material.
        /// </summary>
        public static Material CreateWaterMaterial(Color shallow, Color deep)
        {
            var shader = Shader.Find("Universal Render Pipeline/Lit")
                ?? Shader.Find("Standard");

            var mat = new Material(shader);
            mat.name = "M_Water_Procedural";

            SetColor(mat, shallow);
            if (mat.HasProperty("_Smoothness")) mat.SetFloat("_Smoothness", 0.85f);
            if (mat.HasProperty("_Metallic")) mat.SetFloat("_Metallic", 0.1f);

            // Enable transparency
            if (mat.HasProperty("_Surface"))
            {
                mat.SetFloat("_Surface", 1);
            }

            mat.renderQueue = 3000;
            return mat;
        }

        /// <summary>
        /// Create a cloud material.
        /// </summary>
        public static Material CreateCloudMaterial(Color color)
        {
            var shader = Shader.Find("Universal Render Pipeline/Unlit")
                ?? Shader.Find("Sprites/Default");

            var mat = new Material(shader);
            mat.name = "M_Cloud_Procedural";

            Color cloudColor = color;
            cloudColor.a = 0.6f;
            mat.color = cloudColor;

            mat.renderQueue = 3200;
            return mat;
        }

        /// <summary>
        /// Create a fog material for atmospheric scattering.
        /// </summary>
        public static Material CreateFogMaterial(Color fogColor, float density)
        {
            var shader = Shader.Find("Universal Render Pipeline/Unlit")
                ?? Shader.Find("Sprites/Default");

            var mat = new Material(shader);
            mat.name = "M_Fog_Procedural";

            Color c = fogColor;
            c.a = Mathf.Clamp01(density);
            mat.color = c;

            mat.renderQueue = 3300;
            return mat;
        }

        static void SetColor(Material mat, Color color)
        {
            if (mat.HasProperty("_BaseColor")) mat.SetColor("_BaseColor", color);
            if (mat.HasProperty("_Color")) mat.SetColor("_Color", color);
            mat.color = color;
        }
    }
}
