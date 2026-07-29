using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Planet generator with procedural materials.
    /// </summary>
    public sealed class ProceduralPlanetGenerator18 : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float planetRadius = 300f;
        [SerializeField] int seed = 42;
        [SerializeField] int resolution = 128;

        [Header("References")]
        [SerializeField] PlanetBody planet;

        void Start()
        {
            if (planet == null)
                planet = FindFirstObjectByType<PlanetBody>();
        }

        /// <summary>
        /// Generate procedural material.
        /// </summary>
        public Material GenerateMaterial()
        {
            var shader = Shader.Find("Universal Render Pipeline/Lit")
                ?? Shader.Find("Universal Render Pipeline/Simple Lit")
                ?? Shader.Find("Standard");

            var mat = new Material(shader);
            mat.name = "ProceduralPlanetMaterial";

            // Set base color
            if (mat.HasProperty("_BaseColor"))
            {
                mat.SetColor("_BaseColor", new Color(0.4f, 0.6f, 0.3f));
            }

            // Set smoothness
            if (mat.HasProperty("_Smoothness"))
            {
                mat.SetFloat("_Smoothness", 0.2f);
            }

            return mat;
        }
    }
}
