using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Build configuration file database for the game.
    /// Contains all build parameters.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Build Config File Database")]
    public sealed class BuildConfigFileDatabase : ScriptableObject
    {
        [Header("Build")]
        public string productName = "Asteria";
        public string companyName = "Yhazrin";
        public string bundleIdentifier = "com.yhazrin.asteria";

        [Header("Settings")]
        public string scriptingBackend = "IL2CPP";
        public string apiCompatibilityLevel = ".NET Standard 2.1";
        public bool stripEngineCode = true;

        [Header("Optimization")]
        public bool optimizeMeshData = true;
        public bool vertexCompression = true;
        public bool textureCompression = true;
    }
}
