using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Input preset database for the game.
    /// Contains all input presets (default, accessibility, etc.).
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Input Preset Database")]
    public sealed class InputPresetDatabase : ScriptableObject
    {
        [Header("Presets")]
        public InputPreset[] presets = new InputPreset[]
        {
            new InputPreset
            {
                presetId = "default",
                displayName = "默认",
                description = "标准输入设置",
                mouseSensitivity = 2.4f,
                invertY = false,
                vibration = true
            },
            new InputPreset
            {
                presetId = "accessibility",
                displayName = "无障碍",
                description = "适合行动不便玩家的设置",
                mouseSensitivity = 1.2f,
                invertY = false,
                vibration = false
            },
            new InputPreset
            {
                presetId = "pro",
                displayName = "专业",
                description = "高灵敏度设置",
                mouseSensitivity = 4f,
                invertY = false,
                vibration = true
            },
        };
    }

    [System.Serializable]
    public class InputPreset
    {
        public string presetId;
        public string displayName;
        public string description;
        public float mouseSensitivity;
        public bool invertY;
        public bool vibration;
    }
}
