using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// License database for the game.
    /// Contains all third-party licenses and attributions.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/License Database")]
    public sealed class LicenseDatabase : ScriptableObject
    {
        [Header("Licenses")]
        public LicenseEntry[] licenses = new LicenseEntry[]
        {
            new LicenseEntry
            {
                name = "Unity",
                version = "6000.5.5f1",
                license = "Unity Companion License",
                url = "https://unity.com/legal/licenses/unity-companion-license"
            },
            new LicenseEntry
            {
                name = "Universal Render Pipeline",
                version = "17.5.0",
                license = "Unity Companion License",
                url = "https://unity.com/legal/licenses/unity-companion-license"
            },
            new LicenseEntry
            {
                name = "TextMeshPro",
                version = "3.2.0",
                license = "Unity Companion License",
                url = "https://unity.com/legal/licenses/unity-companion-license"
            },
            new LicenseEntry
            {
                name = "Netcode for GameObjects",
                version = "2.0.0",
                license = "Unity Companion License",
                url = "https://unity.com/legal/licenses/unity-companion-license"
            },
        };
    }

    [System.Serializable]
    public class LicenseEntry
    {
        public string name;
        public string version;
        public string license;
        public string url;
    }
}
