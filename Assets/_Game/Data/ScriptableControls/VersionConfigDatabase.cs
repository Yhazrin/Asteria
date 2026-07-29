using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Version configuration database for the game.
    /// Contains all version parameters.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Version Config Database")]
    public sealed class VersionConfigDatabase : ScriptableObject
    {
        [Header("Version")]
        public string majorVersion = "0";
        public string minorVersion = "1";
        public string patchVersion = "0";
        public string preReleaseTag = "alpha";

        [Header("Build")]
        public string buildNumber = "1";
        public string buildDate = "2026-07-28";
        public string buildHash = "";

        [Header("Compatibility")]
        public int saveSchemaVersion = 1;
        public int apiVersion = 1;
        public int contentVersion = 1;
    }
}
