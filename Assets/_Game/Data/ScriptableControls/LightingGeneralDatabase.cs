using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// General lighting database for the game.
    /// Contains all lighting parameters.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Lighting General Database")]
    public sealed class LightingGeneralDatabase : ScriptableObject
    {
        [Header("Directional Light")]
        public Color sunColor = new(1f, 0.95f, 0.85f);
        public float sunIntensity = 1.2f;
        public float sunShadowStrength = 0.7f;
        public float sunShadowDistance = 200f;

        [Header("Ambient")]
        public AmbientMode ambientMode = AmbientMode.Trilight;
        public Color ambientSkyColor = new(0.5f, 0.6f, 0.8f);
        public Color ambientEquatorColor = new(0.4f, 0.5f, 0.45f);
        public Color ambientGroundColor = new(0.2f, 0.18f, 0.15f);

        [Header("Lightmapping")]
        public bool enableBakedLighting = true;
        public bool enableRealtimeLighting = true;
    }
}
