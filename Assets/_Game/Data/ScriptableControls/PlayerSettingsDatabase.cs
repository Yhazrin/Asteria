using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Player settings database for the game.
    /// Contains all Unity PlayerSettings parameters.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Player Settings Database")]
    public sealed class PlayerSettingsDatabase : ScriptableObject
    {
        [Header("Display")]
        public string productName = "Asteria";
        public string companyName = "Yhazrin";
        public int defaultScreenWidth = 1920;
        public int defaultScreenHeight = 1080;
        public bool runInBackground = true;

        [Header("Rendering")]
        public ColorSpace colorSpace = ColorSpace.Linear;
        public string renderingPath = "Forward"; // Forward, Deferred
        public bool enableSRPBatcher = true;

        [Header("Scripting")]
        public string scriptingBackend = "IL2CPP";
        public string apiCompatibilityLevel = ".NET Standard 2.1";
        public bool allowUnsafeCode = false;

        [Header("Other")]
        public bool stripEngineCode = true;
        public bool optimizeMeshData = true;
    }
}
