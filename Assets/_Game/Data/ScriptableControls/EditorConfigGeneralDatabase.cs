using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// General editor configuration database for the game.
    /// Contains all editor parameters.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Editor Config General Database")]
    public sealed class EditorConfigGeneralDatabase : ScriptableObject
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

        [Header("Tools")]
        public bool enableEditorTools = true;
        public bool enableValidators = true;
        public bool enableDebuggers = true;
    }
}
