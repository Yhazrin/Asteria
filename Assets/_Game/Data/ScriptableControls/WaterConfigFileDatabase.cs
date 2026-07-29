using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Water configuration file database for the game.
    /// Contains all water parameters.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Water Config File Database")]
    public sealed class WaterConfigFileDatabase : ScriptableObject
    {
        [Header("Water")]
        public float waterLevel = 0.4f;
        public int waterResolution = 64;
        public Color shallowColor = new(0.3f, 0.6f, 0.8f, 0.6f);
        public Color deepColor = new(0.1f, 0.2f, 0.4f, 0.8f);

        [Header("Waves")]
        public float waveSpeed = 1f;
        public float waveHeight = 0.5f;
        public float waveFrequency = 0.1f;

        [Header("Properties")]
        public float smoothness = 0.8f;
        public float metallic = 0.1f;
    }
}
