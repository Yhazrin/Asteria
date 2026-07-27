using UnityEngine;

namespace Asteria.Residents
{
    /// <summary>
    /// Static definition of a resident (星友). ScriptableObject asset.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Resident Definition")]
    public sealed class ResidentDefinition : ScriptableObject
    {
        [Header("Identity")]
        [SerializeField] string residentId = "resident_unnamed";
        [SerializeField] string displayName = "未命名";
        [SerializeField] string pronouns = "they/them";
        [TextArea(1, 3)] [SerializeField] string originDescription = "";

        [Header("Personality")]
        [Range(-1f, 1f)] [SerializeField] float sociability;
        [Range(-1f, 1f)] [SerializeField] float curiosity;
        [Range(-1f, 1f)] [SerializeField] float warmth;
        [Range(-1f, 1f)] [SerializeField] float order;
        [Range(-1f, 1f)] [SerializeField] float boldness;

        [Header("Quirks")]
        [SerializeField] string[] quirks = { };

        [Header("Color")]
        [SerializeField] Color bodyColor = new(0.9f, 0.85f, 0.8f);

        // Public read-only accessors
        public string ResidentId => residentId;
        public string DisplayName => displayName;
        public string Pronouns => pronouns;
        public string OriginDescription => originDescription;
        public float Sociability => sociability;
        public float Curiosity => curiosity;
        public float Warmth => warmth;
        public float Order => order;
        public float Boldness => boldness;
        public string[] Quirks => quirks;
        public Color BodyColor => bodyColor;

        /// <summary>
        /// Initialize a runtime-created ResidentDefinition.
        /// Only needed when creating definitions outside the Editor.
        /// </summary>
        public void InitializeRuntime(string id, string name, Color color,
            float soc = 0f, float cur = 0f, float war = 0f, float ord = 0f, float bol = 0f,
            string[] quirkList = null)
        {
            residentId = id;
            displayName = name;
            bodyColor = color;
            sociability = soc;
            curiosity = cur;
            warmth = war;
            order = ord;
            boldness = bol;
            quirks = quirkList ?? System.Array.Empty<string>();
        }
    }
}
