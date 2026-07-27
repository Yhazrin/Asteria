using UnityEngine;

namespace Asteria
{
    /// <summary>
    /// Shared utility for creating simple URP materials at runtime.
    /// Eliminates duplicated shader-lookup and material-setup code.
    /// </summary>
    public static class MaterialHelper
    {
        const string URPShaderName = "Universal Render Pipeline/Lit";
        const string FallbackShaderName = "Sprites/Default";

        static Shader _cachedShader;

        /// <summary>
        /// Get the URP Lit shader (or fallback). Cached after first lookup.
        /// </summary>
        public static Shader GetURPShader()
        {
            if (_cachedShader == null)
            {
                _cachedShader = Shader.Find(URPShaderName)
                                ?? Shader.Find(FallbackShaderName)
                                ?? Shader.Find("Standard");
            }

            return _cachedShader;
        }

        /// <summary>
        /// Create a simple colored material using URP Lit shader.
        /// </summary>
        public static Material CreateSimpleMaterial(Color color)
        {
            Shader shader = GetURPShader();
            Material mat = new(shader);
            if (mat.HasProperty("_BaseColor"))
            {
                mat.SetColor("_BaseColor", color);
            }

            mat.color = color;
            return mat;
        }

        /// <summary>
        /// Apply a simple color to a renderer's material.
        /// </summary>
        public static void ApplyColor(Renderer renderer, Color color)
        {
            if (renderer == null)
            {
                return;
            }

            Material mat = renderer.sharedMaterial;
            if (mat == null)
            {
                mat = CreateSimpleMaterial(color);
                renderer.sharedMaterial = mat;
            }

            if (mat.HasProperty("_BaseColor"))
            {
                mat.SetColor("_BaseColor", color);
            }

            mat.color = color;
        }
    }
}
