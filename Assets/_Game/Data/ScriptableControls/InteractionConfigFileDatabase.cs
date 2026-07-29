using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Interaction configuration file database for the game.
    /// Contains all interaction parameters.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Interaction Config File Database")]
    public sealed class InteractionConfigFileDatabase : ScriptableObject
    {
        [Header("Detection")]
        public float interactionRadius = 3.5f;
        public KeyCode interactKey = KeyCode.E;
        public LayerMask interactMask = -1;

        [Header("Observe")]
        public float observeFocusDistance = 4.5f;
        public bool observeOneShot = true;

        [Header("Restore")]
        public float restoreDuration = 5f;
        public int restoreStages = 3;

        [Header("Cooperate")]
        public float cooperateDuration = 8f;
        public int requiredPlayers = 2;
    }
}
