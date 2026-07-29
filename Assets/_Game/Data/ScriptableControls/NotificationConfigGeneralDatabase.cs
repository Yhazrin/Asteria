using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// General notification configuration database for the game.
    /// Contains all notification parameters.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Notification Config General Database")]
    public sealed class NotificationConfigGeneralDatabase : ScriptableObject
    {
        [Header("Toast")]
        public float toastDuration = 3f;
        public int maxToasts = 5;

        [Header("Popup")]
        public float popupDuration = 5f;
        public float popupFadeSpeed = 2f;

        [Header("Achievement")]
        public float achievementDuration = 5f;
        public float achievementFadeSpeed = 2f;
    }
}
