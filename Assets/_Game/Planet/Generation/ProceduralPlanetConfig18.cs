using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Configuration with material parameters.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Procedural Planet Config 18")]
    public sealed class ProceduralPlanetConfig18 : ScriptableObject
    {
        [Header("Planet")]
        public string planetName = "New Planet";
        public float planetRadius = 300f;
        public int seed = 42;

        [Header("Material")]
        public Color baseColor = new(0.4f, 0.6f, 0.3f);
        public float smoothness = 0.2f;
        public float metallic = 0f;

        [Header("Mesh")]
        public int resolution = 128;
        public float meshScale = 1f;

        /// <summary>
        /// Generate material from config.
        /// </summary>
        public Material GenerateMaterial()
        {
            var shader = Shader.Find("Universal Render Pipeline/Lit")
                ?? Shader.Find("Universal Render Pipeline/Simple Lit")
                ?? Shader.Find("Standard");

            var mat = new Material(shader);
            mat.name = $"{planetName}_Material";

            if (mat.HasProperty("_BaseColor")) mat.SetColor("_BaseColor", baseColor);
            if (mat.HasProperty("_Smoothness")) mat.SetFloat("_Smoothness", smoothness);
            if (mat.HasProperty("_Metallic")) mat.SetFloat("_Metallic", metallic);

            return mat;
        }
    }
}
