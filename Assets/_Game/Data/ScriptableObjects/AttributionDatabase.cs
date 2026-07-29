using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Attribution database for the game.
    /// Contains all asset attributions and credits.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Attribution Database")]
    public sealed class AttributionDatabase : ScriptableObject
    {
        [Header("Attributions")]
        public AttributionEntry[] entries = new AttributionEntry[]
        {
            new AttributionEntry
            {
                assetName = "Unity Built-in Resources",
                author = "Unity Technologies",
                license = "Unity Companion License",
                url = "https://unity.com"
            },
            new AttributionEntry
            {
                assetName = "Universal Render Pipeline Samples",
                author = "Unity Technologies",
                license = "Unity Companion License",
                url = "https://github.com/Unity-Technologies"
            },
        };
    }

    [System.Serializable]
    public class AttributionEntry
    {
        public string assetName;
        public string author;
        public string license;
        public string url;
    }
}
