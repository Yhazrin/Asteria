using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// General layout database for the game.
    /// Contains all layout parameters.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Layout General Database")]
    public sealed class LayoutGeneralDatabase : ScriptableObject
    {
        [Header("Canvas")]
        public float referenceWidth = 1920f;
        public float referenceHeight = 1080f;
        public float matchWidthOrHeight = 0.5f;

        [Header("HUD Layout")]
        public float hudPadding = 16f;
        public float hudSpacing = 8f;
        public float hudFontSize = 18f;

        [Header("Panel Layout")]
        public float panelPadding = 12f;
        public float panelSpacing = 8f;
        public float panelBorderRadius = 8f;

        [Header("Button Layout")]
        public float buttonHeight = 40f;
        public float buttonPadding = 8f;
        public float buttonSpacing = 8f;
    }
}
