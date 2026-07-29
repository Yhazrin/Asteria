using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Deployment configuration database for the game.
    /// Contains all deployment parameters.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Deployment Config Database")]
    public sealed class DeploymentConfigDatabase : ScriptableObject
    {
        [Header("Deployment")]
        public string platform = "StandaloneWindows64";
        public string buildTarget = "StandaloneWindows64";
        public string outputPath = "Builds";

        [Header("Build")]
        public bool developmentBuild = false;
        public bool scriptDebugging = false;
        public bool deepProfiling = false;
        public bool compressAssets = true;

        [Header("Distribution")]
        public string distributionChannel = "direct"; // direct, steam, epic
        public string storeId = "";
    }
}
