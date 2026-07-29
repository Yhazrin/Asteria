using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Social configuration database for the game.
    /// Contains all social parameters.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Social Config Database")]
    public sealed class SocialConfigDatabase : ScriptableObject
    {
        [Header("Relationships")]
        public float affinityDecayRate = 0.001f;
        public float trustDecayRate = 0.0005f;
        public float tensionDecayRate = 0.01f;

        [Header("Needs")]
        public float safetyDecayRate = 0.01f;
        public float socialDecayRate = 0.015f;
        public float solitudeDecayRate = 0.02f;
        public float expressionDecayRate = 0.01f;
        public float explorationDecayRate = 0.008f;

        [Header("Events")]
        public float eventCooldownDays = 1f;
        public float wishCooldownDays = 5f;

        [Header("Bubble")]
        public float dialogueBubbleDuration = 4f;
        public float moodBubbleDuration = 3f;
    }
}
