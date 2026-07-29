using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Notification configuration file database for the game.
    /// Contains all notification parameters.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Notification Config File Database")]
    public sealed class NotificationConfigFileDatabase : ScriptableObject
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
