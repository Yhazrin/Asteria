using UnityEngine;

namespace Asteria.Residents
{
    /// <summary>
    /// Defines a discrete quirk (怪癖) that adds memorable personality flavor.
    /// Quirks trigger specific behavioral modifiers during events.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Quirk Definition")]
    public sealed class QuirkDefinition : ScriptableObject
    {
        public string quirkId = "quirk_default";
        public string displayName = "怪癖";
        [TextArea(1, 3)] public string description = "";

        [Header("Triggers")]
        public string[] triggerTags = { };

        [Header("Behavior")]
        public string[] behaviorModifiers = { };
    }
}
