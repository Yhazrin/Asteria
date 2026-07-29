using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Editor settings file database for the game.
    /// Contains all Unity Editor settings.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Editor Settings File Database")]
    public sealed class EditorSettingsFileDatabase : ScriptableObject
    {
        [Header("Editor")]
        public bool enterPlayModeOptionsEnabled = true;
        public bool reloadDomain = false;
        public bool reloadScene = false;

        [Header("Asset Pipeline")]
        public bool autoRefresh = true;
        public bool compressAssetsOnImport = true;

        [Header("Version Control")]
        public string versionControlMode = "Visible Meta Files";
        public string assetSerializationMode = "Force Text";
    }
}
