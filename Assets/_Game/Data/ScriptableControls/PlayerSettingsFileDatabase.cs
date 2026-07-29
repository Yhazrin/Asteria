using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Player settings file database for the game.
    /// Contains all Unity PlayerSettings parameters.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Player Settings File Database")]
    public sealed class PlayerSettingsFileDatabase : ScriptableObject
    {
        [Header("Display")]
        public string productName = "Asteria";
        public string companyName = "Yhazrin";
        public int defaultScreenWidth = 1920;
        public int defaultScreenHeight = 1080;
        public bool runInBackground = true;

        [Header("Rendering")]
        public ColorSpace colorSpace = ColorSpace.Linear;
        public bool enableSRPBatcher = true;

        [Header("Scripting")]
        public string scriptingBackend = "IL2CPP";
        public bool allowUnsafeCode = false;
    }
}
