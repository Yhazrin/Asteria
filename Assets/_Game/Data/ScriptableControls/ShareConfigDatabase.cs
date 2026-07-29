using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Share configuration database for the game.
    /// Contains all share parameters.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Share Config Database")]
    public sealed class ShareConfigDatabase : ScriptableObject
    {
        [Header("Share")]
        public bool enableSharing = true;
        public string[] shareTargets = { "Twitter", "Discord", "Copy Link" };

        [Header("Content")]
        public bool shareScreenshots = true;
        public bool shareStats = true;
        public bool shareAchievements = true;

        [Header("Format")]
        public string imageFormat = "png";
        public int imageQuality = 90;
        public string shareMessageTemplate = "我在 Asteria 中{achievement}！";
    }
}
