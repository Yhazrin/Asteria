using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Build settings configuration database for the game.
    /// Contains all Unity BuildSettings parameters.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Build Settings Database")]
    public sealed class BuildSettingsDatabase : ScriptableObject
    {
        [Header("Scenes")]
        public string[] scenes = {
            "Assets/_Game/Core/Scenes/Bootstrap.unity",
            "Assets/_Game/Core/Scenes/HomePlanet.unity",
            "Assets/_Game/Planet/Scenes/SphereMoveDemo.unity"
        };

        [Header("Platform")]
        public string targetPlatform = "StandaloneWindows64";
        public string buildTarget = "StandaloneWindows64";

        [Header("Options")]
        public bool developmentBuild = false;
        public bool scriptDebugging = false;
        public bool deepProfiling = false;
        public bool compressAssets = true;
    }
}
