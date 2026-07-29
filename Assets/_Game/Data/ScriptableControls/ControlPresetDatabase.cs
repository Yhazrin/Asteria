using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Control preset database for the game.
    /// Contains all control presets.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Control Preset Database")]
    public sealed class ControlPresetDatabase : ScriptableObject
    {
        [Header("Presets")]
        public ControlPreset[] presets = new ControlPreset[]
        {
            new ControlPreset
            {
                presetId = "default",
                displayName = "默认",
                description = "标准控制方案",
                mouseSensitivity = 2.4f,
                invertY = false,
                vibration = true,
                autoAim = false
            },
            new ControlPreset
            {
                presetId = "southpaw",
                displayName = "左手",
                description = "左手操作方案",
                mouseSensitivity = 2.4f,
                invertY = false,
                vibration = true,
                autoAim = false
            },
            new ControlPreset
            {
                presetId = "accessibility",
                displayName = "无障碍",
                description = "无障碍控制方案",
                mouseSensitivity = 1.2f,
                invertY = false,
                vibration = false,
                autoAim = true
            },
        };
    }

    [System.Serializable]
    public class ControlPreset
    {
        public string presetId;
        public string displayName;
        public string description;
        public float mouseSensitivity;
        public bool invertY;
        public bool vibration;
        public bool autoAim;
    }
}
