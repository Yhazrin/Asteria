using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// General build settings database for the game.
    /// Contains all Unity BuildSettings parameters.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Build Settings General Database")]
    public sealed class BuildSettingsGeneralDatabase : ScriptableObject
    {
        [Header("Scenes")]
        public string[] scenes = {
            "Assets/_Game/Core/Scenes/Bootstrap.unity",
            "Assets/_Game/Core/Scenes/HomePlanet.unity",
            "Assets/_Game/Planet/Scenes/SphereMoveDemo.unity"
        };

        [Header("Platform")]
        public string targetPlatform = "StandaloneWindows64";

        [Header("Options")]
        public bool developmentBuild = false;
        public bool scriptDebugging = false;
        public bool compressAssets = true;
    }
}
