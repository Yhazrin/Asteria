using System.Text.RegularExpressions;
using UnityEngine;

namespace Asteria.Data
{
    [CreateAssetMenu(fileName = "ObserveEntry", menuName = "Asteria/Discoveries/Observe Entry")]
    public sealed class ObserveEntry : ScriptableObject
    {
        static readonly Regex IdPattern = new(@"^[a-z][a-z0-9_]*(\.[a-z][a-z0-9_]*)*$", RegexOptions.Compiled);

        public string id = "observe_untitled";
        public string displayName = "未命名发现";
        [TextArea(2, 6)] public string description = "你注意到了什么。";
        public string promptText = "按 E 观察";

        /// <summary>
        /// Validates the ID format. Returns true if the ID is valid.
        /// Valid IDs: lowercase alphanumeric with underscores, optionally dot-separated.
        /// Examples: "wind_bell_stone", "observe.wind_bell_stone", "biome.wind_grassland"
        /// </summary>
        public bool IsIdValid()
        {
            return !string.IsNullOrWhiteSpace(id) && IdPattern.IsMatch(id);
        }

#if UNITY_EDITOR
        void OnValidate()
        {
            if (!IsIdValid())
            {
                Debug.LogWarning($"[Asteria] ObserveEntry '{name}' has invalid ID: '{id}'. " +
                    "IDs must be lowercase alphanumeric with underscores, optionally dot-separated.", this);
            }
        }
#endif
    }
}
