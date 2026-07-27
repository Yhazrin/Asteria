using UnityEngine;

namespace Asteria.Residents
{
    /// <summary>
    /// Preset personality template for resident creation.
    /// Provides default values for the 5 personality dimensions.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Personality Preset")]
    public sealed class PersonalityPreset : ScriptableObject
    {
        public string presetId = "preset_default";
        public string displayName = "默认性格";

        [Range(-1f, 1f)] public float sociability;
        [Range(-1f, 1f)] public float curiosity;
        [Range(-1f, 1f)] public float warmth;
        [Range(-1f, 1f)] public float order;
        [Range(-1f, 1f)] public float boldness;

        /// <summary>
        /// Apply this preset to a ResidentDefinition's runtime initialization.
        /// </summary>
        public void ApplyTo(ResidentDefinition def)
        {
            def.InitializeRuntime(def.ResidentId, def.DisplayName, def.BodyColor,
                soc: sociability, cur: curiosity, war: warmth, ord: order, bol: boldness,
                quirkList: def.Quirks);
        }
    }
}
