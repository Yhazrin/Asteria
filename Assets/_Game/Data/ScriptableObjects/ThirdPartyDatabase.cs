using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Third-party dependency database for the game.
    /// Contains all third-party dependencies and their versions.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Third Party Database")]
    public sealed class ThirdPartyDatabase : ScriptableObject
    {
        [Header("Dependencies")]
        public DependencyEntry[] dependencies = new DependencyEntry[]
        {
            new DependencyEntry
            {
                name = "com.unity.render-pipelines.universal",
                version = "17.5.0",
                type = "rendering",
                required = true
            },
            new DependencyEntry
            {
                name = "com.unity.multiplayer.center",
                version = "1.0.1",
                type = "networking",
                required = false
            },
            new DependencyEntry
            {
                name = "com.unity.ai.assistant",
                version = "2.16.0-pre.1",
                type = "editor",
                required = false
            },
            new DependencyEntry
            {
                name = "com.unity.ai.inference",
                version = "2.6.1",
                type = "editor",
                required = false
            },
        };
    }

    [System.Serializable]
    public class DependencyEntry
    {
        public string name;
        public string version;
        public string type;
        public bool required;
    }
}
