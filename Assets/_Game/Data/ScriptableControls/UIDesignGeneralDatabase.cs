using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// General UI design database for the game.
    /// Contains all UI design parameters.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/UI Design General Database")]
    public sealed class UIDesignGeneralDatabase : ScriptableObject
    {
        [Header("Typography")]
        public int titleFontSize = 32;
        public int headerFontSize = 24;
        public int bodyFontSize = 18;
        public int smallFontSize = 14;

        [Header("Colors")]
        public Color primaryColor = new(0.3f, 0.5f, 0.7f);
        public Color secondaryColor = new(0.5f, 0.5f, 0.5f);
        public Color accentColor = new(0.95f, 0.85f, 0.4f);
        public Color backgroundColor = new(0.05f, 0.08f, 0.15f);
        public Color surfaceColor = new(0.1f, 0.12f, 0.18f);

        [Header("Spacing")]
        public float spacingXS = 4f;
        public float spacingS = 8f;
        public float spacingM = 16f;
        public float spacingL = 24f;
        public float spacingXL = 32f;

        [Header("Border Radius")]
        public float borderRadiusSmall = 4f;
        public float borderRadiusMedium = 8f;
        public float borderRadiusLarge = 12f;
    }
}
