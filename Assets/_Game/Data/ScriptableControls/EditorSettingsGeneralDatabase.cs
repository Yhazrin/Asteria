using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// General editor settings database for the game.
    /// Contains all Unity Editor settings.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Editor Settings General Database")]
    public sealed class EditorSettingsGeneralDatabase : ScriptableObject
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
