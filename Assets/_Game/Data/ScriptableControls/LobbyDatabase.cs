using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Lobby configuration database for the game.
    /// Contains all lobby parameters.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Lobby Database")]
    public sealed class LobbyDatabase : ScriptableObject
    {
        [Header("Lobby")]
        public string lobbyName = "Asteria Lobby";
        public int maxPlayers = 4;
        public bool isPrivate = false;
        public string region = "auto";

        [Header("Matchmaking")]
        public float matchmakingTimeout = 60f;
        public int minPlayersToStart = 1;
        public bool allowLateJoin = true;

        [Header("Communication")]
        public bool enableVoiceChat = false;
        public bool enableTextChat = true;
        public bool enableEmotes = true;
    }
}
