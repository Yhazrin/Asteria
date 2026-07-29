using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Typography configuration database for the game.
    /// Contains all typography parameters.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Typography Database")]
    public sealed class TypographyDatabase : ScriptableObject
    {
        [Header("Font Sizes")]
        public int displaySize = 48;
        public int h1Size = 32;
        public int h2Size = 24;
        public int h3Size = 20;
        public int bodySize = 16;
        public int smallSize = 14;
        public int captionSize = 12;

        [Header("Line Heights")]
        public float displayLineHeight = 1.2f;
        public float h1LineHeight = 1.3f;
        public float h2LineHeight = 1.3f;
        public float h3LineHeight = 1.4f;
        public float bodyLineHeight = 1.5f;
        public float smallLineHeight = 1.5f;

        [Header("Font Weights")]
        public int lightWeight = 300;
        public int regularWeight = 400;
        public int mediumWeight = 500;
        public int boldWeight = 700;
    }
}
