using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Dialogue configuration file database for the game.
    /// Contains all dialogue parameters.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Dialogue Config File Database")]
    public sealed class DialogueConfigFileDatabase : ScriptableObject
    {
        [Header("Dialogue")]
        public float typewriterSpeed = 0.03f;
        public float pagePauseTime = 0.5f;
        public float dialogueDuration = 5f;

        [Header("Bubble")]
        public float bubbleDuration = 4f;
        public float bubbleFadeSpeed = 2f;
        public float bubbleBobSpeed = 1.5f;
        public float bubbleBobHeight = 0.2f;

        [Header("UI")]
        public float panelWidth = 800f;
        public float panelHeight = 200f;
        public int titleFontSize = 20;
        public int bodyFontSize = 18;
    }
}
