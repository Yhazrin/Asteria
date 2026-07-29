using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// UI settings database.
    /// Contains all UI parameters for the game.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/UI Settings Database")]
    public sealed class UISettingsDatabase : ScriptableObject
    {
        [Header("Layout")]
        public float hudPadding = 16f;
        public float panelPadding = 12f;
        public float buttonHeight = 40f;
        public float sliderHeight = 30f;

        [Header("Typography")]
        public int titleFontSize = 32;
        public int headerFontSize = 24;
        public int bodyFontSize = 18;
        public int smallFontSize = 14;
        public Color titleColor = new(0.95f, 0.85f, 0.4f);
        public Color bodyColor = Color.white;
        public Color subtitleColor = new(0.6f, 0.6f, 0.7f);

        [Header("Colors")]
        public Color backgroundColor = new(0.05f, 0.08f, 0.15f, 0.95f);
        public Color panelColor = new(0.1f, 0.12f, 0.18f, 0.95f);
        public Color buttonColor = new(0.3f, 0.5f, 0.7f);
        public Color accentColor = new(0.95f, 0.85f, 0.4f);
        public Color successColor = new(0.3f, 0.8f, 0.4f);
        public Color warningColor = new(0.9f, 0.7f, 0.3f);
        public Color errorColor = new(0.8f, 0.3f, 0.3f);

        [Header("Animation")]
        public float fadeSpeed = 2f;
        public float slideSpeed = 5f;
        public float bounceScale = 1.1f;
    }
}
