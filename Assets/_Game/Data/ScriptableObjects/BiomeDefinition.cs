using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Defines a biome (生态区) within a planet.
    /// Contains visual, audio, and gameplay parameters.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Biome Definition")]
    public sealed class BiomeDefinition : ScriptableObject
    {
        public string biomeId = "biome_default";
        public string displayName = "生态区";
        public BiomeType biomeType = BiomeType.Wind;

        [Header("Tags")]
        public string[] moodTags = { };
        public string[] pressureTypes = { };

        [Header("Visual")]
        public Color ambientColor = new(0.55f, 0.7f, 0.9f);
        public string[] decorationSets = { };

        [Header("Audio")]
        public string ambientSoundId;
    }

    public enum BiomeType
    {
        Wind,
        Mist,
        Night,
        Ice,
        Bloom,
        Ruin
    }
}
