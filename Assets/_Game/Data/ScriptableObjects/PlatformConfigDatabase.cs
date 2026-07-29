using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Platform configuration database for the game.
    /// Contains all platform-specific settings.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Platform Config Database")]
    public sealed class PlatformConfigDatabase : ScriptableObject
    {
        [Header("Windows")]
        public bool windowsFullscreen = true;
        public int windowsResolutionX = 1920;
        public int windowsResolutionY = 1080;
        public int windowsRefreshRate = 60;

        [Header("macOS")]
        public bool macFullscreen = true;
        public int macResolutionX = 1920;
        public int macResolutionY = 1080;
        public int macRefreshRate = 60;

        [Header("Linux")]
        public bool linuxFullscreen = true;
        public int linuxResolutionX = 1920;
        public int linuxResolutionY = 1080;
        public int linuxRefreshRate = 60;

        [Header("Common")]
        public bool enableVSync = true;
        public int targetFrameRate = 60;
        public int qualityLevel = 2;
    }
}
