using UnityEngine;

namespace Asteria.Data
{
    [CreateAssetMenu(fileName = "ObserveEntry", menuName = "Asteria/Discoveries/Observe Entry")]
    public sealed class ObserveEntry : ScriptableObject
    {
        public string id = "observe_untitled";
        public string displayName = "未命名发现";
        [TextArea(2, 6)] public string description = "你注意到了什么。";
        public string promptText = "按 E 观察";
    }
}
