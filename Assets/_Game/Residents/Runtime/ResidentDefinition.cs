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
        public string residentId = "resident_unnamed";
        public string displayName = "未命名";
        public string pronouns = "they/them";
        [TextArea(1, 3)] public string originDescription = "";

        [Header("Personality")]
        [Range(-1f, 1f)] public float sociability;   // 独处 ↔ 合群
        [Range(-1f, 1f)] public float curiosity;     // 稳定 ↔ 探索
        [Range(-1f, 1f)] public float warmth;         // 克制 ↔ 热情
        [Range(-1f, 1f)] public float order;          // 随性 ↔ 计划
        [Range(-1f, 1f)] public float boldness;       // 谨慎 ↔ 冒险

        [Header("Quirks")]
        public string[] quirks = { };

        [Header("Color")]
        public Color bodyColor = new(0.9f, 0.85f, 0.8f);
    }
}
