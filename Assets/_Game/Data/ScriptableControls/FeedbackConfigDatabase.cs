using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Feedback configuration database for the game.
    /// Contains all feedback parameters.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Feedback Config Database")]
    public sealed class FeedbackConfigDatabase : ScriptableObject
    {
        [Header("Feedback")]
        public bool enableFeedback = true;
        public string feedbackEndpoint = "";
        public bool allowScreenshots = true;
        public bool allowLogs = true;

        [Header("UI")]
        public string feedbackButtonLabel = "反馈";
        public string feedbackFormTitle = "提交反馈";
        public string[] feedbackCategories = { "Bug", "建议", "其他" };
    }
}
