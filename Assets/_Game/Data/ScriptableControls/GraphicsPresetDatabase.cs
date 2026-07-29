using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Graphics preset database for the game.
    /// Contains all graphics presets (low, medium, high, ultra).
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Graphics Preset Database")]
    public sealed class GraphicsPresetDatabase : ScriptableObject
    {
        [Header("Presets")]
        public GraphicsPreset[] presets = new GraphicsPreset[]
        {
            new GraphicsPreset
            {
                presetId = "low",
                displayName = "低",
                description = "适合低端硬件",
                qualityLevel = 0,
                resolution = "1280x720",
                shadowQuality = "低",
                textureQuality = "低",
                antiAliasing = "关闭",
                postProcessing = false
            },
            new GraphicsPreset
            {
                presetId = "medium",
                displayName = "中",
                description = "平衡性能和画质",
                qualityLevel = 1,
                resolution = "1920x1080",
                shadowQuality = "中",
                textureQuality = "中",
                antiAliasing = "FXAA",
                postProcessing = true
            },
            new GraphicsPreset
            {
                presetId = "high",
                displayName = "高",
                description = "高画质",
                qualityLevel = 2,
                resolution = "1920x1080",
                shadowQuality = "高",
                textureQuality = "高",
                antiAliasing = "MSAA 4x",
                postProcessing = true
            },
            new GraphicsPreset
            {
                presetId = "ultra",
                displayName = "极高",
                description = "最高画质",
                qualityLevel = 3,
                resolution = "2560x1440",
                shadowQuality = "极高",
                textureQuality = "极高",
                antiAliasing = "MSAA 8x",
                postProcessing = true
            },
        };
    }

    [System.Serializable]
    public class GraphicsPreset
    {
        public string presetId;
        public string displayName;
        public string description;
        public int qualityLevel;
        public string resolution;
        public string shadowQuality;
        public string textureQuality;
        public string antiAliasing;
        public bool postProcessing;
    }
}
