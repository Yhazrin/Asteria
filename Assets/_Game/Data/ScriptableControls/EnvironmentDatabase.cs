using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Environment configuration database for the game.
    /// Contains all environment parameters.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Environment Database")]
    public sealed class EnvironmentDatabase : ScriptableObject
    {
        [Header("Sky")]
        public Color skyColorZenith = new(0.2f, 0.4f, 0.8f);
        public Color skyColorHorizon = new(0.6f, 0.7f, 0.9f);
        public float skyboxExposure = 1f;

        [Header("Fog")]
        public bool enableFog = true;
        public FogMode fogMode = FogMode.ExponentialSquared;
        public float fogDensity = 0.001f;
        public Color fogColor = new(0.55f, 0.68f, 0.82f);

        [Header("Ambient")]
        public Color ambientSkyColor = new(0.5f, 0.6f, 0.8f);
        public Color ambientEquatorColor = new(0.4f, 0.5f, 0.45f);
        public Color ambientGroundColor = new(0.2f, 0.18f, 0.15f);

        [Header("Wind")]
        public float windBaseSpeed = 2f;
        public float windGustFrequency = 0.3f;
        public float windDirectionVariation = 30f;
    }
}
