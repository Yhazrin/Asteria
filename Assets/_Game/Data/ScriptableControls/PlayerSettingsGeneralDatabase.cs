using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// General player settings database for the game.
    /// Contains all Unity PlayerSettings parameters.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Player Settings General Database")]
    public sealed class PlayerSettingsGeneralDatabase : ScriptableObject
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
