using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Audio preset database for the game.
    /// Contains all audio presets.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Audio Preset Database")]
    public sealed class AudioPresetDatabase : ScriptableObject
    {
        [Header("Presets")]
        public AudioPreset[] presets = new AudioPreset[]
        {
            new AudioPreset
            {
                presetId = "default",
                displayName = "默认",
                description = "标准音频设置",
                masterVolume = 1f,
                musicVolume = 0.7f,
                sfxVolume = 0.8f,
                ambientVolume = 0.6f
            },
            new AudioPreset
            {
                presetId = "quiet",
                displayName = "安静",
                description = "低音量设置",
                masterVolume = 0.5f,
                musicVolume = 0.4f,
                sfxVolume = 0.5f,
                ambientVolume = 0.3f
            },
            new AudioPreset
            {
                presetId = "loud",
                displayName = "响亮",
                description = "高音量设置",
                masterVolume = 1f,
                musicVolume = 0.9f,
                sfxVolume = 1f,
                ambientVolume = 0.8f
            },
        };
    }

    [System.Serializable]
    public class AudioPreset
    {
        public string presetId;
        public string displayName;
        public string description;
        public float masterVolume;
        public float musicVolume;
        public float sfxVolume;
        public float ambientVolume;
    }
}
