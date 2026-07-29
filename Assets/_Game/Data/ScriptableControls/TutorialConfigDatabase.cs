using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Tutorial configuration database for the game.
    /// Contains all tutorial parameters.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Tutorial Config Database")]
    public sealed class TutorialConfigDatabase : ScriptableObject
    {
        [Header("Tutorial")]
        public bool showTutorialOnFirstPlay = true;
        public float messageDuration = 5f;
        public float fadeSpeed = 2f;

        [Header("Steps")]
        public int totalSteps = 8;
        public float autoAdvanceDelay = 4f;

        [Header("UI")]
        public float panelWidth = 600f;
        public float panelHeight = 120f;
        public int fontSize = 20;
    }
}
