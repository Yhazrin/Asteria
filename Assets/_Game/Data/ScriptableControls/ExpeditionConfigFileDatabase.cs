using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Expedition configuration file database for the game.
    /// Contains all expedition parameters.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Expedition Config File Database")]
    public sealed class ExpeditionConfigFileDatabase : ScriptableObject
    {
        [Header("Expedition")]
        public float targetDuration = 25f; // minutes
        public int minPlayers = 1;
        public int maxPlayers = 4;

        [Header("Scoring")]
        public float discoveryWeight = 10f;
        public float restoreWeight = 20f;
        public float cooperateWeight = 30f;
        public float rescueWeight = 15f;
        public float timeBonus = 5f;

        [Header("Checkpoints")]
        public float checkpointInterval = 30f;
        public int maxCheckpoints = 5;
    }
}
