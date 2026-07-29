using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Game design parameters database.
    /// Contains all game design values for the game.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Game Design Database")]
    public sealed class GameDesignDatabase : ScriptableObject
    {
        [Header("Core Loop")]
        public float explorationFeedbackInterval = 30f;
        public float expeditionTargetDuration = 25f;
        public float homeReturnRewardMultiplier = 1.5f;

        [Header("Survival")]
        public float pressureWarningDuration = 10f;
        public float pressureActiveDuration = 180f;
        public float rescueTimeout = 30f;
        public float shelterRecoveryTime = 5f;

        [Header("Social")]
        public float relationshipDecayRate = 0.01f;
        public float moodUpdateInterval = 5f;
        public float dialogueBubbleDuration = 4f;
        public float interactionRange = 4f;

        [Header("Building")]
        public int maxLargeAnchors = 6;
        public int maxMediumAnchors = 12;
        public int maxSmallAnchors = 20;
        public float buildCooldown = 1f;

        [Header("Multiplayer")]
        public int maxPlayers = 4;
        public float syncRate = 20f;
        public float reconnectTimeout = 30f;
        public float checkpointInterval = 30f;
    }
}
