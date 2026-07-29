using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Release configuration database for the game.
    /// Contains all release parameters.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Release Config Database")]
    public sealed class ReleaseConfigDatabase : ScriptableObject
    {
        [Header("Release")]
        public string releaseVersion = "0.1.0";
        public string releaseDate = "2026-07-28";
        public string releaseChannel = "alpha"; // alpha, beta, stable

        [Header("Notes")]
        public string releaseTitle = "Alpha 初始版本";
        [TextArea(3, 6)] public string releaseNotes = "首个可测试版本";

        [Header("Requirements")]
        public string minUnityVersion = "6000.5.5f1";
        public string minOS = "Windows 10";
        public int minRAM = 8;
    }
}
