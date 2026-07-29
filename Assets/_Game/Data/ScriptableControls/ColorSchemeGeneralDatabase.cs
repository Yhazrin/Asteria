using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// General color scheme database for the game.
    /// Contains all color scheme parameters.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Color Scheme General Database")]
    public sealed class ColorSchemeGeneralDatabase : ScriptableObject
    {
        [Header("Primary Colors")]
        public Color primaryLight = new(0.4f, 0.6f, 0.8f);
        public Color primaryMain = new(0.3f, 0.5f, 0.7f);
        public Color primaryDark = new(0.2f, 0.3f, 0.5f);

        [Header("Secondary Colors")]
        public Color secondaryLight = new(0.8f, 0.7f, 0.5f);
        public Color secondaryMain = new(0.7f, 0.6f, 0.4f);
        public Color secondaryDark = new(0.5f, 0.4f, 0.3f);

        [Header("Accent Colors")]
        public Color accentLight = new(1f, 0.9f, 0.5f);
        public Color accentMain = new(0.95f, 0.85f, 0.4f);
        public Color accentDark = new(0.8f, 0.7f, 0.3f);

        [Header("Semantic Colors")]
        public Color successColor = new(0.3f, 0.8f, 0.4f);
        public Color warningColor = new(0.9f, 0.7f, 0.3f);
        public Color errorColor = new(0.8f, 0.3f, 0.3f);
        public Color infoColor = new(0.3f, 0.6f, 0.9f);
    }
}
