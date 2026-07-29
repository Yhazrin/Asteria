using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Editor settings configuration database for the game.
    /// Contains all Unity Editor settings.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Editor Settings Database")]
    public sealed class EditorSettingsDatabase : ScriptableObject
    {
        [Header("Editor")]
        public bool enterPlayModeOptionsEnabled = true;
        public bool reloadDomain = false;
        public bool reloadScene = false;

        [Header("Asset Pipeline")]
        public bool autoRefresh = true;
        public bool compressAssetsOnImport = true;
        public bool cacheServerEnabled = false;

        [Header("Version Control")]
        public string versionControlMode = "Visible Meta Files";
        public string assetSerializationMode = "Force Text";

        [Header("Other")]
        public bool showAssetStoreSearchInHierarchy = false;
        public bool enableTextureStreamingInEditMode = true;
    }
}
