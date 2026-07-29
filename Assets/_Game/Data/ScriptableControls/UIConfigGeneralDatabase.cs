using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// General UI configuration database for the game.
    /// Contains all UI parameters.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/UI Config General Database")]
    public sealed class UIConfigGeneralDatabase : ScriptableObject
    {
        [Header("Canvas")]
        public float referenceWidth = 1920f;
        public float referenceHeight = 1080f;
        public float matchWidthOrHeight = 0.5f;

        [Header("HUD")]
        public float hudPadding = 16f;
        public float hudFontSize = 18f;

        [Header("Panels")]
        public float panelPadding = 12f;
        public float panelBorderRadius = 8f;

        [Header("Buttons")]
        public float buttonHeight = 40f;
        public float buttonPadding = 8f;

        [Header("Typography")]
        public int titleFontSize = 32;
        public int headerFontSize = 24;
        public int bodyFontSize = 18;
        public int smallFontSize = 14;
    }
}
