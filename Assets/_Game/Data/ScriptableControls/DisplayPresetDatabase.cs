using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Display preset database for the game.
    /// Contains all display presets.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Display Preset Database")]
    public sealed class DisplayPresetDatabase : ScriptableObject
    {
        [Header("Presets")]
        public DisplayPreset[] presets = new DisplayPreset[]
        {
            new DisplayPreset
            {
                presetId = "windowed",
                displayName = "窗口化",
                description = "窗口模式",
                fullscreen = false,
                resolution = "1920x1080",
                refreshRate = 60
            },
            new DisplayPreset
            {
                presetId = "fullscreen",
                displayName = "全屏",
                description = "全屏模式",
                fullscreen = true,
                resolution = "1920x1080",
                refreshRate = 60
            },
            new DisplayPreset
            {
                presetId = "borderless",
                displayName = "无边框窗口",
                description = "无边框窗口模式",
                fullscreen = false,
                resolution = "1920x1080",
                refreshRate = 60
            },
        };
    }

    [System.Serializable]
    public class DisplayPreset
    {
        public string presetId;
        public string displayName;
        public string description;
        public bool fullscreen;
        public string resolution;
        public int refreshRate;
    }
}
