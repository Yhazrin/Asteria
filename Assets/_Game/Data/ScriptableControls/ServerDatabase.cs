using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Server configuration database for the game.
    /// Contains all server parameters.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Server Database")]
    public sealed class ServerDatabase : ScriptableObject
    {
        [Header("Server")]
        public string serverName = "Asteria Server";
        public string serverDescription = "";
        public int maxPlayers = 4;
        public bool isPrivate = false;
        public string password = "";

        [Header("Gameplay")]
        public string gameMode = "expedition";
        public float gameDuration = 25f;
        public bool allowLateJoin = true;
        public bool allowSpectators = false;

        [Header("Network")]
        public int tickRate = 20;
        public float sendRate = 20f;
        public float receiveRate = 20f;
    }
}
